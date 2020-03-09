// Copyright (c) 2019-2020 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// Represents a single setting for <see cref="Settings"></see>-derived classes.
    /// </summary>
    public class Setting
    {
        private readonly Settings Settings;
        private readonly PropertyInfo PropertyInfo;
        private readonly bool Encrypted;

        /// <summary>
        /// Constructs a new instance of the <see cref="Setting"></see> class.
        /// </summary>
        /// <param name="settings">The <see cref="Settings"/> class that contains
        /// this property (setting).</param>
        /// <param name="propertyInfo">The <see cref="PropertyInfo"></see> for this
        /// property.</param>
        /// <param name="encrypted">Indicates whether or not this setting is
        /// encrypted.</param>
        public Setting(Settings settings, PropertyInfo propertyInfo, bool encrypted)
        {
            Settings = settings;
            PropertyInfo = propertyInfo;
            Encrypted = encrypted;
        }

        /// <summary>
        /// Gets the name of this setting.
        /// </summary>
        public string Name => PropertyInfo.Name;

        /// <summary>
        /// Gets the type of this setting.
        /// </summary>
        public Type Type => Encrypted ? typeof(string) : PropertyInfo.PropertyType;

        /// <summary>
        /// Gets the value of this setting.
        /// </summary>
        /// <returns>Returns the value of this setting.</returns>
        public object GetValue()
        {
            object value = PropertyInfo.GetValue(Settings);
            if (value != null && Encrypted)
                return Settings.Encryption.Encrypt(value);
            return value;
        }

        /// <summary>
        /// Sets the value of this setting.
        /// </summary>
        /// <param name="value">The value this setting should be set to.</param>
        public void SetValue(object value)
        {
            // Leave property value unmodified if no value or error
            if (value != null)
            {
                try
                {
                    if (Encrypted)
                    {
                        // Ecrypted values stored as string
                        if (value is string)
                            PropertyInfo.SetValue(Settings, Settings.Encryption.Decrypt(value as string, PropertyInfo.PropertyType));
                    }
                    else PropertyInfo.SetValue(Settings, Convert.ChangeType(value, Type));
                }
                catch (Exception) { Debug.Assert(false); }
            }
        }

        /// <summary>
        /// Gets this setting's value as a string.
        /// </summary>
        /// <returns>Returns this setting's value as a string.</returns>
        public string GetValueAsString()
        {
            object value = GetValue();
            if (value == null)
                return string.Empty;
            else if (value is byte[])
                return ArrayToString.Encode((value as byte[]).Select(b => b.ToString()).ToArray());
            else if (value is string[])
                return ArrayToString.Encode(value as string[]);
            else
                return value.ToString();
        }

        /// <summary>
        /// Sets this setting's value from a string.
        /// </summary>
        /// <param name="value">A string that represents the value this setting should be set to.</param>
        public void SetValueFromString(string value)
        {
            if (value != null)
            {
                try
                {
                    if (Type == typeof(byte[]))
                        SetValue(ArrayToString.Decode(value).Select(s => byte.Parse(s)).ToArray());
                    else if (Type == typeof(string[]))
                        SetValue(ArrayToString.Decode(value));
                    else
                        SetValue(value);
                }
                catch (Exception) { Debug.Assert(false); }
            }
        }
    }
}
