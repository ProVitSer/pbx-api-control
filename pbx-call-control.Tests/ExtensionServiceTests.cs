using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using PbxApiControl.Enums;
using PbxApiControl.Interface;
using PbxApiControl.Models.Extensions;
using PbxApiControl.Services.Pbx;
using TCX.Configuration;
using Xunit;

namespace PbxApiControl.Tests
{
    public class ExtensionServiceTests
    {
        private readonly Mock<ILogger<ExtensionService>> _mockLogger;
        private readonly Mock<PhoneSystem> _mockPhoneSystem;
        private readonly ExtensionService _extensionService;

        public ExtensionServiceTests()
        {
            _mockLogger = new Mock<ILogger<ExtensionService>>();
            _mockPhoneSystem = new Mock<PhoneSystem>();
            _extensionService = new ExtensionService(_mockLogger.Object);
        }

        [Fact]
        public void ExtensionStatus_ShouldReturnCorrectStatus_WhenExtensionExists()
        {
            // Arrange
            var extNumber = "1001";
            var mockExtension = new Mock<Extension>();
            mockExtension.SetupGet(e => e.Number).Returns(extNumber);

            var mockDN = mockExtension.As<DN>();
            _mockPhoneSystem.Setup(ps => ps.GetDNByNumber(extNumber)).Returns(mockDN.Object);

            // Act
            var result = _extensionService.ExtensionStatus(extNumber);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(extNumber, result.ExtensionNumber);
        }
        
    }
}