// Copyright (c) 2019-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// Attribute specifies that this property does not represent a setting and
    /// should not be saved.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ExcludedSettingAttribute : Attribute
    {
    }
}
