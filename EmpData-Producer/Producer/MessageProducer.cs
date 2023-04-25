using Common;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EmpDataProducer
{
    public class MessageProducer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageProducer> _logger;

        public MessageProducer(ILogger<MessageProducer> logger,
                             IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using
            var producer = new ProducerBuilder<Null, string>(GetConfig()).Build();

            var randomId = new Random(10);
            var randomHourlyRate = new Random();
            var randomHoursWorked = new Random();

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000);
                    var emp = new Employee
                    {
                        Name = "Employee" + randomId.Next().ToString(),
                        HourlyRate = randomHourlyRate.Next(1, 10),
                        HoursWorked = randomHoursWorked.Next(1, 8)
                    };
                    var message = new Message<Null, string>
                    {
                        Value = JsonConvert.SerializeObject(emp)
                    };                    
                    await producer.ProduceAsync(_configuration.GetValue<string>("ProducerTopic"), message);                    
                    _logger.LogTrace($"Message published {message.Value}");
                }
            } 
            catch (OperationCanceledException opex)
            {
                _logger.LogCritical("Exception occured", opex.Message);
            }
            finally
            {
                producer.Dispose();
            }

        }

        protected ProducerConfig GetConfig()
        {
            return new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                ClientId = "my-app",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };

        }
    }
}
