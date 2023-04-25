using Common;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace EmpDataProducer
{
    public class MessageProducer : BackgroundService
    {
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using
            var producer = new ProducerBuilder<Null, string>(GetConfig()).Build();

            var randomId = new Random(10);
            var randomHourlyRate = new Random();
            var randomHoursWorked = new Random();


            while (true)
            {
                await Task.Delay(1000);
                var emp = new Employee
                {
                    Name = "Employee" + randomId.Next().ToString(),
                    HourlyRate = randomHourlyRate.Next(1, 10),
                    HoursWorked = randomHoursWorked.Next(1, 8)
                };
                var message = new Message<Null,string>
                {
                    Value = JsonConvert.SerializeObject(emp)
                };
                await producer.ProduceAsync("topic-employee", message);                
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
