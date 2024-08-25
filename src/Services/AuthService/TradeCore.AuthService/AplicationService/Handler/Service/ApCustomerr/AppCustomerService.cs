using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Domain.AppCustomerClaimAggregate;
using TradeCore.AuthService.Models.Dtos;
using TradeCore.AuthService.Models.Request.Command.AppCustomer;
using TradeCore.AuthService.Repository;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerClaimRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppOperationClaimRepositoryAggregate;

namespace TradeCore.AuthService.AplicationService.Handler.Service
{
    public class AppCustomerService : IAppCustomerService
    {
        private readonly IAppCustomerService _appCustomerService;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IAppCustomerRepository _appCustomerRepository;
        private readonly IAppCustomerClaimRepository _appCustomerClaimRepository;
        private readonly IAppOperationClaimRepository _appOperationClaimRepository;
        public AppCustomerService(
            IAppCustomerService appCustomerService, 
            IDbContextHandler dbContextHandler,
            IAppCustomerRepository appCustomerRepository,
            IAppCustomerClaimRepository appCustomerClaimRepository,
            IAppOperationClaimRepository appOperationClaimRepository
            )
        {
            _appCustomerService = appCustomerService;
            _dbContextHandler = dbContextHandler;
            _appCustomerRepository = appCustomerRepository;
            _appCustomerClaimRepository = appCustomerClaimRepository;
            _appOperationClaimRepository = appOperationClaimRepository;
        }

        public async Task<AppCustomerDto> CreateUser(CreateAppCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var username = request.Name;

            var userExits = await _appCustomerRepository.FilterByAsync(a => username.Contains(a.Name), cancellationToken);

            if (userExits.Count() > 0)
                username = username + userExits.Count();

            var user = new AppCustomer
            {
                Name = username,
                Address = request.Address,
                Email = request.Email,
                Status = true,
                IsSystemAdmin = false,
            };

            await _appCustomerRepository.SaveAsync(user, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            var getUser = await _appCustomerRepository.FindByAsync(a=>a.Email == request.Email, cancellationToken);
            
            var _funcList = await _appOperationClaimRepository.AllAsync(cancellationToken);
            var _funcs = _funcList.ToList();

            var lstCustomerClaims = new List<AppCustomerClaim>();

            foreach (var item in _funcList)
            {
                lstCustomerClaims.Add(new AppCustomerClaim
                {
                    AppCustomerId=getUser.Id,
                    AppOperationClaimId=item.Id,
                    CreatedCustomerId=getUser.Id,
                    IsActive=true
                });

            }

            await _appCustomerClaimRepository.BulkAddAsync(lstCustomerClaims, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            return new AppCustomerDto();
        }

        public async Task<AppCustomerDto> DeleteUser(DeleteAppCustomerCommandRequest request, CancellationToken cancellationToken)
        {
            var userToDelete = await _appCustomerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (userToDelete != null)
            {
                userToDelete.Status = false;
                userToDelete.IsActive = false;

                _appCustomerRepository.Update(userToDelete, cancellationToken);
                await _dbContextHandler.SaveChangesAsync(cancellationToken);
            }
            return new AppCustomerDto();
        }

        public async Task<AppCustomerDto> UpdateUser(UpdateAppCustomerCommandRequest request, AppCustomer user, CancellationToken cancellationToken)
        {
            var isUserExits = await _appCustomerRepository.FindByAsync(u => u.Id == request.CustomerId, cancellationToken);

            isUserExits.Address = string.IsNullOrEmpty(request.Address) ? isUserExits.Address : request.Email;
            isUserExits.IsSystemAdmin = request.IsSystemAdmin;
            isUserExits.Status = true;

            _appCustomerRepository.Update(isUserExits, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            if (request.AuthList != null && request.AuthList.Count > 0)
            {
                var _funcList = await _appOperationClaimRepository.AllAsync(cancellationToken);
                var _funcs = _funcList.ToList();
                List<Guid> lst = new List<Guid>();
                foreach (var item in request.AuthList)
                {
                    lst.Add(item);

                    var _func = _funcs.FirstOrDefault(a => a.Id == item);
                    if (_func != null)
                    {

                        if (_func.ParentFunctionId != Guid.Empty && !request.AuthList.Contains(_func.ParentFunctionId))
                        {
                            if (!lst.Contains(_func.ParentFunctionId))
                            {
                                lst.Add(_func.ParentFunctionId);
                            }
                        }
                    }
                }
                request.AuthList = lst;
            }

            if (request.AuthList != null && request.AuthList.Count > 0)
            {
                var functLists = await _appCustomerClaimRepository.AllAsync(cancellationToken);
                var functs = functLists.Where(a => a.AppCustomerId == request.CustomerId).ToList();

                if (functs != null && functs.Count > 0)
                {
                    _appCustomerClaimRepository.DeleteRange(functs, cancellationToken);
                    await _dbContextHandler.SaveChangesAsync(cancellationToken);
                }

                List<AppCustomerClaim> userFunctions = new List<AppCustomerClaim>();
                foreach (var item in request.AuthList)
                {
                    userFunctions.Add(new AppCustomerClaim()
                    {
                        AppCustomerId = request.CustomerId,
                        AppOperationClaimId = item
                    });

                }
                await _appCustomerClaimRepository.BulkAddAsync(userFunctions, cancellationToken);
                await _dbContextHandler.SaveChangesAsync(cancellationToken);

            }


            return new AppCustomerDto();
        }
    }
}
