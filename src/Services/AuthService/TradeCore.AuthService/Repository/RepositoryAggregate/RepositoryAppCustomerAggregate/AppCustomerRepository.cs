using Microsoft.EntityFrameworkCore;
using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Domain.AppCustomerAggregate;
using TradeCore.AuthService.Models.Dtos;
using TradeCore.AuthService.Repository.IRepositoryAggregate.IAppCustomerRepositoryAggregate;

namespace TradeCore.AuthService.Repository.RepositoryAggregate.RepositoryappCustomerAggregate
{
    public class AppCustomerRepository : GenericRepository<AppCustomer>, IAppCustomerRepository
    {
        private readonly ILogger<AppCustomerRepository> _logger;

        public AppCustomerRepository(AuthDbContext context, ILogger<AppCustomerRepository> logger) : base(context, logger)
        {
            _logger = logger;
        }


        public async Task<List<AppCustomerDto>> GetAllUserList(CancellationToken cancellationToken)
        {
            var list = await _entities.AsQueryable().OrderBy(c => c.Name).Select(a => new AppCustomerDto
            {
                AppCustomerId = a.Id,
                Address = a.Address,
                Email = a.Email,
                IsSystemAdmin = a.IsSystemAdmin,
                SessionId = a.SessionId,
                Name = a.Name
            }).ToListAsync(cancellationToken);

            return list;
        }

        public async Task<AppCustomerDto> GetUserById(Guid AppCustomerId, CancellationToken cancellationToken)
        {
            var res = await _entities.AsQueryable().OrderBy(c => c.Name).Where(a => a.Id == AppCustomerId).Select(a => new AppCustomerDto
            {
                AppCustomerId = a.Id,
                Address = a.Address,
                Email = a.Email,
                IsSystemAdmin = a.IsSystemAdmin,
                SessionId = a.SessionId,
                Name = a.Name
            }).FirstOrDefaultAsync(cancellationToken);

            return res;
        }

        public async Task<bool> GetUserByEmail(string email, CancellationToken cancellationToken) => await _entities.AsQueryable().AnyAsync(a => a.Address == email, cancellationToken);
    }
}
