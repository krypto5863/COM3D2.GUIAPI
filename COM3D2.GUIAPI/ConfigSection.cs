using COM3D2.GUIAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ConfigSection
	{
		private readonly string Name;
		private List<ConfigControl> ListOfControls = new List<ConfigControl>();

		readonly ConfigMenu ParentMenu;
		GameObject SectionObject;
		public GameObject Table { get; private set; }
		internal bool IsInstalled {get; private set;}


		internal ConfigSection(string name, ConfigMenu parent) 
		{
			Name = name;
			ParentMenu = parent;
		}

		internal void CreateSection() 
		{
			if (!IsInstalled)
			{
				SectionObject = ElementStoreHouse.BuildConfigSection(ParentMenu.RootTable);
				SectionObject.name = Name;
				var label = SectionObject.GetComponentsInChildren<UILabel>().First(lab => lab.name.Equals("Title"));
				label.text = Name;

				Table = SectionObject.GetComponentsInChildren<Transform>().First(lab => lab.name.Equals("Section Table")).gameObject;
			}

			foreach (ConfigControl control in ListOfControls) 
			{
				if (!control.DoneCreating) 
				{
					control.CreateControl();
				}
			}

			var frameSprite = SectionObject.GetComponentsInChildren<UISprite>(true).First(go => go.name.Equals("Frame"));
			frameSprite.height = (ElementStoreHouse.SingleRowHeight) * ((ListOfControls.Count+1)/ 2);

			Main.BepLogger.LogDebug("Fetch table from table...");

			var table = Table.GetComponent<UITable>();
			table.transform.localPosition = new Vector3(0, 0, 0);
			table.padding = new Vector2(25, 6);
			ParentMenu.RootTable.GetComponent<UITable>().Reposition();
			table.Reposition();

			Main.BepLogger.LogDebug("Calling update on scrollview...");

			ParentMenu.MenuRoot.GetComponentInChildren<UIScrollView>().UpdatePosition();

			Main.BepLogger.LogDebug($"Done building section {Name}");

			IsInstalled = true;
		}
		internal void DeleteSection()
		{
			foreach (ConfigControl control in ListOfControls)
			{
				control.DeleteControl();
			}

			ListOfControls = null;
			UnityEngine.Object.Destroy(SectionObject);
		}

		public ConfigControlSwitch AddSwitchControl(string name, bool start = false)
		{
			var newSwitch = new ConfigControlSwitch(this, name, start);

			ListOfControls.Add(newSwitch);

			if (IsInstalled) 
			{
				CreateSection();
			}

			return newSwitch;
		}
		public ConfigControlSlider AddSliderControl(string name, float startVal = 0)
		{
			var newSlider = new ConfigControlSlider(this, name, startVal);

			ListOfControls.Add(newSlider);

			if (IsInstalled)
			{
				CreateSection();
			}

			return newSlider;
		}
		public ConfigControlDropDown AddDropDownControl(string name, List<string> Items, string startVal = null)
		{
			var newDropdown = new ConfigControlDropDown(this, name, Items, startVal);

			ListOfControls.Add(newDropdown);

			if (IsInstalled)
			{
				CreateSection();
			}

			return newDropdown;
		}
		public ConfigControlInputField AddInputFieldControl(string name, string startVal = null)
		{
			var newInputField = new ConfigControlInputField(this, name, startVal);

			ListOfControls.Add(newInputField);

			if (IsInstalled)
			{
				CreateSection();
			}

			return newInputField;
		}
		public void DeleteControl(ConfigControl control) 
		{
			if (ListOfControls.Contains(control)) 
			{
				ListOfControls.Remove(control);
				control.DeleteControl();

				if (IsInstalled)
				{
					CreateSection();
				}
			}
		}
	}
}
