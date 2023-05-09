using Confluent.Kafka;
using KafkaConsumerExample.BgServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<KafkaConsumerBgService>();

var app = builder.Build();

app.MapGet("/send-message", () =>
{
    var producer = new ProducerBuilder<Null, string>(new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    }).Build();

    producer.Produce("test-topic", new Message<Null, string>
    {
        Value = "Hello World!"
    });

    return "Message sent!";
});

app.Run();