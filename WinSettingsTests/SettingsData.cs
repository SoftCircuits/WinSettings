// Copyright (c) 2019-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace WinSettingsTests
{
    public class SettingsData
    {
        public String? StringValue { get; set; }
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
        public Byte[]? ByteArrayValue { get; set; }
        public String[]? StringArrayValue { get; set; }

        public static readonly SettingsData[] TestData =
        {
            new SettingsData    // Min values
            {
                StringValue = string.Empty,
                CharValue = Char.MinValue,
                BooleanValue = false,
                SByteValue = SByte.MinValue,
                ByteValue = Byte.MinValue,
                Int16Value = Int16.MinValue,
                UInt16Value = UInt16.MinValue,
                Int32Value = Int32.MinValue,
                UInt32Value = UInt32.MinValue,
                Int64Value = Int64.MinValue,
                UInt64Value = UInt64.MinValue,
                SingleValue = 0,    // Single.MinValue, TODO: Bug fixed in .NET Core 3.0
                DoubleValue = 0,    // Double.MinValue + 1, TODO: Bug fixed in .NET Core 3.0
                DecimalValue = Decimal.MinValue,
                DateTimeValue = DateTime.MinValue,
                ByteArrayValue = new Byte[] { },
                StringArrayValue = new String[] { },
            },
            new SettingsData    // Max values
            {
                StringValue = "Abcdefghijklmnopqrstuvwxyz1234567890",
                CharValue = Char.MaxValue,
                BooleanValue = true,
                SByteValue = SByte.MaxValue,
                ByteValue = Byte.MaxValue,
                Int16Value = Int16.MaxValue,
                UInt16Value = UInt16.MaxValue,
                Int32Value = Int32.MaxValue,
                UInt32Value = UInt32.MaxValue,
                Int64Value = Int64.MaxValue,
                UInt64Value = UInt64.MaxValue,
                SingleValue = 0,    // Single.MaxValue, TODO: Bug fixed in .NET Core 3.0
                DoubleValue = 0,    // Double.MaxValue, TODO: Bug fixed in .NET Core 3.0
                DecimalValue = Decimal.MaxValue,
                DateTimeValue = DateTime.MaxValue,
                ByteArrayValue = new Byte[]
                {
                    Byte.MaxValue,
                    Byte.MinValue,
                    Byte.MaxValue,
                    Byte.MinValue
                },
                StringArrayValue = new String[]
                {
                    "Abcdefghijklmnopqrstuvwxyz1234567890",
                    "0987654321zyxwvutsrqponmlkjihgfedcbA",
                    "Abcdefghijklmnopqrstuvwxyz1234567890",
                    "0987654321zyxwvutsrqponmlkjihgfedcbA",
                },
            },
            new SettingsData    // Zeros and empty
            {
                StringValue = string.Empty,
                CharValue = '\0',
                BooleanValue = false,
                SByteValue = 0,
                ByteValue = 0,
                Int16Value = 0,
                UInt16Value = 0,
                Int32Value = 0,
                UInt32Value = 0,
                Int64Value = 0,
                UInt64Value = 0,
                SingleValue = 0,
                DoubleValue = 0,
                DecimalValue = 0,
                DateTimeValue = DateTime.MinValue,
                ByteArrayValue = new Byte[] { },
                StringArrayValue = new String[] { },
            },
            new SettingsData    // Random
            {
                StringValue = "The quick brown fox jumps over the lazy dog",
                CharValue = 'R',
                BooleanValue = true,
                SByteValue = 107,
                ByteValue = 213,
                Int16Value = 3810,
                UInt16Value = 44287,
                Int32Value = -59883,
                UInt32Value = 732046,
                Int64Value = 842097,
                UInt64Value = 4407819,
                SingleValue = 9883778.123F,
                DoubleValue = 29310078.1234567,
                DecimalValue = 3889621099876.09981332m,
                DateTimeValue = new DateTime(1961, 10, 29, 14, 23, 40),
                ByteArrayValue = new Byte[]
                {
                    14,
                    189,
                    90,
                    27,
                    82
                },
                StringArrayValue = new String[]
                {
                    "\"abc\"",
                    "def",
                    "g,h,i",
                    "jk\tl",
                    "m n o"
                },
            },
            new SettingsData    // Random 2
            {
                StringValue = "All things come to those who wait.",
                CharValue = 'w',
                BooleanValue = false,
                SByteValue = 47,
                ByteValue = 18,
                Int16Value = 5992,
                UInt16Value = 47722,
                Int32Value = -340981,
                UInt32Value = 230977,
                Int64Value = 4067128,
                UInt64Value = 4984109978,
                SingleValue = 30728.25F,
                DoubleValue = 3498720091.43780,
                DecimalValue = 4532339011298.775902231m,
                DateTimeValue = DateTime.Now,
                ByteArrayValue = new Byte[]
                {
                    1,
                    8,
                    6,
                    14,
                    12
                },
                StringArrayValue = new String[]
                {
                    "One",
                    "Two",
                    "Three",
                    "Four",
                    "Five"
                },
            },
        };
    }
}
