using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using ConfigurationManager;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using COM3D2.GUIAPI;

//These two lines tell your plugin to not give a flying fuck about accessing private variables/classes whatever. It requires a publicized stubb of the library with those private objects though.
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace ConfigManButton
{
	//This is the metadata set for your plugin.
	[BepInPlugin("ConfigManButton", "ConfigManButton", "1.0")]
	[BepInDependency("com.bepis.bepinex.configurationmanager", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("COM3D2.GUIAPI", BepInDependency.DependencyFlags.HardDependency)]
	public class Main : BaseUnityPlugin
	{
		//static saving of the main instance. This makes it easier to run stuff like coroutines from static methods or accessing non-static vars.
		public static Main @this;

		//Static var for the logger so you can log from other classes.
		public static ManualLogSource logger;

		//Config entry variable. You set your configs to this.
		internal static ConfigEntry<bool> DisableManagerHotkey;
		private ConfigurationManager.ConfigurationManager ConfigMan;
		private static COM3D2.GUIAPI.ConfigTabButton Button;

		private void Awake()
		{
			//Useful for engaging coroutines or accessing variables non-static variables. Completely optional though.
			@this = this;

			//pushes the logger to a public static var so you can use the bepinex logger from other classes.
			logger = Logger;

			//Binds the configuration. In other words it sets your ConfigEntry var to your config setup.
			DisableManagerHotkey = Config.Bind("General", "Use ConfigManager Hotkey", false, "Will turn off the F1 key from activating config manager.");

			DisableManagerHotkey.SettingChanged += (s,e) => 
			{
				ConfigMan.OverrideHotkey = !DisableManagerHotkey.Value;
			};
			

			ConfigMan = GetComponent<ConfigurationManager.ConfigurationManager>();

			Button = COM3D2.GUIAPI.ButtonHandler.CreateConfigTabButton("PLUGINS", new EventDelegate(() =>
			{
				ConfigMan.DisplayingWindow = !ConfigMan.DisplayingWindow;
				ConfigMan.OverrideHotkey = !DisableManagerHotkey.Value;
			}));
		}
	}
}
