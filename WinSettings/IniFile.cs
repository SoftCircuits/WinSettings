// Copyright (c) 2019-2022 Jonathan Wood (www.softcircuits.com)
// Licensed under the MIT license.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SoftCircuits.WinSettings
{
    /// <summary>
    /// Represents one name/value pair in an INI file.
    /// </summary>
    internal class IniSetting
    {
        /// <summary>
        /// The name of this INI setting.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// The value of this INI setting.
        /// </summary>
        public string? Value { get; set; }
    }

    /// <summary>
    /// Class to create and read INI files.
    /// </summary>
    internal class IniFile
    {
        /// <summary>
        /// Section used for settings not under any section header (within [])
        /// </summary>
        public const string DefaultSectionName = "General";

        /// <summary>
        /// Represents an entire INI file section.
        /// </summary>
        private class IniSection
        {
            public string Name { get; set; }
            public Dictionary<string, IniSetting> Settings { get; private set; }

            public IniSection()
            {
                Name = string.Empty;
                Settings = new Dictionary<string, IniSetting>(StringComparer.OrdinalIgnoreCase);
            }
        }

        private readonly Dictionary<string, IniSection> Sections = new(StringComparer.OrdinalIgnoreCase);

        #region File functions

        /// <summary>
        /// Loads an INI settings file.
        /// </summary>
        /// <param name="filename">Path of file to load.</param>
        public void Load(string filename)
        {
            Sections.Clear();

            // Default section
            IniSection? section = new() { Name = DefaultSectionName };
            Sections.Add(section.Name, section);

            string? line;
            using StreamReader file = new(filename);

            while ((line = file.ReadLine()) != null)
            {
                line = line.TrimStart();
                if (line.Length > 0)
                {
                    if (line[0] == ';')
                    {
                        // Ignore comments
                    }
                    else if (line[0] == '[')
                    {
                        // Parse section header
                        int pos = line.IndexOf(']', 1);
                        if (pos == -1)
                            pos = line.Length;
#if NETSTANDARD
                        string name = line.Substring(1, pos - 1).Trim();
#else
                        string name = line[1..pos].Trim();
#endif
                        if (name.Length > 0)
                        {
                            if (!Sections.TryGetValue(name, out section))
                            {
                                section = new IniSection { Name = name };
                                Sections.Add(section.Name, section);
                            }
                        }
                    }
                    else
                    {
                        // Parse setting name and value
                        string name, value;

                        int pos = line.IndexOf('=');
                        if (pos == -1)
                        {
                            name = line.Trim();
                            value = string.Empty;
                        }
                        else
                        {
#if NETSTANDARD
                            name = line.Substring(0, pos).Trim();
                            value = line.Substring(pos + 1);
#else
                            name = line[..pos].Trim();
                            value = line[(pos + 1)..];
#endif
                        }

                        if (name.Length > 0)
                        {
                            if (section.Settings.TryGetValue(name, out IniSetting? setting))
                            {
                                setting.Value = value;
                            }
                            else
                            {
                                setting = new IniSetting { Name = name, Value = value };
                                section.Settings.Add(name, setting);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Writes the current settings to an INI file. If the file already exists, it is overwritten.
        /// </summary>
        /// <param name="filename">Path of file to write to.</param>
        public void Save(string filename)
        {
            using StreamWriter file = new(filename, false);

            bool firstLine = true;
            foreach (IniSection section in Sections.Values)
            {
                if (firstLine)
                    firstLine = false;
                else
                    file.WriteLine();

                if (section.Settings.Any())
                {
                    file.WriteLine("[{0}]", section.Name);
                    foreach (IniSetting setting in section.Settings.Values)
                        file.WriteLine("{0}={1}", setting.Name, setting.Value);
                }
            }
        }

        #endregion

        #region Read values

        /// <summary>
        /// Returns the value of an INI setting.
        /// </summary>
        /// <param name="section">The INI file section.</param>
        /// <param name="setting">The INI setting name.</param>
        /// <param name="defaultValue">The value to return if the setting was not found.</param>
        /// <returns>Returns the specified setting value.</returns>
        public string? GetSetting(string section, string setting, string? defaultValue = null)
        {
            if (Sections.TryGetValue(section, out IniSection? iniSection))
            {
                if (iniSection.Settings.TryGetValue(setting, out IniSetting? iniSetting))
                    return iniSetting.Value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns the value of an INI setting as an integer value.
        /// </summary>
        /// <param name="section">The INI file section.</param>
        /// <param name="setting">The INI setting name.</param>
        /// <param name="defaultValue">The value to return if the setting was not found,
        /// or if it could not be converted to a integer value.</param>
        /// <returns>Returns the specified setting value as an integer value.</returns>
        public int GetSetting(string section, string setting, int defaultValue)
        {
            if (int.TryParse(GetSetting(section, setting), out int value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// Returns the value of an INI setting as a double value.
        /// </summary>
        /// <param name="section">The INI file section.</param>
        /// <param name="setting">The INI setting name.</param>
        /// <param name="defaultValue">The value to return if the setting was not found,
        /// or if it could not be converted to a double value.</param>
        /// <returns>Returns the specified setting value as a double value.</returns>
        public double GetSetting(string section, string setting, double defaultValue)
        {
            if (double.TryParse(GetSetting(section, setting), out double value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// Returns the value of an INI setting as a Boolean value.
        /// </summary>
        /// <param name="section">The INI file section.</param>
        /// <param name="setting">The INI setting name.</param>
        /// <param name="defaultValue">The value to return if the setting was not found,
        /// or if it could not be converted to a Boolean value.</param>
        /// <returns>Returns the specified setting value as a Boolean.</returns>
        public bool GetSetting(string section, string setting, bool defaultValue)
        {
            if (ConvertToBool(GetSetting(section, setting), out bool value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// Returns all settings in the given INI section.
        /// </summary>
        /// <param name="section">The section that contains the settings to be retrieved.</param>
        /// <returns>Returns the settings in the given INI section.</returns>
        public IEnumerable<IniSetting> GetSectionSettings(string section)
        {
            if (Sections.TryGetValue(section, out IniSection? iniSection))
            {
                foreach (var setting in iniSection.Settings)
                    yield return setting.Value;
            }
        }

        #endregion

        #region Write values

        /// <summary>
        /// Sets an INI file setting. The setting is not written to disk until
        /// <see cref="Save"/> is called.
        /// </summary>
        /// <param name="section">The INI-file section.</param>
        /// <param name="setting">The name of the INI-file setting.</param>
        /// <param name="value">The value of the INI-file setting</param>
        public void SetSetting(string section, string setting, string value)
        {
            if (!Sections.TryGetValue(section, out IniSection? iniSection))
            {
                iniSection = new IniSection { Name = section };
                Sections.Add(iniSection.Name, iniSection);
            }
            if (!iniSection.Settings.TryGetValue(setting, out IniSetting? iniSetting))
            {
                iniSetting = new IniSetting { Name = setting };
                iniSection.Settings.Add(iniSetting.Name, iniSetting);
            }
            iniSetting.Value = value;
        }

        /// <summary>
        /// Sets an INI file setting with an integer value.
        /// </summary>
        /// <param name="section">The INI-file section.</param>
        /// <param name="setting">The name of the INI-file setting.</param>
        /// <param name="value">The value of the INI-file setting</param>
        public void SetSetting(string section, string setting, int value)
        {
            SetSetting(section, setting, value.ToString());
        }

        /// <summary>
        /// Sets an INI file setting with a double value.
        /// </summary>
        /// <param name="section">The INI-file section.</param>
        /// <param name="setting">The name of the INI-file setting.</param>
        /// <param name="value">The value of the INI-file setting</param>
        public void SetSetting(string section, string setting, double value)
        {
            SetSetting(section, setting, value.ToString());
        }

        /// <summary>
        /// Sets an INI file setting with a Boolean value.
        /// </summary>
        /// <param name="section">The INI-file section.</param>
        /// <param name="setting">The name of the INI-file setting.</param>
        /// <param name="value">The value of the INI-file setting</param>
        public void SetSetting(string section, string setting, bool value)
        {
            SetSetting(section, setting, value.ToString());
        }

        #endregion

        #region Boolean parsing

        private readonly string[] TrueStrings = { "true", "yes", "on" };
        private readonly string[] FalseStrings = { "false", "no", "off" };

        private bool ConvertToBool(string? s, out bool value)
        {
            if (s == null)
                value = false;
            if (TrueStrings.Any(s2 => string.Compare(s, s2, true) == 0))
                value = true;
            else if (FalseStrings.Any(s2 => string.Compare(s, s2, true) == 0))
                value = false;
            else if (int.TryParse(s, out int i))
                value = (i != 0);
            else
            {
                value = false;
                return false;
            }
            return true;
        }

        #endregion

        //public void Dump()
        //{
        //    foreach (IniSection section in Sections.Values)
        //    {
        //        Debug.WriteLine(string.Format("[{0}]", section.Name));
        //        foreach (IniSetting setting in section.Settings.Values)
        //            Debug.WriteLine("[{0}]=[{1}]", setting.Name, setting.Value);
        //    }
        //}

    }
}
