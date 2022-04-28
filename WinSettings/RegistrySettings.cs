// Copyright (c) 2019-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using Microsoft.Win32;
using System.Collections.Generic;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// Specifies the location within the registry where <see cref="RegistrySettings"/> stores
    /// settings.
    /// </summary>
    public enum RegistrySettingsType
    {
        /// <summary>
        /// Stores settings in the Windows registry base key HKEY_CURRENT_USER, normally
        /// used for storing information about the current user preferences.
        /// </summary>
        CurrentUser,

        /// <summary>
        /// Stores settings in thee Windows registry base key HKEY_LOCAL_MACHINE, normally
        /// use for storing configuration data for the local machine.
        /// </summary>
        LocalMachine
    }

    /// <summary>
    /// The <see cref="RegistrySettings"/> class makes it very easy to save your application
    /// settings to the system registry.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To use the class, simply derive your own settings class from
    /// <see cref="RegistrySettings" /> and add the public properties that you want to be
    /// saved as settings. You can then call the <see cref="Settings.Load"/> and
    /// <see cref="Settings.Save"/> methods to read or write those settings to the
    /// system registry.
    /// </para>
    /// <para>
    /// Your derived class' constructor should initialize your settings properties to
    /// their default values.
    /// </para>
    /// <para>
    /// Two attributes are available for public properties in your derived class. The
    /// first is <see cref="EncryptedSettingAttribute" />. Use this attribute if you
    /// want the setting to be encrypted when saved to file. When using this attribute on
    /// any property, you must provide a valid encryption password to the
    /// <see cref="RegistrySettings" /> constructor.
    /// </para>
    /// <para>
    /// The second is the <see cref="ExcludedSettingAttribute"/>. Use this attribute
    /// on any properties that are used internally by your code and should not saved to
    /// the registry.
    /// </para>
    /// <para>
    /// All public properties without the <see cref="ExcludedSettingAttribute"></see>
    /// attribute must be of one of the supported data types. This includes all the basic
    /// data types as well as <see cref="string[]"></see> and <see cref="byte[]"></see>.
    /// All other types will raise an exception.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example creates a settings class called <c>MySettings</c> with
    /// several properties, two of which are encrypted when saved to file.
    /// <code>
    /// public class MySettings : RegistrySettings
    /// {
    ///     // Define properties to be saved to file
    ///     public string EmailHost { get; set; }
    ///     public int EmailPort { get; set; }
    /// 
    ///     // The following properties will be encrypted
    ///     [EncryptedSetting]
    ///     public string UserName { get; set; }
    ///     [EncryptedSetting]
    ///     public string Password { get; set; }
    /// 
    ///     // The following property will not be saved to file
    ///     // Non-public properties are also not saved to file
    ///     [ExcludedSetting]
    ///     public DateTime Created { get; set; }
    /// 
    ///     public MySettings(string companyName, string applicationName, RegistrySettingsType settingsType)
    ///         : base(companyName, applicationName, settingsType, "Password123")
    ///     {
    ///         // Set initial, default property values
    ///         EmailHost = string.Empty;
    ///         EmailPort = 0;
    ///         UserName = string.Empty;
    ///         Password = string.Empty;
    /// 
    ///         Created = DateTime.Now;
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="IniSettings"/>
    /// <seealso cref="XmlSettings"/>
    public abstract class RegistrySettings : Settings
    {
        private readonly string SubKeyPath;
        private readonly RegistryKey RegistryKey;

        /// <summary>
        /// Constructs a new <c>RegistrySettings</c> instance.
        /// </summary>
        /// <param name="companyName">Company name entry in registry.</param>
        /// <param name="applicationName">Application name entry in registration.</param>
        /// <param name="settingsType">Section to store entries in registry.</param>
        /// <param name="password">Encryption password. May be <c>null</c> if no settings
        /// use the <see cref="EncryptedSettingAttribute" /> attribute.</param>
        public RegistrySettings(string companyName, string applicationName, RegistrySettingsType settingsType, string? password = null)
            : base(password)
        {
            SubKeyPath = string.Format("Software\\{0}\\{1}", companyName, applicationName);
            RegistryKey = (settingsType == RegistrySettingsType.CurrentUser) ? Registry.CurrentUser : Registry.LocalMachine;
        }

        /// <summary>
        /// Performs internal save operations.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        public override void OnSaveSettings(IEnumerable<Setting> settings)
        {
            using RegistryKey registryKey = RegistryKey.CreateSubKey(SubKeyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            foreach (var setting in settings)
            {
                var value = setting.GetValue();
                if (value != null)
                    registryKey.SetValue(setting.Name, value);
            }
        }

        /// <summary>
        /// Performs internal load operations.
        /// </summary>
        /// <param name="settings">Settings to be loaded.</param>
        public override void OnLoadSettings(IEnumerable<Setting> settings)
        {
            using RegistryKey? registryKey = RegistryKey.OpenSubKey(SubKeyPath);
            if (registryKey != null)
            {
                foreach (var setting in settings)
                {
                    object? value = registryKey.GetValue(setting.Name);
                    if (value != null)
                        setting.SetValue(value);
                }
            }
        }
    }
}
