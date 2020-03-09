# WinSettings

[![NuGet version (SoftCircuits.WinSettings)](https://img.shields.io/nuget/v/SoftCircuits.WinSettings.svg?style=flat-square)](https://www.nuget.org/packages/SoftCircuits.WinSettings/)

```
Install-Package SoftCircuits.WinSettings
```

## Overview

WinSettings is a .NET class library that makes it easy to save and retrieve application settings on Windows. It includes three settings classes: `IniSettings`, which stores the settings to an INI file; `XmlSettings`, which stores the settings to an XML file, and `RegistrySettings`, which stores the settings to the Windows registry. In addition, it makes it easy to define your own settings type.

Settings can be encrypted just by adding a property attribute. There is also an attribute to exclude a particular property when the property is used internally and does not represent an application setting.

To use a settings class, simply derive your own settings class from one of the ones described above and add public properties that you want to be saved. Your class' constructor should set any default values. Then call the `Save()` and `Load()` methods to save the settings in your class.

## IniSettings Class

The <see cref="IniSettings"/> class makes it very easy to save your application settings to an INI file.

To use the class, simply derive your own settings class from `IniSettings` and add the public properties that you want to be saved as settings. You can then call the `Load()` and `Save()` methods to read or write those settings to an INI file.

Your derived class' constructor should initialize your settings properties to their default values.

Two attributes are available for public properties in your derived class. The first is `EncryptedSettingAttribute`. Use this attribute if you want the setting to be encrypted when saved to file. When using this attribute on any property, you must provide a valid encryption password to the `IniSettings` constructor.

The second is the `ExcludedSettingAttribute`. Use this attribute on any properties that are used internally by your code and should not saved to file.

All properties without the `ExcludedSettingAttribute` attribute must be of one of the supported data types. This includes all the basic data types as well as `string[]` and `byte[]`. All other types will raise an exception. In addition, INI files do not support strings that contain newlines unless those strings are encrypted.

#### Example

The following example creates a settings class called `MySettings` with several properties, two of which are encrypted when saved to file.

```cs
public class MySettings : IniSettings
{
    // Define properties to be saved to file
    public string EmailHost { get; set; }
    public int EmailPort { get; set; }

    // The following properties will be encrypted
    [EncryptedSetting]
    public string UserName { get; set; }
    [EncryptedSetting]
    public string Password { get; set; }

    // The following property will not be saved to file
    // Non-public properties are also not saved to file
    [ExcludedSetting]
    public DateTime Created { get; set; }

    public MySettings(string filename)
        : base(filename, "Password123")
    {
        // Set initial, default property values
        EmailHost = string.Empty;
        EmailPort = 0;
        UserName = string.Empty;
        Password = string.Empty;

        Created = DateTime.Now;
    }
}
```

## XmlSettings Class

The <see `XmlSettings` class makes it very easy to save your application settings to an XML file.

To use the class, simply derive your own settings class from `XmlSettings` and add the public properties that you want to be saved as settings. You can then call the `Load()` and `Save()` methods to read or write those settings to an XML file.

Your derived class' constructor should initialize your settings properties to their default values.

Two attributes are available for public properties in your derived class. The first is `EncryptedSettingAttribute`. Use this attribute if you want the setting to be encrypted when saved to file. When using this attribute on any property, you must provide a valid encryption password to the `XmlSettings` constructor.

The second is the `ExcludedSettingAttribute` Use this attribute on any properties that are used internally by your code and should not saved to file.

All properties without the `ExcludedSettingAttribute` attribute must be of one of the supported data types. This includes all the basic data types `string[]` and `byte[]`. All other types will raise an exception.

#### Example

The following example creates a settings class called `MySettings` with several properties, two of which are encrypted when saved to file.

```cs
public class MySettings : XmlSettings
{
    // Define properties to be saved to file
    public string EmailHost { get; set; }
    public int EmailPort { get; set; }

    // The following properties will be encrypted
    [EncryptedSetting]
    public string UserName { get; set; }
    [EncryptedSetting]
    public string Password { get; set; }

    // The following property will not be saved to file
    // Non-public properties are also not saved to file
    [ExcludedSetting]
    public DateTime Created { get; set; }

    public MySettings(string filename)
        : base(filename, "Password123")
    {
        // Set initial, default property values
        EmailHost = string.Empty;
        EmailPort = 0;
        UserName = string.Empty;
        Password = string.Empty;

        Created = DateTime.Now;
    }
}
```

## RegistrySettings Class

The `RegistrySettings` class makes it very easy to save your application settings to the system registry.

To use the class, simply derive your own settings class from `RegistrySettings` and add the public properties that you want to be saved as settings. You can then call the `Settings.Load()` and `Settings.Save()` methods to read or write those settings to the system registry.

Your derived class' constructor should initialize your settings properties to their default values.

Two attributes are available for public properties in your derived class. The first is `EncryptedSettingAttribute`. Use this attribute if you want the setting to be encrypted when saved to file. When using this attribute on any property, you must provide a valid `Encryption` object to the `RegistrySettings` constructor.

The second is the `ExcludedSettingAttribute`. Use this attribute on any properties that are used internally by your code and should not saved to the registry.

Note that only properties with data types supported by the `Encryption class are supported by `RegistrySettings`. This includes all the basic data types as well as `string[]` and `byte[]`. All other types will raise an exception.

#### Example

The following example creates a settings class called `MySettings` with several properties, two of which are encrypted when saved to the registry.

```cs
public class MySettings : RegistrySettings
{
    // Define properties to be saved to the registry
    public string EmailHost { get; set; }
    public int EmailPort { get; set; }

    // The following properties will be encrypted
    [EncryptedSetting]
    public string UserName { get; set; }
    [EncryptedSetting]
    public string Password { get; set; }

    // The following property will not be saved to the registry
    // Non-public properties are also not saved to the registry
    [ExcludedSetting]
    public DateTime Created { get; set; }

    public MySettings(string companyName, string applicationName, RegistrySettingsType settingsType)
        : base(companyName, applicationName, settingsType, new Encryption("Password", EncryptionAlgorithm.Aes))
    {
        // Set initial, default property values
        EmailHost = string.Empty;
        EmailPort = 0;
        UserName = string.Empty;
        Password = string.Empty;

        Created = DateTime.Now;
    }
}
```

## Settings Class

The `Settings` class is the base class for the `IniSettings`, `XmlSettings` and `RegistrySettings` classes. You don't need this class but you could use it to create your own type of custom settings class.

To do this, create your own `static`, `abstract` class that derives from `Settings` and override the virtual `OnSaveSettings()` and `OnLoadSettings()` methods.

As the name suggests, `OnSaveSettings()` is called when the settings are being saved. This method is passed a collection of `Setting` objects. Your handler needs to write these settings to your custom data store. The `Setting.Name` property contains the setting name. Use the `Setting.GetValue()` method to get the value. Or use the `Setting.GetValueAsString()` instead if your data store only supports string values.

The steps to override `OnLoadSettings()` is similar. This method is also passed a collection of `Setting` objects. Your handler needs to read each named setting from your custom data store. You can then set that value using the `Setting.SetValue()` or `Setting.SetValueFromString()` methods.

## Dependencies

This project requires the NuGet packages [SoftCircuits.EasyEncryption](https://www.nuget.org/packages/SoftCircuits.EasyEncryption/) and Microsoft.Win32.Registry.
