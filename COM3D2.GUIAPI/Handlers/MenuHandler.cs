using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.GUIAPI
{
	public class MenuHandler
	{
		private static List<ConfigMenu> ListOfMenus = new List<ConfigMenu>();

		[HarmonyPatch(typeof(ConfigMgr), "OpenConfigPanel")]
		[HarmonyPostfix]
		private static void InstallMenus()
		{
			if (!ElementStoreHouse.WarehouseStocked)
			{
				ElementStoreHouse.StockWarehouse();
			}

			foreach (ConfigMenu menu in ListOfMenus)
			{
				if (!menu.MenuInstalled)
				{
					menu.InstallMenu();
				}
			}
		}
		/// <summary>
		/// Create a new config menu in which you can make sections and add sliders, buttons, etc,. The button is automatically created and managed.
		/// </summary>
		/// <param name="name">The name of your menu to be used on the button.</param>
		/// <returns>Returns the menu to which you can add sections.</returns>
		public static ConfigMenu CreateConfigMenu(string name)
		{
			var configMenu = new ConfigMenu(name);

			ListOfMenus.Add(configMenu);

			return configMenu;
		}
		/// <summary>
		/// Remove a ConfigMenu entirely with sections controls and all.
		/// </summary>
		/// <param name="configMenu"></param>
		public static void RemoveConfigMenu(ConfigMenu configMenu)
		{
			if (ListOfMenus.Contains(configMenu))
			{
				ListOfMenus.Remove(configMenu);
				configMenu.RemoveMenu();
			}
		}
	}
}
