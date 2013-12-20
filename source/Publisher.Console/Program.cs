using System.Text;
using RabbitMQ.Client;

namespace Publisher.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            var factory = new ConnectionFactory() { Address = "192.168.1.111" }; using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("logs", "fanout");

                while (true)
                {
                    var message = System.Console.ReadLine();
                    var body = Encoding.UTF8.GetBytes(message);
                    var basicProperties = channel.CreateBasicProperties();
                    basicProperties.SetPersistent(true);
                    channel.BasicPublish("logs", "", basicProperties, body);
                }
            }
        }
    }
}
