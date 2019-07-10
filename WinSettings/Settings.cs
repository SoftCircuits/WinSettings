// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.EasyEncryption;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// Provides an abstract base class for specialized settings classes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Specialized classes should inherit from this class and implement specific storage and
    /// retrieval logic for each settings by overriding the
    /// <see cref="OnSaveSettings(IEnumerable{Setting})"/> and
    /// <see cref="OnLoadSettings(IEnumerable{Setting})"/> methods.
    /// </para>
    /// <para>
    /// Ultimately, the specialized classes will then be overridden by each application's
    /// settings class. The public properties in that class will become the settings that
    /// are saved by classes that derive from this class.
    /// </para>
    /// </remarks>
    public abstract class Settings
    {
        private IEnumerable<Setting> SettingsList { get; }

        /// <summary>
        /// Abstract method called when the settings should be saved. Allows
        /// the derived class to save those settings in a specialized way.
        /// </summary>
        /// <param name="settings">The list of settings to be saved.</param>
        protected abstract void OnSaveSettings(IEnumerable<Setting> settings);

        /// <summary>
        /// Abstract method called when the settings sould be loaded. Allows
        /// the derived class to load those settings in a specialized way.
        /// </summary>
        /// <param name="settings">The list of settings to be loaded.</param>
        protected abstract void OnLoadSettings(IEnumerable<Setting> settings);

        /// <summary>
        /// Gets the <c>Encryption</c> instance associated with this <c>Settings</c>
        /// instance.
        /// </summary>
        [ExcludedSetting]
        public Encryption Encryption { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// An exception is thrown if <paramref name="encryption"/> is null but one or more properties have the
        /// EncryptedSetting attribute.
        /// </remarks>
        /// <param name="encryption">May be null if no properties have the
        /// <code>EncryptedSetting</code> attribute.</param>
        public Settings(Encryption encryption)
        {
            SettingsList = BuildSettingsList();
            Encryption = encryption;
        }

        /// <summary>
        /// Saves all settings.
        /// </summary>
        public void Save()
        {
            OnSaveSettings(SettingsList);
        }

        /// <summary>
        /// Loads all settings.
        /// </summary>
        public void Load()
        {
            OnLoadSettings(SettingsList);
        }

        private IEnumerable<Setting> BuildSettingsList()
        {
            // Iterate through all public instance properties
            foreach (PropertyInfo prop in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                // Ignore properties with ExcludedSetting attribute
                if (!Attribute.IsDefined(prop, typeof(ExcludedSettingAttribute)))
                {
                    // Test for supported data type (same types as for Encryption class)
                    if (!Encryption.IsTypeSupported(prop.PropertyType))
                        throw new Exception(string.Format("Settings property '{0}' is an unsupported data type '{1}'. Change property type or use ExcludedSetting attribute.",
                            prop.Name, prop.PropertyType.ToString()));
                    bool encrypted = Attribute.IsDefined(prop, typeof(EncryptedSettingAttribute));
                    if (encrypted && Encryption == null)
                        throw new InvalidOperationException("Encryption cannot be null if one or more settings have the EncryptedSetting attribute.");
                    yield return new Setting(this, prop, encrypted);
                }
            }
        }
    }
}
