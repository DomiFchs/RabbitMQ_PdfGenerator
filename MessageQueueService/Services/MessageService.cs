using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model.Entities;
using RabbitMQ.Client;

namespace MessageQueueService.Services; 

public class MessageService {
    
    private readonly ConnectionFactory? _factory;
    private readonly ILogger<MessageService> _logger;
    
    public MessageService(IConfiguration config, ILogger<MessageService> logger) {
        _logger = logger;
        var factory = new ConnectionFactory { HostName = "rabbitmqServer", Port = 5672, UserName = "guest", Password = "guest" };
        _factory = factory;
    }
    
    public void SendMessage(List<ToDo> toDos) {
        try {
            using var connection = _factory!.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "Todos transfer", durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            var todosJson = JsonSerializer.Serialize(toDos);
            var body = Encoding.UTF8.GetBytes(todosJson);

            channel.BasicPublish(exchange: string.Empty, routingKey: "Todos transfer",basicProperties:null, body: body);

            _logger.LogInformation(" [x] Sent {0}", todosJson);
        }
        catch (OperationCanceledException) {
            _logger.LogInformation("Operation cancelled");
        }
        catch (Exception e) {
            _logger.LogError(e.Message);
        }
    }
}