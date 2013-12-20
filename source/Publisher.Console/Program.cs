using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.Impl.v0_9;
using ReferenceData.API;
using Thrift.Protocol;

namespace Publisher.Console
{
    class Program
    {
        static void Main()
        {
            //var factory = new ConnectionFactory() { HostName = "localhost" };
            var factory = new ConnectionFactory { Address = "192.168.1.111" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare("logs", "fanout");

                    while (true)
                    {
                        var message = System.Console.ReadLine();

                        var agency = new Agency
                        {
                            Name = message
                        };

                        var person = new Person
                        {
                            Name = message
                        };

                        var json = Serializer.Serialize(new List<TBase> { agency, person });

                        var body = Encoding.UTF8.GetBytes(json);
                        var basicProperties = channel.CreateBasicProperties();
                        basicProperties.SetPersistent(true);
                        channel.BasicPublish("logs", "", basicProperties, body);
                    }
                }
            }
        }
    }
}
