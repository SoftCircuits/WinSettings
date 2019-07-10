// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace WinSettingsTests
{
    interface ISettingsCommon
    {
        void Save();
        void Load();

        String StringValue { get; set; }
        Char CharValue { get; set; }
        Boolean BooleanValue { get; set; }
        SByte SByteValue { get; set; }
        Byte ByteValue { get; set; }
        Int16 Int16Value { get; set; }
        UInt16 UInt16Value { get; set; }
        Int32 Int32Value { get; set; }
        UInt32 UInt32Value { get; set; }
        Int64 Int64Value { get; set; }
        UInt64 UInt64Value { get; set; }
        Single SingleValue { get; set; }
        Double DoubleValue { get; set; }
        Decimal DecimalValue { get; set; }
        DateTime DateTimeValue { get; set; }
        Byte[] ByteArrayValue { get; set; }
        String[] StringArrayValue { get; set; }
        Func<string> UnsupportedSettingValue { get; set; }
    }
}
