using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FraudValue=io.confluent.ksql.avro_schemas.KsqlDataSourceSchema;

namespace app {
    class Program {
        static void Main (string[] args) {
            Console.WriteLine ("Starting Consumer!");
            var conf = new Dictionary<string, object> { 
                  { "group.id", "test-consumer-group" },
                  { "bootstrap.servers", "kafka:9092" },
                  { "enable.auto.commit", true },
                  { "auto.commit.interval.ms", 1000 },
                  { "auto.offset.reset", "earliest" },
                  { "schema.registry.url", "http://schema-registry:8081" }
                };

            using (var consumer = new Consumer<string, FraudValue> (conf, 
                new StringDeserializer (Encoding.UTF8), 
                new AvroDeserializer<FraudValue>())) {
                consumer.OnMessage += (_, msg) => 
                  Console.WriteLine ($"Read ('{msg.Key}', '{msg.Value.CREDIT_CARD_NBR}') from: {msg.TopicPartitionOffset}");

                consumer.OnError += (_, error) => 
                  Console.WriteLine ($"Error: {error}");

                consumer.OnConsumeError += (_, msg) => 
                  Console.WriteLine ($"Consume error ({msg.TopicPartitionOffset}): {msg.Error}");

                consumer.Subscribe ("CC_POTENTIAL_FRAUD_COUNTS");

                while (true) {
                    consumer.Poll (TimeSpan.FromMilliseconds (100));
                }
            }
        }
    }
}