using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ConfigMenu
	{
		public string MenuName;
		public GameObject MenuRoot;
		public GameObject RootTable;
		public ConfigTabButton TabButton;
		internal bool MenuInstalled = false;
		public static EventHandler ButtonWasClicked;

		private List<ConfigSection> ListOfSections = new List<ConfigSection>();

		internal ConfigMenu(string menuName) 
		{
			MenuName = menuName;
			TabButton = ButtonHandler.CreateConfigTabButton(menuName, new EventDelegate(() => ButtonWasClicked.Invoke(this, null) ), true);

			ButtonWasClicked += (s, a) => HandleButtonClick(s);
			ButtonHandler.VanillaMenuOpened += (s, a) => HandleButtonClick(s);
		}
		internal void InstallMenu()
		{
			MenuRoot = ElementStoreHouse.BuildConfigMenu();
			MenuRoot.name = MenuName;
			MenuRoot.SetActive(false);

			RootTable = MenuRoot.GetComponentInChildren<UITable>().gameObject;

			foreach (ConfigSection section in ListOfSections) 			
			{
				if (!section.IsInstalled) 
				{
					section.CreateSection();
				}
			}

			MenuInstalled = true;
		}
		internal void RemoveMenu() 
		{
			foreach (ConfigSection sec in ListOfSections) 
			{
				sec.DeleteSection();
			}

			ButtonHandler.RemoveConfigTabButton(TabButton);
			TabButton = null;
			UnityEngine.Object.Destroy(MenuRoot);
			ListOfSections = null;
		}

		internal void HandleButtonClick(object sender) 
		{
			if (MenuRoot) 
			{
				if (sender != this)
				{
					MenuRoot.SetActive(false);
					SetButtonToClicked(false);
				}
				else
				{
					MenuRoot.SetActive(true);
					SetButtonToClicked(true);
				}
			}
		}
		private void SetButtonToClicked(bool isSelected) 
		{
			var wFButton = TabButton.Button.GetComponent<UIWFTabButton>();
			wFButton.SetSelect(isSelected);
		}
		public ConfigSection AddSection(string name) 
		{
			var section = new ConfigSection(name, this);

			ListOfSections.Add(section);

			return section;
		}
		public void RemoveSection(ConfigSection section) 
		{
			if (ListOfSections.Contains(section)) 
			{
				ListOfSections.Remove(section);
				section.DeleteSection();
			}
		}
	}
}
