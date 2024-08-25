using System.Net;
using System.Text;
using TradeCore.EventBus.Base.Abstraction;
using TradeCore.OrderService.Container.Decorator;
using TradeCore.OrderService.CrossCuttingConcerns.Caching;
using TradeCore.OrderService.Domain.CustomerOrderAggregate;
using TradeCore.OrderService.Domain.Helpers;
using TradeCore.OrderService.Domain.ProductAggregate;
using TradeCore.OrderService.Helpers;
using TradeCore.OrderService.IntegrationEvents.EventHandlers;
using TradeCore.OrderService.IntegrationEvents.Events;
using TradeCore.OrderService.Models.Dtos;
using TradeCore.OrderService.Models.Request.Command.CustomerOrder;
using TradeCore.OrderService.Repository;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryCustomerOrderAggregate;
using TradeCore.OrderService.Repository.IRepositoryAggregate.IRepositoryProductAggregate;

namespace TradeCore.OrderService.AplicationService.Handler.Service
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly IEventBus _eventBus;
        private readonly ICacheManager _cacheManager;
        private readonly IConfiguration _configuration;
        private readonly IDbContextHandler _dbContextHandler;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerOrderRepository _customerOrderRepository;

        public CustomerOrderService(
            IEventBus eventBus,
            ICacheManager cacheManager,
            IConfiguration configuration,
            IProductRepository productRepository,
            IDbContextHandler dbContextHandler,
            ICustomerOrderRepository customerOrderRepository)
        {
            _eventBus = eventBus;
            _cacheManager = cacheManager;
            _configuration = configuration;
            _productRepository = productRepository;
            _dbContextHandler = dbContextHandler;
            _customerOrderRepository = customerOrderRepository;
        }

        public async Task<CustomerOrderDto> AddCustomerOrder(AddCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await UserHelper.User;
            var userId = user.AppCustomerId;

            var idControl = await _customerOrderRepository.FindByAsync(p => p.CustomerId == request.CustomerId, cancellationToken);
            if (idControl == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistCustomerId.Code(),
                                             ApplicationMessage.NotExistCustomerId.Message(),
                                             ApplicationMessage.NotExistCustomerId.UserMessage());

            var productList = request.Products.Select(a => new Product
            {
                Id = a.Id,
                Price = a.Price,
                Barcode = a.Barcode,
                Quantity = a.Quantity,
                Description = a.Description
            }).ToList();

            var addProductForCustomer = new CustomerOrder
            {
                Id = Guid.NewGuid(),
                CustomerId = userId,
                Products = productList,
                CreatedUserId = userId,
                CustomerAddress = request.CustomerAddress,
                CustomerName = user.Name,
                CreatedDate = DateTimeHelper.DateTimeUtcTimeZone(),
                IsActive = true,
            };

            await _customerOrderRepository.SaveAsync(addProductForCustomer, cancellationToken);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);


            StringBuilder content = new StringBuilder();
            content.AppendLine("<html><body>");

            content.AppendLine("Merhaba, ");
            content.AppendLine("Sipraişiniz oluşturuldu.");
            content.AppendLine("");
            content.AppendLine("<a href=\"" + _configuration.GetSection("MailSendUrl").Value + "> Order bilgilerini görmek için lütfen tıklayın.</a>");
            content.AppendLine("</body></html>");


            var communicateStartedIntegrationEventHandler = new CommunicateStartedIntegrationEventHandler(_configuration, _eventBus);

            await communicateStartedIntegrationEventHandler.Handle
                (
                    new CommunicateStartedIntegrationEvent(user.Email, _configuration.GetSection("EmailConfiguration").GetSection("SenderName").Value, content.ToString())
                );


            var addedProduct = await _customerOrderRepository.GetCustomerOrder(addProductForCustomer.CustomerId, cancellationToken);

            return addedProduct;
        }

        public async Task<CustomerOrderDto> UpdateCustomerOrder(UpdateCustomerOrderCommandRequest request, CustomerOrder customerOrder, CancellationToken cancellationToken)
        {
            var user = await UserHelper.User;
            var userId = user.AppCustomerId;

            var idControl = await _customerOrderRepository.FindByAsync(p => p.CustomerId == request.CustomerId, cancellationToken);
            if (idControl == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistCustomerId.Code(),
                                             ApplicationMessage.NotExistCustomerId.Message(),
                                             ApplicationMessage.NotExistCustomerId.UserMessage());


            var productIdListNew = request.Products.Select(a=>a.Id);
            //var productIdListOld = idControl.Products.ToList();
            
            var getProducts = await _productRepository.FilterByAsync(a=> productIdListNew.Contains(a.Id));


            var updateProductForCustomer = new CustomerOrder
            {
                Id = idControl.Id,
                CustomerId = idControl.CustomerId,
                Products = getProducts,
                CreatedUserId = userId,
                CustomerAddress = request.CustomerAddress,
                CustomerName = user.Name,
                ModifiedDate = DateTimeHelper.DateTimeUtcTimeZone(),
                IsActive = true,
            };

            _customerOrderRepository.Update(updateProductForCustomer);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);

            StringBuilder content = new StringBuilder();
            content.AppendLine("<html><body>");

            content.AppendLine("Merhaba, ");
            content.AppendLine("Sipraişiniz güncellendi.");
            content.AppendLine("");
            content.AppendLine("<a href=\"" + _configuration.GetSection("MailSendUrl").Value + "> Sipariş bilgilerini görmek için lütfen tıklayın.</a>");
            content.AppendLine("</body></html>");


            var communicateStartedIntegrationEventHandler = new CommunicateStartedIntegrationEventHandler(_configuration, _eventBus);

            await communicateStartedIntegrationEventHandler.Handle
                (
                    new CommunicateStartedIntegrationEvent(user.Email, _configuration.GetSection("EmailConfiguration").GetSection("SenderName").Value, content.ToString())
                );


            var custOrderDto = await _customerOrderRepository.GetCustomerOrder(customerOrder.Id, cancellationToken);

            return custOrderDto;
        }

        public async Task<bool> DeleteCustomerOrder(DeleteCustomerOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await UserHelper.User;
            var userId = user.AppCustomerId;

            var idControl = await _customerOrderRepository.FindByAsync(p => p.Id == request.CustomerId, cancellationToken);
            if (idControl == null)
                throw new BusinessRuleException(ApplicationMessage.NotExistCustomerId.Code(),
                                             ApplicationMessage.NotExistCustomerId.Message(),
                                             ApplicationMessage.NotExistCustomerId.UserMessage());


            _customerOrderRepository.Delete(idControl);
            await _dbContextHandler.SaveChangesAsync(cancellationToken);


            StringBuilder content = new StringBuilder();
            content.AppendLine("<html><body>");

            content.AppendLine("Merhaba, ");
            content.AppendLine("Sipraişiniz silindi.");
            content.AppendLine("");
            content.AppendLine("<a href=\"" + _configuration.GetSection("MailSendUrl").Value + "> Order bilgilerini görmek için lütfen tıklayın.</a>");
            content.AppendLine("</body></html>");


            var communicateStartedIntegrationEventHandler = new CommunicateStartedIntegrationEventHandler(_configuration, _eventBus);

            await communicateStartedIntegrationEventHandler.Handle
                (
                    new CommunicateStartedIntegrationEvent(user.Email, _configuration.GetSection("EmailConfiguration").GetSection("SenderName").Value, content.ToString())
                );



            return true;
        }

    }
}
