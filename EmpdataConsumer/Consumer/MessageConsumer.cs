using Common;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace EmpdataConsumer
{
    internal class MessageConsumer : BackgroundService
    {
        private readonly EmployeeHub _employeeHub;        
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IConfiguration _configuration;

        public MessageConsumer(EmployeeHub employeeHub,
                               IServiceProvider serviceProvider ,
                               ILogger<MessageConsumer> logger,
                               IConfiguration configuration)
        {
            _employeeHub = employeeHub;            
            _serviceProvider = serviceProvider;
            _logger= logger;
            _configuration = configuration; 
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            using var consumer = new ConsumerBuilder<Ignore,string>(GetConfig()).Build();
            consumer.Subscribe(_configuration.GetValue <string>("ProducerTopic"));
            try
            {
                Employee? emp = null;
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume();
                                     
                    _logger.LogTrace($"Message received from {consumeResult.TopicPartitionOffset}: {consumeResult.Message.Value}");
                    
                    if (consumeResult?.Message != null)
                    {
                        emp = JsonConvert.DeserializeObject<Employee>(consumeResult.Message.Value);
                        if (emp != null)
                        {
                            await PersistEmployee(emp);
                            await _employeeHub.SendMessage(emp);
                        }
                    }
                }
            }
            catch (OperationCanceledException opex)
            {
                _logger.LogCritical("Exception occured", opex.Message);
            }
            finally
            {
                consumer.Close();                
            }            
        }

        protected ConsumerConfig GetConfig()
        {
            return new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                ClientId = "my-app",
                GroupId = "my-group",
                BrokerAddressFamily = BrokerAddressFamily.V4,
            };
        }

        protected async Task PersistEmployee(Employee emp)
        {
            using var scope = _serviceProvider.CreateScope();
            using var employeeRepository = scope.ServiceProvider.GetRequiredService<IEmployeeRepository>();
            await employeeRepository.AddEmployeeAsync(emp);
            await employeeRepository.SaveAsync();
            _logger.LogInformation("Employee data published to SignalR clients");
        }
    } 
}
