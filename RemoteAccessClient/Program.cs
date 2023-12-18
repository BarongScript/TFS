using Grpc.Net.Client;
using SessionManagementService;

namespace RemoteSystemHost
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5248");
            var client = new RemoteSystem.RemoteSystemClient(channel);
            var request1 = new OpenRemoteSessionRequest
            {
                RemoteUserId = "UserOne",
                RemoteIpAddress = "127.0.0.1"
            };
            var response1 = await client.OpenRemoteSessionAsync(request1);
            var sessionId = response1.SessionId;
            Console.WriteLine($"SessionId = {sessionId}");

            var request2 = new NotifySessionStatusRequest
            {
                SessionId = sessionId,
                EventType = string.IsNullOrEmpty(sessionId) ? "failed" : "opened"
            };

            var response2 = await client.NotifySessionStatusAsync(request2);
            Console.WriteLine($"NotifySessionResponse = {response2}");

            Console.ReadLine();
        }
    }
}

