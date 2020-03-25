using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using FraudValue=io.confluent.ksql.avro_schemas.KsqlDataSourceSchema;

namespace app {
    public class SocketHandler {
        WebSocket _socket;
        SocketHandler(WebSocket socket)
        {
            _socket = socket;
        }
        void WriteJsonToStream(Stream stream, object data)
        {
            using (var streamWriter = new StreamWriter(stream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }
        async Task SendTextJson(object data)
        {
            using (var stream = new MemoryStream())
            {
                WriteJsonToStream(stream, data);
                var outgoing = new ArraySegment<byte>(stream.ToArray());
                await _socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        async Task HandleMessage(Message<string, FraudValue> msg)
        {
            Console.Write(".");
            dynamic data = new { 
                creditCardNbr = msg.Value.CREDIT_CARD_NBR, 
                attempts = msg.Value.ATTEMPTS,
            };
            await SendTextJson(data);
        }
        void PollLoop()
        {
            Console.WriteLine("---> Starting PollLoop");

            using(var consumer = ConsumerProvider.Create()){
                consumer.OnMessage += async (_, msg) 
                    => await HandleMessage(msg);

                while (_socket.State == WebSocketState.Open)
                {
                    consumer.Poll(100);
                }
            }
        }
        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;
            
            Console.WriteLine("---> Creating WebSocket");

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            h.PollLoop();
        }
        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }
    }
}