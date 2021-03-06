﻿// Copyright (c) 2019-2021 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// This attribute specifies that this property should be encrypted
    /// when it is saved.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptedSettingAttribute : Attribute
    {
    }
}
