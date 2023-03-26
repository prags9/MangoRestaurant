using Azure.Messaging.ServiceBus;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Repository;
using Mango.Services.ShoppingCartAPI.Messages;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.OrderAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionSring;
        private readonly string serviceBusTopic;
        private readonly string serviceBusSubscription;
        private readonly IConfiguration _configuration;
        private readonly IOrderRepository _orderRepository;

        private ServiceBusProcessor checkoutProcessor;

        public AzureServiceBusConsumer(IOrderRepository orderRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _configuration = configuration;
            serviceBusConnectionSring = _configuration.GetValue<string>("ServiceBusConnectionString");
            serviceBusTopic = _configuration.GetValue<string>("CheckoutMessageTopic");
            serviceBusSubscription = _configuration.GetValue<string>("CheckoutSubscription");

            var client = new ServiceBusClient(serviceBusConnectionSring);
            checkoutProcessor = client.CreateProcessor(serviceBusTopic, serviceBusSubscription);

        }

        public async Task Stop()
        {             
            await checkoutProcessor.StopProcessingAsync();
            await checkoutProcessor.DisposeAsync();
        }

        public async Task Start()
        {
            checkoutProcessor.ProcessMessageAsync += OnCheckoutMessageReceived;
            checkoutProcessor.ProcessErrorAsync += ErrorHandler;
            await checkoutProcessor.StartProcessingAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnCheckoutMessageReceived(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CheckoutHeaderDto checkoutHeaderDto = JsonConvert.DeserializeObject<CheckoutHeaderDto>(body);

            OrderHeader orderHeader = new()
            {
                CardNUmber = checkoutHeaderDto.CardNUmber,
                CartTotalItems = checkoutHeaderDto.CartTotalItems,
                CouponCode = checkoutHeaderDto.CouponCode,
                CVV = checkoutHeaderDto.CVV,
                DiscountTotal = checkoutHeaderDto.DiscountTotal,
                Email = checkoutHeaderDto.Email,
                ExpiryMonthYear = checkoutHeaderDto.ExpiryMonthYear,
                FirstName = checkoutHeaderDto.FirstName,
                LastName = checkoutHeaderDto.LastName,
                OrderDetails = new List<OrderDetail>(),
                OrderTime = DateTime.Now,
                OrderTotal = checkoutHeaderDto.OrderTotal,
                paymentStatus = false,
                Phone = checkoutHeaderDto.Phone,
                PickUpDataTime = checkoutHeaderDto.PickUpDataTime,
                UserId = checkoutHeaderDto.UserId

            };

            foreach (var item in checkoutHeaderDto.CartDetails)
            {
                OrderDetail orderDetail = new()
                {
                    Count = item.Count,
                    ProductId = item.ProductId,
                    Productname = item.Product.Name,
                    Price = item.Product.Price
                };
                orderHeader.CartTotalItems += orderDetail.Count;
                orderHeader.OrderDetails.Add(orderDetail);

                await _orderRepository.AddOrder(orderHeader);
            }
        }
    }
}
