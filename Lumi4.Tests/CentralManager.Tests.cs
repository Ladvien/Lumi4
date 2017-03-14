using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Lumi4.LumiCommunication.CentralManager;


// How to write unit tests
// https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Getting-Started-with-Unit-Testing-Part-1
// https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Getting-Started-with-Unit-Testing-Part-2

// Test naming strategies:
// http://stackoverflow.com/questions/155436/unit-test-naming-best-practices
// UnitOfWork__StateUnderTest__ExpectedBehavior
// http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html
// https://www.petrikainulainen.net/programming/testing/writing-clean-tests-naming-matters/

namespace Lumi4.Tests
{
    [TestClass]
    public class CentralManagerTests
    {
        [TestMethod]
        public void WifiCentralManager_ValidUriProvided_ReturnWifiCentralManagerObj()
        {
            // Arrange
            Uri ip = new Uri("http://192.145.1.1/");
            // Act
            WifiCentralManager wifiCentralManager = new WifiCentralManager(ip);
            // Assert
            Assert.AreEqual(typeof(WifiCentralManager), wifiCentralManager.GetType());
        }

        [TestMethod]
        public void WifiCentralManager_NullProvided_ThrowException()
        {
            try
            {
                var wifiCentralManager = new WifiCentralManager(null);
                Assert.Fail("An exception should have been thrown");
            }
            catch (ArgumentNullException ae)
            {
                Assert.AreEqual("Value cannot be null.", ae.Message);
            }
        }
    }
}
