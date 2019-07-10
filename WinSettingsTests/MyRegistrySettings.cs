// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.EasyEncryption;
using SoftCircuits.WinSettings;
using System;

namespace WinSettingsTests
{
    public class MyRegistrySettings : RegistrySettings, ISettingsCommon
    {
        public MyRegistrySettings(EncryptionAlgorithm algorithm)
            : base("SoftCircuits", "WinSettingsTests", RegistrySettingsType.CurrentUser, new Encryption("@Password123", algorithm))
        {
        }

        public String StringValue { get; set; }
        public Char CharValue { get; set; }
        public Boolean BooleanValue { get; set; }
        public SByte SByteValue { get; set; }
        public Byte ByteValue { get; set; }
        public Int16 Int16Value { get; set; }
        public UInt16 UInt16Value { get; set; }
        public Int32 Int32Value { get; set; }
        public UInt32 UInt32Value { get; set; }
        public Int64 Int64Value { get; set; }
        public UInt64 UInt64Value { get; set; }
        public Single SingleValue { get; set; }
        public Double DoubleValue { get; set; }
        public Decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public Byte[] ByteArrayValue { get; set; }
        public String[] StringArrayValue { get; set; }
        [ExcludedSetting]
        public Func<string> UnsupportedSettingValue { get; set; }
    }
}
