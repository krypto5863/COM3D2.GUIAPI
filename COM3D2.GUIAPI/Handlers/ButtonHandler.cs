using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ButtonHandler
	{
		internal static EventHandler VanillaMenuOpened;
		private static ConfigManager GameConfigMan;

		private static List<ConfigTabButton> ListOfButtons = new List<ConfigTabButton>();

		private static UIGrid ButtonGrid;
		private static bool InitialSetup = false;

		internal static void StartButtonHandler()
		{
			VanillaMenuOpened += ProcessVanillaTabs;
			ConfigMenu.ButtonWasClicked += ProcessVanillaTabs;
		}
		private static void ProcessVanillaTabs(object sender, EventArgs args)
		{
			if (sender is ConfigManager == false)
			{
				GameConfigMan.systemWidget.alpha = 0f;
				GameConfigMan.soundWidget.alpha = 0f;
				GameConfigMan.cblConfigMgr.visible = false;

				foreach (ConfigManager.CategoryButton categoryButton in GameConfigMan.categoryTabButtons)
				{
					categoryButton.tab.SetSelect(false);
				}
			}
		}

		[HarmonyPatch(typeof(ConfigMgr), "OpenConfigPanel")]
		[HarmonyPostfix]
		private static void InstallButtons()
		{
			if (!ElementStoreHouse.WarehouseStocked)
			{
				ElementStoreHouse.StockWarehouse();
			}

			foreach (ConfigTabButton button in ListOfButtons)
			{
				if (!button.ButtonInstalled)
				{
					button.CreateButton();
				}
			}

			if (!InitialSetup)
			{
				var soundButton = GameObject.Find("SystemUI Root").GetComponentsInChildren<UISprite>(true).First(so => so && so.gameObject && so.name.Equals("SOUND"));
				var sysButton = GameObject.Find("SystemUI Root").GetComponentsInChildren<UISprite>(true).First(so => so && so.gameObject && so.name.Equals("SYSTEM"));

				var uiScrollView = soundButton.transform.parent.gameObject.AddComponent<UIScrollView>();

				var dragscrollview = soundButton.gameObject.AddComponent<UIDragScrollView>();
				var dragscrollview2 = sysButton.gameObject.AddComponent<UIDragScrollView>();

				dragscrollview.scrollView = uiScrollView;
				dragscrollview2.scrollView = uiScrollView;

				var panel = soundButton.transform.parent.gameObject.GetComponentsInChildren<UIPanel>(true)[0];
				//Not sure why depth changed once we added components and stuff but it did so we change it back.
				panel.depth += 50;
				panel.clipping = UIDrawCall.Clipping.SoftClip;
				panel.SetRect((panel.width * 3.5f) / 2.25f, 0, panel.width * 3.5f, panel.height);

				ButtonGrid = soundButton.transform.parent.gameObject.GetComponentsInChildren<UIGrid>(true)[0];
				ButtonGrid.hideInactive = true;
				ButtonGrid.sorting = UIGrid.Sorting.Custom;
				ButtonGrid.onCustomSort = CustomSort;
				ButtonGrid.Reposition();

				InitialSetup = true;
			}
		}
		private static int CustomSort(Transform t1, Transform t2)
		{
			string[] vanillaTabs = new string[] 
			{ 
				"SOUND",
				"SYSTEM",
				"CBL"
			};

			//returning -1 means t1 comes before. 1 means t2 comes before.
			if (vanillaTabs.Contains(t1.gameObject.name) && !vanillaTabs.Contains(t2.gameObject.name))
			{
				return -1;
			}
			else if (!vanillaTabs.Contains(t1.gameObject.name) && vanillaTabs.Contains(t2.gameObject.name))
			{
				return 1;
			}
			else if (vanillaTabs.Contains(t1.gameObject.name) && vanillaTabs.Contains(t2.gameObject.name))
			{
				return 0;
			}

			var configButton =  ListOfButtons.FirstOrDefault(but => but.Label.Equals(t1.name));
			var configButton2 = ListOfButtons.FirstOrDefault(but => but.Label.Equals(t2.name));

			if (configButton != null && !configButton.CategoryButton && (configButton2 != null && configButton2.CategoryButton))
			{
				return 1;
			} else if (configButton != null && configButton.CategoryButton && (configButton2 != null && !configButton2.CategoryButton)) 
			{
				return -1;
			}

			return t1.gameObject.name.CompareTo(t2.gameObject.name);
		}

		[HarmonyPatch(typeof(ConfigManager), "OnClickCategoryTabButton")]
		[HarmonyPrefix]
		private static void GetVanillaTabClicks(ConfigManager __instance)
		{
			VanillaMenuOpened.Invoke(__instance, null);
		}

		[HarmonyPatch(typeof(ConfigManager), "Init")]
		[HarmonyPrefix]
		private static void GetConfigManagerInstance(ConfigManager __instance)
		{
			GameConfigMan = __instance;
		}

		/// <summary>
		/// Create a basic button that runs a method when pressed. The button object is returned so you can manipulate some basic settings. If you just want to make a basic config menu, don't use this. Buttons are handled for you by CreateConfigMenu.
		/// </summary>
		/// <param name="label">The text that will appear on your button.</param>
		/// <param name="eventDelegate">The method that will be called on a button press</param>
		///  <param name="categoryButton">If this button will be exclusively used to open a menu in the config menu. Typically if you're calling this function directly, you're gonna want the default.</param>
		public static ConfigTabButton CreateConfigTabButton(string label, EventDelegate eventDelegate, bool categoryButton = false)
		{
			var newButton = new ConfigTabButton(label, eventDelegate, categoryButton);

			ListOfButtons.Add(newButton);

			if (InitialSetup) 
			{
				InstallButtons();
			}

			return newButton;
		}
		/// <summary>
		/// Should null and remove a button from the menu.
		/// </summary>
		/// <param name="button">The button to be removed...</param>
		public static void RemoveConfigTabButton(ConfigTabButton button)
		{
			Main.BepLogger.LogDebug($"Call for button of name {button.Label}");

			if (ListOfButtons.Contains(button))
			{
				ListOfButtons.Remove(button);
			}

			button.RemoveButton();

			ButtonGrid.Reposition();

			foreach (ConfigTabButton tabButton in ListOfButtons)
			{
				tabButton.Button.SetActive(false);
				tabButton.Button.SetActive(true);
			}
		}
	}
}