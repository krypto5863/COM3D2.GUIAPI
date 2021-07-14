using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ConfigControlDropDown : ConfigControl
	{
		private List<string> ListOfItems = new List<string>();
		private readonly string StartVal;
		public EventHandler ValueChanged;
		private UIPopupList DropDown;
		public UIPopupList.Position Position 
		{
			get => DropDown.position;
			set => DropDown.position = value;
		}
		public string Value 
		{
			get 
			{
				if (DropDown) 
				{
					return DropDown.mSelectedItem;
				}

				return StartVal;
			}
			set
			{
				DropDown.mSelectedItem = Value;
			}
		}
		internal ConfigControlDropDown(ConfigSection section, string name, List<string> listOfItems, string startVal) 
		{
			base.Parent = section;
			base.Name = name;
			StartVal = startVal;

			ListOfItems = listOfItems;
		}

		internal override void CreateControl()
		{
			base.MainObject = ElementStoreHouse.BuildConfigControlDropDown(base.Parent.Table);
			MainObject.name = Name;

			var label = base.MainObject.GetComponentsInChildren<UILabel>(true).First(go => go.name.Equals("Title"));
			label.text = base.Name;

			DropDown = base.MainObject.GetComponentsInChildren<UIPopupListFixed>(true).First();
			DropDown.items = ListOfItems;
			DropDown.mSelectedItem = StartVal;

			DropDown.onChange.Add(new EventDelegate(() => {
				try
				{
					ValueChanged.Invoke(this, null);
				}
				catch { }	
			}));

			DropDown.Start();
			DropDown.openOn = UIPopupList.OpenOn.ClickOrTap;

			base.DoneCreating = true;
		}

		internal override void DeleteControl()
		{
			UnityEngine.Object.Destroy(base.MainObject);
			DropDown = null;
			ListOfItems = null;
			ValueChanged = null;
		}
	}
}
