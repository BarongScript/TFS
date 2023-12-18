using Grpc.Core;

namespace SessionManagementService.Services
{
    public class RemoteSystemService : RemoteSystem.RemoteSystemBase
    {
        private bool isAllowOpenSession = true;
        private readonly ILogger<RemoteSystemService> _logger;
        public RemoteSystemService(ILogger<RemoteSystemService> logger)
        {
            _logger = logger;
        }

        public override Task<OpenRemoteSessionResponse> OpenRemoteSession(OpenRemoteSessionRequest request, ServerCallContext context)
        {
            if (this.IsAllowedToOpenSession(request))
            {
                return Task.FromResult(new OpenRemoteSessionResponse { SessionId = GenerateSessionId() });
            }
            else
            {
                context.Status = new Status(StatusCode.PermissionDenied, "Access denied");
                return Task.FromResult(new OpenRemoteSessionResponse());
            }
        }

        public override Task<NotifySessionStatusResponse> NotifySessionStatus(NotifySessionStatusRequest request, ServerCallContext context)
        {
            if (IsValidEvent(request.EventType))
            {
                Console.WriteLine($"Received event for session {request.SessionId}: {request.EventType}");
                return Task.FromResult(new NotifySessionStatusResponse());
            }
            else
            {
                context.Status = new Status(StatusCode.Internal, "Invalid event type");
                return Task.FromResult(new NotifySessionStatusResponse());
            }
        }

        internal void SetupForDeniedSession()
        {
            isAllowOpenSession = false;

        }

        private bool IsAllowedToOpenSession(OpenRemoteSessionRequest request)
        {
            // Implement logic whether it is allowed to establish a remote session
            // and return the appropriate response.
            return isAllowOpenSession;
        }

        private string GenerateSessionId()
        {
            // Implement logic to generate a unique session ID.
            return Guid.NewGuid().ToString();
        }

        private bool IsValidEvent(string eventType)
        {
            // Implement logic to validate the event type.
            switch (eventType)
            {
                case "opened": break;
                case "closed": break;
                case "approved": break;
                case "rejected": break;
                case "timeout": break;
                default: return false;
            }
            // and return the appropriate response.
            return true;
        }
    }
}