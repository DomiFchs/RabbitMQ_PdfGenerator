// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Model.Entities;
using PDFGenerator.Services;
using QuestPDF.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PDFGenerator;

public class Program
{
    private static void Main(string[] args)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        var factory = new ConnectionFactory { HostName = "rabbitmqServer", Port = 5672, UserName = "guest", Password = "guest" };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();
        
        channel.QueueDeclare("Todos transfer", false, false, false, null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var todos = JsonSerializer.Deserialize<List<ToDo>>(body);
            Console.WriteLine($"Consuming Message");
            if (todos == null) return;
            
            var pdfService = new PdfService();
            pdfService.Generate(todos);
        }; 
        channel.BasicConsume(queue: "Todos transfer", autoAck: true, consumer: consumer, noLocal: false, exclusive: false, arguments: null);

        Thread.Sleep(Timeout.Infinite);
    }
}
