using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class ConfigTabButton
	{
		public string Label;
		public EventDelegate MethodCall;
		public GameObject Button;
		public bool ButtonInstalled { get; private set; }
		public bool CategoryButton { get; private set; }
		internal ConfigTabButton(string label, EventDelegate eventDelegate, bool catButton) 
		{
			Label = label;
			MethodCall = eventDelegate;
			CategoryButton = catButton;
		}

		internal void CreateButton() 
		{
			Button = ElementStoreHouse.BuildConfigTabButton();
			Button.name = Label;

			var tabButtonComponent = Button.GetComponentsInChildren<UIWFTabButton>(true)[0];
			tabButtonComponent.onClick.Add(MethodCall);

			var label = Button.GetComponentsInChildren<UILabel>(true)[0];
			label.text = Label;

			if (!CategoryButton) 
			{
				var sprite = Button.GetComponentsInChildren<UISprite>(true)[0];
				var refSprite = ElementStoreHouse.DropDown.GetComponentsInChildren<UISprite>(true)[0];

				sprite.atlas = refSprite.atlas;
				sprite.spriteName = refSprite.spriteName;
				sprite.width = (int)(sprite.width * 0.75f);
				sprite.height = sprite.width;
			}

			ButtonInstalled = true;
		}
		internal void RemoveButton() 
		{
			MethodCall.Clear();
			UnityEngine.Object.Destroy(Button);
		}
	}
}
