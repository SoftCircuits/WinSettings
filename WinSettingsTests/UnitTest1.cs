// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoftCircuits.EasyEncryption;
using SoftCircuits.WinSettings;
using System;
using System.IO;

namespace WinSettingsTests
{
    [TestClass]
    public class SettingsTests
    {
        [TestMethod]
        public void TestSettings()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WinSettingsTests.txt");
            DateTime now = DateTime.Now;

            foreach (EncryptionAlgorithm algorithm in Enum.GetValues(typeof(EncryptionAlgorithm)))
            {
                ISettingsCommon[] settings = new ISettingsCommon[]
                {
                    new MyIniSettings(path, algorithm),
                    new MyEncryptedIniSettings(path, algorithm),
                    new MyXmlSettings(path, algorithm),
                    new MyEncryptedXmlSettings(path, algorithm),
                    new MyRegistrySettings(algorithm),
                    new MyEncryptedRegistrySettings(algorithm),
                };

                // Init test values
                string stringValue = "Abcdefghijklmnopqrstuvwxyz";
                char charValue = '\x123';
                bool boolValue = true;
                SByte sbyteValue = -128;
                Byte byteValue = 128;
                Int16 int16Value = Int16.MinValue;
                UInt16 uint16Value = UInt16.MaxValue;
                Int32 int32Value = Int32.MinValue;
                UInt32 uint32Value = UInt32.MaxValue;
                Int64 int64Value = Int64.MinValue;
                UInt64 uint64Value = UInt64.MaxValue;
                Single singleValue = 123.456F;
                Double doubleValue = 12345.6789;
                Decimal decimalValue = 123456789.123456789m;
                DateTime datetimeValue = now;
                byte[] byteArrayValue = new byte[] { 88, 89, 90, 91, 92 };
                string[] stringArrayValue = new string[] { "\"abc\"", "def", "g,h,i", "jk\tl", "m n o" };

                foreach (ISettingsCommon setting in settings)
                {
                    // Set values
                    setting.StringValue = stringValue;
                    setting.CharValue = charValue;
                    setting.BooleanValue = boolValue;
                    setting.SByteValue = sbyteValue;
                    setting.ByteValue = byteValue;
                    setting.Int16Value = int16Value;
                    setting.UInt16Value = uint16Value;
                    setting.Int32Value = int32Value;
                    setting.UInt32Value = uint32Value;
                    setting.Int64Value = int64Value;
                    setting.UInt64Value = uint64Value;
                    setting.SingleValue = singleValue;
                    setting.DoubleValue = doubleValue;
                    setting.DecimalValue = decimalValue;
                    setting.DateTimeValue = datetimeValue;
                    setting.ByteArrayValue = byteArrayValue;
                    setting.StringArrayValue = stringArrayValue;
                    setting.UnsupportedSettingValue = () => stringValue;
                    Assert.AreNotEqual(null, setting.UnsupportedSettingValue);

                    // Save values
                    setting.Save();

                    // Reset values
                    setting.StringValue = default;
                    setting.CharValue = default;
                    setting.BooleanValue = default;
                    setting.SByteValue = default;
                    setting.ByteValue = default;
                    setting.Int16Value = default;
                    setting.UInt16Value = default;
                    setting.Int32Value = default;
                    setting.UInt32Value = default;
                    setting.Int64Value = default;
                    setting.UInt64Value = default;
                    setting.SingleValue = default;
                    setting.DoubleValue = default;
                    setting.DecimalValue = default;
                    setting.DateTimeValue = default;
                    setting.ByteArrayValue = default;
                    setting.StringArrayValue = default;
                    setting.UnsupportedSettingValue = default;
                    Assert.AreEqual(null, setting.UnsupportedSettingValue);

                    // Load values
                    setting.Load();

                    // Tests
                    Assert.AreEqual(stringValue, setting.StringValue);
                    Assert.AreEqual(charValue, setting.CharValue);
                    Assert.AreEqual(boolValue, setting.BooleanValue);
                    Assert.AreEqual(sbyteValue, setting.SByteValue);
                    Assert.AreEqual(byteValue, setting.ByteValue);
                    Assert.AreEqual(int16Value, setting.Int16Value);
                    Assert.AreEqual(uint16Value, setting.UInt16Value);
                    Assert.AreEqual(int32Value, setting.Int32Value);
                    Assert.AreEqual(uint32Value, setting.UInt32Value);
                    Assert.AreEqual(int64Value, setting.Int64Value);
                    Assert.AreEqual(uint64Value, setting.UInt64Value);
                    Assert.AreEqual(singleValue, setting.SingleValue);
                    Assert.AreEqual(doubleValue, setting.DoubleValue);
                    Assert.AreEqual(decimalValue, setting.DecimalValue);
                    // TODO: DateTime.Millisecond and DateTime.Kind are not saved
                    Assert.AreEqual(datetimeValue.Year, setting.DateTimeValue.Year);
                    Assert.AreEqual(datetimeValue.Month, setting.DateTimeValue.Month);
                    Assert.AreEqual(datetimeValue.Day, setting.DateTimeValue.Day);
                    Assert.AreEqual(datetimeValue.Hour, setting.DateTimeValue.Hour);
                    Assert.AreEqual(datetimeValue.Minute, setting.DateTimeValue.Minute);
                    Assert.AreEqual(datetimeValue.Second, setting.DateTimeValue.Second);
                    CollectionAssert.AreEqual(byteArrayValue, setting.ByteArrayValue);
                    CollectionAssert.AreEqual(stringArrayValue, setting.StringArrayValue);
                    Assert.AreEqual(null, setting.UnsupportedSettingValue);
                }
            }
            File.Delete(path);
        }
    }
}
