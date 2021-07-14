using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.GUIAPI
{
	public class ConfigControlInputField : ConfigControl
	{
		private readonly string StartVal;
		private UIInput InputField;
		public string Value 	
		{
			get 
			{

				if (InputField) 
				{
					return InputField.value;
				}

				return StartVal;
				
			} 
			set 
			{

				if (base.MainObject)
				{
					InputField.value = value;
				}
			} 
		
		
		}
		public EventHandler ValueChanged;

		internal ConfigControlInputField(ConfigSection section, string name, string startval)
		{
			base.Name = name;
			StartVal = startval;
			base.Parent = section;
		}

		internal override void CreateControl()
		{
			base.MainObject = ElementStoreHouse.BuildConfigControlInputField(base.Parent.Table);
			MainObject.name = Name;

			MainObject.GetComponentsInChildren<UILabel>(true).First(go => go.name.Equals("TitleLabel")).text = base.Name;


			InputField = MainObject.GetComponentsInChildren<UIInput>()[0];
			InputField.value = StartVal;

			InputField.onSubmit.Add(new EventDelegate(() =>
			{
				try
				{
					ValueChanged.Invoke(this, null);
				}
				catch
				{

				}
			}));

			base.DoneCreating = true;
		}

		internal override void DeleteControl()
		{
			UnityEngine.Object.Destroy(base.MainObject);
			InputField = null;
			ValueChanged = null;
		}
	}
}
