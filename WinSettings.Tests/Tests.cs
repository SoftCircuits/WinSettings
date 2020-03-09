// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WinSettingsTests.dat");

            Test(new MyIniSettings(path));
            Test(new MyEncryptedIniSettings(path));
            Test(new MyXmlSettings(path));
            Test(new MyEncryptedXmlSettings(path));
            Test(new MyRegistrySettings());
            Test(new MyEncryptedRegistrySettings());
            File.Delete(path);
        }

        private void Test(ISettings settings)
        {
            foreach (SettingsData data in SettingsData.TestData)
            {
                // Set values
                settings.StringValue = data.StringValue;
                settings.CharValue = data.CharValue;
                settings.BooleanValue = data.BooleanValue;
                settings.SByteValue = data.SByteValue;
                settings.ByteValue = data.ByteValue;
                settings.Int16Value = data.Int16Value;
                settings.UInt16Value = data.UInt16Value;
                settings.Int32Value = data.Int32Value;
                settings.UInt32Value = data.UInt32Value;
                settings.Int64Value = data.Int64Value;
                settings.UInt64Value = data.UInt64Value;
                settings.SingleValue = data.SingleValue;
                settings.DoubleValue = data.DoubleValue;
                settings.DecimalValue = data.DecimalValue;
                settings.DateTimeValue = data.DateTimeValue;
                settings.ByteArrayValue = data.ByteArrayValue;
                settings.StringArrayValue = data.StringArrayValue;

                settings.ExcludedStringValue = "Abcdefghijklmnopqrstuvwxyz";
                Assert.AreNotEqual(null, settings.ExcludedStringValue);

                // Save values
                settings.Save();

                // Reset values
                settings.StringValue = default;
                settings.CharValue = default;
                settings.BooleanValue = default;
                settings.SByteValue = default;
                settings.ByteValue = default;
                settings.Int16Value = default;
                settings.UInt16Value = default;
                settings.Int32Value = default;
                settings.UInt32Value = default;
                settings.Int64Value = default;
                settings.UInt64Value = default;
                settings.SingleValue = default;
                settings.DoubleValue = default;
                settings.DecimalValue = default;
                settings.DateTimeValue = default;
                settings.ByteArrayValue = default;
                settings.StringArrayValue = default;

                settings.ExcludedStringValue = default;
                Assert.AreEqual(null, settings.ExcludedStringValue);

                // Load values
                settings.Load();

                // Tests
                Assert.AreEqual(data.StringValue, settings.StringValue);
                Assert.AreEqual(data.CharValue, settings.CharValue);
                Assert.AreEqual(data.BooleanValue, settings.BooleanValue);
                Assert.AreEqual(data.SByteValue, settings.SByteValue);
                Assert.AreEqual(data.ByteValue, settings.ByteValue);
                Assert.AreEqual(data.Int16Value, settings.Int16Value);
                Assert.AreEqual(data.UInt16Value, settings.UInt16Value);
                Assert.AreEqual(data.Int32Value, settings.Int32Value);
                Assert.AreEqual(data.UInt32Value, settings.UInt32Value);
                Assert.AreEqual(data.Int64Value, settings.Int64Value);
                Assert.AreEqual(data.UInt64Value, settings.UInt64Value);
                Assert.AreEqual(data.SingleValue, settings.SingleValue);
                Assert.AreEqual(data.DoubleValue, settings.DoubleValue);
                Assert.AreEqual(data.DecimalValue, settings.DecimalValue);
                // NOTE: DateTime.Millisecond and DateTime.Kind are not saved
                Assert.AreEqual(data.DateTimeValue.Year, settings.DateTimeValue.Year);
                Assert.AreEqual(data.DateTimeValue.Month, settings.DateTimeValue.Month);
                Assert.AreEqual(data.DateTimeValue.Day, settings.DateTimeValue.Day);
                Assert.AreEqual(data.DateTimeValue.Hour, settings.DateTimeValue.Hour);
                Assert.AreEqual(data.DateTimeValue.Minute, settings.DateTimeValue.Minute);
                Assert.AreEqual(data.DateTimeValue.Second, settings.DateTimeValue.Second);
                CollectionAssert.AreEqual(data.ByteArrayValue, settings.ByteArrayValue);
                CollectionAssert.AreEqual(data.StringArrayValue, settings.StringArrayValue);

                // Test excluded property is not saved/loaded
                Assert.AreEqual(null, settings.ExcludedStringValue);
            }
        }
    }
}
