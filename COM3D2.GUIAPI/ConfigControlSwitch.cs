using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ConfigControlSwitch : ConfigControl
	{
		public bool Value { get; set; }
		public EventHandler ValueChanged;

		internal ConfigControlSwitch(ConfigSection section, string name, bool startval) 
		{
			base.Name = name;
			Value = startval;
			base.Parent = section;
		}

		internal override void CreateControl()
		{
			base.MainObject = ElementStoreHouse.BuildConfigControlSwitch(base.Parent.Table);
			base.MainObject.name = Name;
			var label = MainObject.GetComponentsInChildren<UILabel>()[0].text = base.Name;

			var buttonFunc = MainObject.GetComponentsInChildren<ConfigSelectButton>()[0];
			buttonFunc.onGetValue = null;
			buttonFunc.onSetValue = null;

			buttonFunc.onSetValue = (newVal) => 
			{
				Value = newVal;
				try
				{
					ValueChanged.Invoke(this, null);
				}
				catch 
				{ 
					
				}
			};
			buttonFunc.onGetValue = () =>
			{
				return Value;
			};

			buttonFunc.UpdateButton();

			base.DoneCreating = true;
		}
		internal override void DeleteControl()
		{
			UnityEngine.Object.Destroy(base.MainObject);
			ValueChanged = null;
		}
	}
}
