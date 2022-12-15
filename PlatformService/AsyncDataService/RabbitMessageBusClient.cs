using System.Text;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsynDataService
{
    public class RabbitMessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private IModel _channel;

        public RabbitMessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
                // UserName = "guest",
                // Password = "guest"
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to message bus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Cannot connect to the mesage bus {ex.Message}");
            }

        }


        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection is open. sending Message ...");
                _channel.BasicPublish(exchange: "trigger",
                                      routingKey: "",
                                      basicProperties: null,
                                      body: body);

                Console.WriteLine($"--> We have sent {message}");



            }
            else
            {
                Console.WriteLine("RabbitMQ connection is closed. Message not sending.");
            }
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();

            }
        }
    }
}