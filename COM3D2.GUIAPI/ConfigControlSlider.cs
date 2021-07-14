using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ConfigControlSlider : ConfigControl
	{
		private UISlider Slider;
		public EventHandler ValueChanged;
		private readonly float StartVal;
		public float Value
		{
			get
			{
				if (Slider)
				{
					return Slider.value;
				}

				return StartVal;
			}
			set
			{
				Slider.value = value;
			}
		}

		internal ConfigControlSlider(ConfigSection section, string name, float startVal) 
		{
			base.Name = name;
			this.StartVal = startVal;
			base.Parent = section;
		}
		internal override void CreateControl()
		{
			base.MainObject = ElementStoreHouse.BuildConfigControlSlider(base.Parent.Table);
			base.MainObject.name = Name;

			MainObject.GetComponentsInChildren<UILabel>()[0].text = base.Name;

			Slider = MainObject.GetComponentsInChildren<UISlider>()[0];
			Slider.onDragFinished = () => 
			{
				//Main.logger.LogDebug($"Drag finished at a value of {Value}");
				try
				{
					ValueChanged.Invoke(this, null);
				}
				catch { }
			};

			Slider.value = StartVal;

			base.DoneCreating = true;
		}
		internal override void DeleteControl()
		{
			UnityEngine.Object.Destroy(base.MainObject);
			Slider = null;
			ValueChanged = null;
		}
	}
}
