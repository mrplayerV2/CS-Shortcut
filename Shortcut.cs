using IWshRuntimeLibrary;
using Microsoft.Win32;
using System;
using System.IO;

internal class Shortcut
{
	public static void CreateShortcut(string shortcutPath, string targetPath)
	{
		IWshShortcut obj = (IWshShortcut)new WshShellClass().CreateShortcut(shortcutPath);
		obj.TargetPath = targetPath;
		obj.Save();
	}

	public static void AddShortcutToStartup()
	{
		RemoveShortcutFromStartup(App.ApplicationName);
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
		folderPath = Path.Combine(folderPath, App.PublisherName);
		folderPath = Path.Combine(folderPath, App.ApplicationName + ".appref-ms");
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", writable: true);
		if (registryKey.GetValue(App.ApplicationName) != null)
		{
			registryKey.DeleteValue(App.ApplicationName, throwOnMissingValue: false);
		}
		registryKey.SetValue(App.ApplicationName, folderPath);
		registryKey.Close();
	}

	public static void RemoveShortcutFromStartup(string productName)
	{
		string[] files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
		foreach (string text in files)
		{
			if (text.IndexOf(productName) > 0)
			{
				System.IO.File.Delete(text);
			}
		}
		files = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup));
		foreach (string text2 in files)
		{
			if (text2.IndexOf(productName) > 0)
			{
				System.IO.File.Delete(text2);
			}
		}
	}
}