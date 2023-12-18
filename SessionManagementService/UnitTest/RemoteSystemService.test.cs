using Grpc.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SessionManagementService.Services;

namespace SessionManagementService.Tests
{
    [TestClass]
    public class RemoteSystemServiceTests
    {
        [TestMethod]
        public async Task OpenRemoteSession_Allowed()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoteSystemService>>();
            var service = new RemoteSystemService(loggerMock.Object);
            var request = new OpenRemoteSessionRequest();

            // Act
            var response = await service.OpenRemoteSession(request, null);

            // Assert
            Assert.IsNotNull(response.SessionId);
        }

        [TestMethod]
        public async Task OpenRemoteSession_Denied()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoteSystemService>>();
            var mockContext = new Mock<ServerCallContext>();
            var service = new RemoteSystemService(loggerMock.Object);
            var request = new OpenRemoteSessionRequest
            {
                RemoteUserId = "UserUnknown",
                RemoteIpAddress = "127.0.0.1"
            };
            var expectedResponse = new OpenRemoteSessionResponse();

            // Set up a condition for denying the session
            service.SetupForDeniedSession();

            // Act
            var response = await service.OpenRemoteSession(request, mockContext.Object);

            // Assert
            Assert.AreEqual(response, expectedResponse);
            Assert.AreEqual(response.SessionId, "");
        }

        [TestMethod]
        public async Task NotifySessionStatus_ValidEvent()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoteSystemService>>();
            var service = new RemoteSystemService(loggerMock.Object);
            var request = new NotifySessionStatusRequest { EventType = "opened" };

            // Act
            var response = await service.NotifySessionStatus(request, null);

            // Assert
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task NotifySessionStatus_InvalidEvent()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<RemoteSystemService>>();
            var mockContext = new Mock<ServerCallContext>();
            var service = new RemoteSystemService(loggerMock.Object);
            var request = new NotifySessionStatusRequest { EventType = "failed" };
            var expectedResponse = new NotifySessionStatusResponse();

            // Act
            var response = await service.NotifySessionStatus(request, mockContext.Object);

            // Assert
            Assert.AreEqual(response, expectedResponse);
        }
    }
}

