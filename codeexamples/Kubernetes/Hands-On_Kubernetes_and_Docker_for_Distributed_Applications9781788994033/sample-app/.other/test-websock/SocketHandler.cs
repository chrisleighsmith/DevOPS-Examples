using System;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace app {
    public class SocketHandler {
        public const int BufferSize = 4096;

        WebSocket socket;

        SocketHandler(WebSocket socket)
        {
            this.socket = socket;
        }
        async Task EchoLoop()
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            while (this.socket.State == WebSocketState.Open)
            {
                var incoming = await this.socket.ReceiveAsync(seg, CancellationToken.None);
                var outgoing = new ArraySegment<byte>(buffer, 0, incoming.Count);
                await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        static async Task Acceptor(HttpContext hc, Func<Task> n)
        {
            if (!hc.WebSockets.IsWebSocketRequest)
                return;

            var socket = await hc.WebSockets.AcceptWebSocketAsync();
            var h = new SocketHandler(socket);
            //await h.EchoLoop();

            await h.Send();
        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }


        //=====================
        void WriteJsonToStream(Stream stream, object data)
        {
            using (var streamWriter = new StreamWriter(stream))
            using (var writer = new JsonTextWriter(streamWriter))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, data);
            }
        }

        public async Task SendTextJson(object data)
        {
            using (var stream = new MemoryStream())
            {
                WriteJsonToStream(stream, data);
                var outgoing = new ArraySegment<byte>(stream.ToArray());
                await this.socket.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task Send(){
            while (this.socket.State == WebSocketState.Open)
            {
                dynamic data = new { name = "Sample", time = DateTime.Now };
                await SendTextJson(data);
                await Task.Delay(1000);
            }
            Console.WriteLine("Connection closed...");
        }
    }
}