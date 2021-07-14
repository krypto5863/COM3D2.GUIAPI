using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

//These two lines tell your plugin to not give a flying fuck about accessing private variables/classes whatever. It requires a publicized stubb of the library with those private objects though.
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace COM3D2.GUIAPI
{
	//This is the metadata set for your plugin.
	[BepInPlugin("COM3D2.GUIAPI", "COM3D2.GUIAPI", "1.0")]
	public class Main : BaseUnityPlugin
	{
		//static saving of the main instance. This makes it easier to run stuff like coroutines from static methods or accessing non-static vars.
		internal static Main This;

		//Static var for the logger so you can log from other classes.
		internal static ManualLogSource BepLogger;

		internal static EventHandler MouseWheelScroll;

		private void Awake()
		{
			//Useful for engaging coroutines or accessing variables non-static variables. Completely optional though.
			This = this;

			//pushes the logger to a public static var so you can use the bepinex logger from other classes.
			BepLogger = base.Logger;

			//Installs the patches in the Main class.
			var harmony = Harmony.CreateAndPatchAll(typeof(Main));

			ButtonHandler.StartButtonHandler();
			harmony.PatchAll(typeof(ButtonHandler));
			harmony.PatchAll(typeof(MenuHandler));
		}
		private void Update() 
		{
			try
			{
				var scroll = Input.GetAxis("Mouse ScrollWheel");

				if (scroll > 0 || scroll < 0) {
					MouseWheelScroll.Invoke(scroll, null);
				}
			}
			catch
			{ 
			
			}
		}
	}
}
