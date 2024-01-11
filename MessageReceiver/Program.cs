// See https://aka.ms/new-console-template for more information

using MessageReceiver.Services;
using RabbitMQ.Client;

public class Program
{
    private const string UserName = "guest";
    private const string Password = "guest";
    private const string HostName = "localhost";

    private static void Main(string[] args)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = HostName,
            UserName = UserName,
            Password = Password,
            Port = 15672
        };
        var connection = connectionFactory.CreateConnection();
        var channel = connection.CreateModel();
        
        var messageReceiver = new MessageReceiveService(channel);
        channel.BasicQos(0, 1, false);
        channel.BasicConsume("Todos Queue", true, messageReceiver);
        Console.ReadLine();
    }
}