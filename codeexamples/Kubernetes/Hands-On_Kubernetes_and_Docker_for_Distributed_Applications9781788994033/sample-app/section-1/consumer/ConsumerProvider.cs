using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FraudValue=io.confluent.ksql.avro_schemas.KsqlDataSourceSchema;

namespace app {
    public class ConsumerProvider{
        public static Consumer<string,FraudValue> Create() 
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();
            var topicName = config["topic.name"];
            
            var consumerConfig = new Dictionary<string, object>
            {
                { "bootstrap.servers",   config["bootstrap.servers"] },
                { "group.id",            config["group.id"] },
                { "schema.registry.url", config["schema.registry.url"] },
                { "auto.offset.reset",   config["auto.offset.reset"] }
            };
 
            var consumer = new Consumer<string, FraudValue>(
                consumerConfig,
                new StringDeserializer(Encoding.UTF8),
                new AvroDeserializer<FraudValue>());

            consumer.OnError += (_, e)
                => Console.WriteLine("Error: " + e.Reason);

            consumer.OnConsumeError += (_, e)
                => Console.WriteLine("Consume error: " + e.Error.Reason);

            consumer.Subscribe(topicName);
            return consumer;
        }
    }
}