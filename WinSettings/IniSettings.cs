// Copyright (c) 2019 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using SoftCircuits.EasyEncryption;
using System;
using System.Collections.Generic;
using System.IO;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// The <see cref="IniSettings"/> class makes it very easy to save your application
    /// settings to an INI file.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To use the class, simply derive your own settings class from
    /// <see cref="IniSettings" /> and add the public properties that you want to be
    /// saved as settings. You can then call the <see cref="Settings.Load"/> and
    /// <see cref="Settings.Save"/> methods to read or write those settings to an INI
    /// file.
    /// </para>
    /// <para>
    /// Your derived class' constructor should initialize your settings properties to
    /// their default values.
    /// </para>
    /// <para>
    /// Two attributes are available for public properties in your derived class. The
    /// first is <see cref="EncryptedSettingAttribute" />. Use this attribute if you
    /// want the setting to be encrypted when saved to file. When using this attribute on
    /// any property, you must provide a valid <see cref="Encryption"/> object to the
    /// <see cref="IniSettings" /> constructor.
    /// </para>
    /// <para>
    /// The second is the <see cref="ExcludedSettingAttribute"/>. Use this attribute
    /// on any properties that are used internally by your code and should not saved to
    /// file.
    /// </para>
    /// <para>
    /// Note that only properties with data types supported by the
    /// <see cref="Encryption"/> class are supported by <see cref="IniSettings"/>.
    /// This includes all the basic data types as well as <c>string[]</c> and
    /// <c>byte[]</c>. All other types will raise an exception. In addition, INI files
    /// do not support strings that contain newlines unless those strings are encrypted.
    /// </para>
    /// </remarks>
    /// <example>
    /// The following example creates a settings class called <c>MySettings</c> with
    /// several properties, two of which are encrypted when saved to file.
    /// <code>
    /// public class MySettings : IniSettings
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
    ///     public MySettings(string filename)
    ///         : base(filename, new Encryption("Password", EncryptionAlgorithm.Aes))
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
    /// <seealso cref="RegistrySettings"/>
    /// <seealso cref="XmlSettings"/>
    public abstract class IniSettings : Settings
    {
        /// <summary>
        /// Gets or sets the name of the INI settings file.
        /// </summary>
        [ExcludedSetting]
        public string FileName { get; set; }

        /// <summary>
        /// Constructs an instance of the <c>XmlSettings</c> class.
        /// </summary>
        /// <param name="filename">Name of the settings file.</param>
        /// <param name="encryption"><see cref="Encryption" /> instance used for encrypted settings. May be <c>null</c>
        /// if no settings use the <see cref="EncryptedSettingAttribute" /> attribute.</param>
        public IniSettings(string filename, Encryption encryption)
            : base(encryption)
        {
            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("A valid path and file name is required.", nameof(filename));
            FileName = filename;
        }

        /// <summary>
        /// Performs internal load operations.
        /// </summary>
        /// <param name="settings">Settings to be loaded.</param>
        protected override void OnLoadSettings(IEnumerable<Setting> settings)
        {
            if (File.Exists(FileName))
            {
                // Load INI file
                IniFile iniFile = new IniFile();
                iniFile.Load(FileName);
                // Read settings
                foreach (Setting setting in settings)
                {
                    string value = iniFile.GetSetting(IniFile.DefaultSectionName, setting.Name, null);
                    if (value != null)
                        setting.SetValueFromString(value);
                }
            }
        }

        /// <summary>
        /// Performs internal save operations.
        /// </summary>
        /// <param name="settings">Settings to be saved.</param>
        protected override void OnSaveSettings(IEnumerable<Setting> settings)
        {
            // Create INI file
            IniFile iniFile = new IniFile();
            // Write settings
            foreach (Setting setting in settings)
            {
                string value = setting.GetValueAsString();
                if (value != null)
                    iniFile.SetSetting(IniFile.DefaultSectionName, setting.Name, value);
            }
            iniFile.Save(FileName);
        }
    }
}
