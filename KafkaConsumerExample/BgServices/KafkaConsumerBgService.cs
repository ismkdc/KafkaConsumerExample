using Confluent.Kafka;

namespace KafkaConsumerExample.BgServices;

public class KafkaConsumerBgService : BackgroundService
{
    private readonly IConsumer<Null, string> _consumer;

    public KafkaConsumerBgService()
    {
        _consumer = new ConsumerBuilder<Null, string>(new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "test-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        }).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() => ConsumeMessages(stoppingToken), stoppingToken);
    }

    private void ConsumeMessages(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("test-topic");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var data = _consumer.Consume(stoppingToken);
            Console.WriteLine(data.Message.Value);
        }
    }
}