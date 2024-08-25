using AutoMapper;
using System.Security.Claims;
using TradeCore.AuthService.CrossCuttingConcerns.Caching;
using TradeCore.AuthService.Domain.AppOperationClaimAggregate;
using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Domain.Helpers;
using TradeCore.AuthService.Models.Dtos;
using TradeCore.AuthService.Models.Request.Query.Auth;
using TradeCore.AuthService.Models.Response;
using TradeCore.AuthService.Models.Response.Query.Auth;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAuthRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Command.Auth
{
    public class AuthLoginServiceCommandHandler : RequestHandlerBase<AuthLoginServiceRequest, AuthLoginServiceResponse>
    {

        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IAppCustomerRepository _appCustomerRepository;
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public AuthLoginServiceCommandHandler(
            IMapper mapper,
            ICacheManager cacheManager,
            ITokenRepository tokenRepository,
            IAppCustomerRepository appCustomerRepository,
            IAppOperationClaimRepository appOperationClaimRepository,
            IAppCustomerClaimRepository appCustomerClaimRepository
            )
        {
            _mapper = mapper;
            _cacheManager = cacheManager;
            _tokenRepository = tokenRepository;
            _appCustomerRepository = appCustomerRepository;
            _appCustomerClaimRepository = appCustomerClaimRepository;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public override async Task<ResponseBase<AuthLoginServiceResponse>> Handle(AuthLoginServiceRequest request, CancellationToken cancellationToken)
        {
            AppCustomerDto usermodel = new AppCustomerDto();
            UserHelperModel userInfo = new UserHelperModel();

            var seesionIdGuid = Guid.NewGuid().ToString();

            var user = await _appCustomerRepository.FindByAsync(u => u.Name == request.Name && u.Status, cancellationToken);

            if (user is null)
                return new ResponseBase<AuthLoginServiceResponse> { Data = new AuthLoginServiceResponse(), Success = false, Message = "Kullanıcı adı / Şifre hatalı" };


            List<AppOperationClaim> claims = new List<AppOperationClaim>();

            if (user.IsSystemAdmin)
                claims = await _appOperationClaimRepository.AllAsync(cancellationToken);
            else
            {
                var userList = await _appCustomerRepository.AllAsync(cancellationToken);
                var operationClaimList = await _appOperationClaimRepository.AllAsync(cancellationToken);
                var userClaimList = await _appCustomerClaimRepository.AllAsync(cancellationToken);

                var result = (from userx in userList
                             join userClaimx in userClaimList on userx.Id equals userClaimx.AppCustomerId
                             join operationClaimx in operationClaimList on userClaimx.AppOperationClaimId equals operationClaimx.Id
                             where userx.Id == user.Id 

                             select new
                             {
                                 OperationClaimId=operationClaimx.Id,
                                 operationClaimx.FunctionName
                             }
               );

                claims = result.Select(x => new AppOperationClaim { FunctionName = x.FunctionName, Id = x.OperationClaimId }).Distinct().ToList();


            }


            var userAuthTags = claims.Select(a => a.FunctionName).ToList();


            await _cacheManager.Add($"{CacheKeys.UserIdForClaim}={user.Id}", claims.Select(x => x.FunctionName), 8765);


            List<AppCustomerClaim> appCustomerClaim = new List<AppCustomerClaim>();
            foreach (var item in claims)
            {
                appCustomerClaim.Add(new AppCustomerClaim
                {
                    Id = user.Id,
                    AppOperationClaimId = item.Id                    
                });
            }

            user.SessionId = seesionIdGuid;
            usermodel = _mapper.Map<AppCustomerDto>(user);
            usermodel.SessionId = seesionIdGuid;

            usermodel.AppCustomerClaims = appCustomerClaim;

            await _cacheManager.Add($"{CacheKeys.UserSession}={usermodel.SessionId}", usermodel, 8765);
            await _cacheManager.Add($"{CacheKeys.UserSessionForUserId}={user.Id}", usermodel.SessionId, 8765);


            var claimsToken = new List<Claim>
            {
                new Claim(ClaimTypes.Name, request.Name),
                new Claim(ClaimTypes.Role, userInfo.IsAdUser  ? "Admin":"User")
            };

            var accessToken = _tokenRepository.GenerateAccessToken(user);
            var refreshToken = _tokenRepository.GenerateRefreshToken();

            usermodel.Token = accessToken.Token;

            return new ResponseBase<AuthLoginServiceResponse>
            {
                Data = new AuthLoginServiceResponse { Token = accessToken.Token, RefreshToken = refreshToken, CustomerInfo = usermodel },
                Success = true,
                Message = "Giriş işlemi başarılı"
            };

        }
    }
}