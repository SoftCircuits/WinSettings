// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.WinSettings;
using System;

namespace WinSettingsTests
{
    public class MyEncryptedIniSettings : IniSettings, ISettings
    {
        public MyEncryptedIniSettings(string filePath)
            : base(filePath, "Password123")
        {
        }

        [EncryptedSetting]
        public String StringValue { get; set; }
        [EncryptedSetting]
        public Char CharValue { get; set; }
        [EncryptedSetting]
        public Boolean BooleanValue { get; set; }
        [EncryptedSetting]
        public SByte SByteValue { get; set; }
        [EncryptedSetting]
        public Byte ByteValue { get; set; }
        [EncryptedSetting]
        public Int16 Int16Value { get; set; }
        [EncryptedSetting]
        public UInt16 UInt16Value { get; set; }
        [EncryptedSetting]
        public Int32 Int32Value { get; set; }
        [EncryptedSetting]
        public UInt32 UInt32Value { get; set; }
        [EncryptedSetting]
        public Int64 Int64Value { get; set; }
        [EncryptedSetting]
        public UInt64 UInt64Value { get; set; }
        [EncryptedSetting]
        public Single SingleValue { get; set; }
        [EncryptedSetting]
        public Double DoubleValue { get; set; }
        [EncryptedSetting]
        public Decimal DecimalValue { get; set; }
        [EncryptedSetting]
        public DateTime DateTimeValue { get; set; }
        [EncryptedSetting]
        public Byte[] ByteArrayValue { get; set; }
        [EncryptedSetting]
        public String[] StringArrayValue { get; set; }
        [ExcludedSetting]
        public String ExcludedStringValue { get; set; }
    }
}
