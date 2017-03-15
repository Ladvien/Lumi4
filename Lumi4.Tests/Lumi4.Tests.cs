using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Lumi4.LumiCommunication.CentralManager;
using Lumi4.LumiCommunication.DataHandling;


// How to write unit tests
// https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Getting-Started-with-Unit-Testing-Part-1
// https://channel9.msdn.com/Shows/Visual-Studio-Toolbox/Getting-Started-with-Unit-Testing-Part-2

// Test naming strategies:
// http://stackoverflow.com/questions/155436/unit-test-naming-best-practices
// UnitOfWork__StateUnderTest__ExpectedBehavior
// http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html
// https://www.petrikainulainen.net/programming/testing/writing-clean-tests-naming-matters/

// Structuring Unit tests
// http://haacked.com/archive/2012/01/02/structuring-unit-tests.aspx/

namespace Lumi4.Tests
{
    [TestClass]
    public class CentralManagerTests
    {
        [TestClass]
        public class TheWifiCentralManagerConstructor
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
        

        [TestClass]
        public class DataConversionTests
        {
            [TestClass]
            public class SeperateStringByCharacterIndexMethod
            {
                [TestMethod]
                public void DataConversion_StringWithThreeSeperatorsFirstIndex_ReturnSeperatedString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex("123.123", 0, '.');
                    Assert.AreEqual("123", seperatedString);
                }

                [TestMethod]
                public void DataConversion_StringWithThreeSeperatorsNegativeIndex_ReturnSeperatedString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex("123.12.3.", -1, '.');
                    Assert.AreEqual("12.3.", seperatedString);
                }

                [TestMethod]
                public void DataConversion_EmptyString_ReturnSeperatedString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex("", 1, '.');
                    Assert.AreEqual("", seperatedString);
                }

                [TestMethod]
                public void DataConversion_StringWithAllSeperators_ReturnSeperatedString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex("....", 1, '.');
                    Assert.AreEqual("...", seperatedString);
                }

                [TestMethod]
                public void DataConversion_StringWithStartIndexThree_ReturnSeperatedString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex(".123.123.123", 0, '.');
                    Assert.AreEqual("123.123.123", seperatedString);
                }

                [TestMethod]
                public void DataConversion_LowNonReadableSeperationChar_ReturnEmptyString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex(".123.123.123", 2, Convert.ToChar(0x02));
                    Assert.AreEqual("", seperatedString);
                }

                [TestMethod]
                public void DataConversion_HighNonReadableSeperationChar_ReturnEmptyString()
                {
                    var seperatedString = DataConversion.SeperateStringByCharacterIndex(".123.123.123", 2, Convert.ToChar(0xFF));
                    Assert.AreEqual("", seperatedString);
                }



            }
        }
    }
} 
