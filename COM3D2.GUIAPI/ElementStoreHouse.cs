using COM3D2.GUIAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	class ElementStoreHouse
	{
		internal static GameObject PanelBackground;
		internal static Font TabButtonFont;

		internal static GameObject Menu;
		internal static GameObject Section;

		internal static GameObject Switch;
		internal static GameObject Slider;
		internal static GameObject DropDown;

		internal static UIPanel RootPanel;
		internal static int BackgroundDepth;

		internal static int SingleRowHeight;

		internal static bool WarehouseStocked { get; set; }
		/// <summary>
		/// Simple function which makes copies of elements we will reference later for cloning purposes.
		/// </summary>
		internal static void StockWarehouse()
		{
			TabButtonFont = GameObject.Find("SystemUI Root").GetComponentsInChildren<UILabel>(true).First(lab => lab.mTrueTypeFont != null && lab.mTrueTypeFont.name.Equals("NotoSansCJKjp-Thin")).trueTypeFont;

			PanelBackground = GameObject.Find("SystemUI Root").GetComponentsInChildren<UISprite>(true).First(so => so && so.gameObject && so.name.Equals("SOUND")).gameObject;

			Menu = GameObject.Find("SystemUI Root").GetComponentsInChildren<UIWidget>(true).First(so => so && so.gameObject && so.name.Equals("SystemTab")).gameObject;

			Section = Menu.GetComponentsInChildren<Transform>(true).First(so => so && so.gameObject && so.name.Equals("System")).gameObject;

			Switch = Section.GetComponentsInChildren<Transform>(true).First(so => so && so.gameObject && so.name.Equals("SysButtonShowAlways")).gameObject;
			Slider = Section.GetComponentsInChildren<Transform>(true).First(so => so && so.gameObject && so.name.Equals("PlayerModelTransparency")).gameObject;
			DropDown = Section.GetComponentsInChildren<Transform>(true).First(so => so && so.gameObject && so.name.Equals("Resolution")).gameObject;

			RootPanel = GameObject.Find("SystemUI Root").GetComponentsInChildren<UIPanel>(true).First(so => so && so.gameObject && so.name.Equals("ConfigPanel"));
			BackgroundDepth = GameObject.Find("SystemUI Root").GetComponentsInChildren<UISprite>(true).First(so => so && so.gameObject && so.name.Equals("BG")).depth;

			if (TabButtonFont && PanelBackground && Menu && Section && Switch && Slider && DropDown)
			{
				Main.BepLogger.LogDebug("Warehouse is full!");
				WarehouseStocked = true;
			}
			else 
			{
				Main.BepLogger.LogDebug("Warehouse is missing some components!");
			}
		}
		/// <summary>
		/// Creates a template button as a child of the parent gameobject.
		/// </summary>
		/// <returns>Returns the created GameObject</returns>
		internal static GameObject BuildConfigTabButton()
		{
			var button = NGUITools.AddChild(PanelBackground.transform.parent.gameObject, PanelBackground);

			var sprite = button.GetComponentsInChildren<UISprite>(true)[0];
			sprite.spriteName = "main_buttom";

			var drag = sprite.gameObject.AddComponent<UIDragScrollView>();

			var label = sprite.gameObject.AddComponent<UILabel>();
			label.width = sprite.width - 20;
			label.height = sprite.height - 20;
			label.trueTypeFont = TabButtonFont;
			label.color = new Color(0, 0, 0);
			label.fontSize = 21;
			label.depth = sprite.depth + 1;

			button.GetComponentsInChildren<UIWFTabButton>(true)[0].onClick.Clear();

			return button;
		}
		/// <summary>
		/// Builds an empty config menu and adds it to the parent of the other tabs.
		/// </summary>
		/// <returns></returns>
		internal static GameObject BuildConfigMenu()
		{

#if DEBUG
			Main.BepLogger.LogDebug("Building New Menu");
#endif

			var panelBackground = GameObject.Find("ConfigPanel").GetComponentsInChildren<UISprite>(true).First(so => so && so.gameObject && so.name.Equals("BG") && so.gameObject.transform.parent.gameObject.name.Equals("ConfigPanel"));

			//var rootObject = NGUITools.AddChild(Menu.transform.parent.gameObject);
			//rootObject.name = "Menu";

			var newMenu = NGUITools.AddChild(Menu.transform.parent.gameObject);
			newMenu.name = "New Menu";

			//UnityEngine.Object.Destroy(newMenu.GetComponentsInChildren<Transform>().First(to => to.gameObject.name.Equals("System")).gameObject);

			//UnityEngine.Object.Destroy(newMenu.GetComponentsInChildren<Transform>().First(to => to.gameObject.name.Equals("Message")).gameObject);

			var scrollview = NGUITools.AddChild<UIScrollView>(newMenu);
			scrollview.scrollWheelFactor = 2;
			scrollview.movement = UIScrollView.Movement.Vertical;
			scrollview.dragEffect = UIScrollView.DragEffect.None;

			var table = NGUITools.AddChild<UITable>(scrollview.gameObject);
			table.direction = UITable.Direction.Down;
			table.columns = 1;
			table.name = "MenuRootTable";
			table.padding = new Vector2(0, 50);
			table.pivot = UIWidget.Pivot.TopLeft;		

			Main.MouseWheelScroll += (s,e) => 
			{
				scrollview.Scroll((float)s);
			};

			var panel = newMenu.GetComponentInChildren<UIPanel>();
			panel.depth = RootPanel.depth + 1;
			panel.clipping = UIDrawCall.Clipping.SoftClip;
			panel.SetRect(0,0,panelBackground.width, 850);
			panel.transform.localPosition = new Vector3(0, 0, 0);
			panel.transform.localPosition = new Vector3(0, -128, 0);

			var sectionSprite = Section.GetComponentsInChildren<UISprite>(true).First(go => go.name.Equals("Frame"));

			table.transform.position = new Vector3(panelBackground.worldCorners[0].x + (UIHelperFuncs.GetHorizontalDistanceToMiddle(panelBackground) - UIHelperFuncs.GetHorizontalDistanceToMiddle(sectionSprite)), table.transform.position.y);

			var scrollbar = NGUITools.AddChild<UIScrollBar>(newMenu);
			scrollbar.transform.position = new Vector3(panel.worldCorners[3].x, panel.transform.position.y);
			scrollbar.transform.localRotation *= Quaternion.Euler(0f, 0f, -90f);
			scrollbar.value = 0;
			scrollview.Scroll(5);

			var refThumb = Slider.GetComponentsInChildren<UISprite>(true).First(go => go.name.Equals("Thumb"));

			var scrollBarSprite = NGUITools.AddChild<UISprite>(scrollbar.gameObject);
			scrollBarSprite.name = "Thumb";
			scrollBarSprite.atlas = refThumb.atlas;
			scrollBarSprite.spriteName = refThumb.spriteName;
			scrollBarSprite.type = refThumb.type;
			scrollBarSprite.SetDimensions(15, 15);
			scrollBarSprite.color = new Color(1, 1, 1);
			scrollBarSprite.depth = BackgroundDepth + 2;
			NGUITools.AddWidgetCollider(scrollBarSprite.gameObject);

			var scrollBarWidget = NGUITools.AddChild<UIWidget>(scrollbar.gameObject);
			scrollBarWidget.name = "ScrollTrack";
			scrollBarWidget.height = 15;
			scrollBarWidget.width = (int)(panelBackground.height*.75f);

			var refbackground = Slider.GetComponentsInChildren<UISprite>(true).First(go => go.name.Equals("Slider"));

			var scrollBarBackground = NGUITools.AddWidget<UISprite>(scrollbar.gameObject);
			scrollBarBackground.height = refbackground.height;
			scrollBarBackground.width = (int)(panelBackground.height * .75);
			scrollBarBackground.depth = scrollBarSprite.depth - 1;
			scrollBarBackground.atlas = refbackground.atlas;
			scrollBarBackground.spriteName = refbackground.spriteName;
			scrollBarBackground.type = refbackground.type;

			scrollbar.thumb = scrollBarSprite.transform;
			scrollbar.foregroundWidget = scrollBarWidget;
			scrollview.verticalScrollBar = scrollbar;

#if DEBUG
			Main.BepLogger.LogDebug("New Menu Built");
#endif

			return newMenu;
		}
		internal static GameObject BuildConfigSection(GameObject parentMenu)
		{
#if DEBUG
			Main.BepLogger.LogDebug("Building New ConfigSection");
#endif

			var parentPanel = parentMenu.GetComponentsInParent<UIPanel>(true).First(so => so && so.gameObject && so.name.Equals("ScrollView"));

#if DEBUG
			Main.BepLogger.LogDebug("Fetching parent panel.");
#endif

			var newSection = NGUITools.AddChild(parentMenu, Section);
			newSection.name = "New Section";

			UnityEngine.Object.Destroy(newSection.GetComponentsInChildren<Transform>(true).First(to => to.gameObject.name.Equals("SystemBlockLeft")).gameObject);

			UnityEngine.Object.Destroy(newSection.GetComponentsInChildren<Transform>(true).First(to => to.gameObject.name.Equals("SystemBlockRight")).gameObject);

#if DEBUG
			Main.BepLogger.LogDebug("Blocks removed...");
#endif

			var label = newSection.GetComponentsInChildren<UILabel>().First(to => to.gameObject.name.Equals("Title"));
			label.text = "New Section";

#if DEBUG
			Main.BepLogger.LogDebug("Label set...");
#endif

			var frameSprite = newSection.GetComponentsInChildren<UISprite>(true).First(to => to.gameObject.name.Equals("Frame"));
			SingleRowHeight = (int)(frameSprite.height*0.80f / 6);
			frameSprite.height = 0;
			frameSprite.transform.localPosition = new Vector3(0,0,0);

#if DEBUG
			Main.BepLogger.LogDebug("framesprite built.");
#endif

			label.width = frameSprite.width;
			label.pivot = UIWidget.Pivot.TopLeft;
			label.transform.localPosition = new Vector3(0,label.height,0);

#if DEBUG
			Main.BepLogger.LogDebug("Label fixed.");
#endif

			var table = NGUITools.AddChild<UITable>(frameSprite.gameObject);
			table.direction = UITable.Direction.Down;
			table.columns = 2;
			table.name = "Section Table";
			table.transform.localPosition = new Vector3(0,0,0);
			table.padding = new Vector2(25, 5);
			table.mPanel = parentPanel;
			table.pivot = UIWidget.Pivot.TopLeft;

			newSection.transform.localPosition = new Vector3(0, 0, 0);

#if DEBUG
			Main.BepLogger.LogDebug("Done building configsection.");
#endif

			return newSection;
		}
		internal static GameObject BuildConfigControlSwitch(GameObject section)
		{
			var newSwitch = NGUITools.AddChild(section, Switch);
			newSwitch.name = "Switch";
			newSwitch.GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
			newSwitch.GetComponentsInChildren<UILabel>()[0].text = "New Switch";

			var buttonFunc = newSwitch.GetComponentsInChildren<ConfigSelectButton>()[0];
			buttonFunc.onGetValue = null;
			buttonFunc.onSetValue = null;

			foreach (NGUILabelLocalizeSupport component in newSwitch.GetComponentsInChildren<NGUILabelLocalizeSupport>(true))
			{
				UnityEngine.Object.Destroy(component);
			}
			foreach (I2.Loc.Localize localizer in newSwitch.GetComponentsInChildren<I2.Loc.Localize>(true))
			{
				UnityEngine.Object.Destroy(localizer);
			}

			return newSwitch;
		}
		internal static GameObject BuildConfigControlSlider(GameObject section)
		{
			var newSlider = NGUITools.AddChild(section, Slider);
			newSlider.name = "NewSlider";
			newSlider.transform.localPosition = new Vector3(0,0,0);

			var label = newSlider.GetComponentsInChildren<UILabel>(true)[0];
			label.text = "New Slider";

			var slider = newSlider.GetComponentsInChildren<UISlider>(true)[0];
			slider.onDragFinished = null;
			slider.onChange = null;
			slider.value = 0;
			slider.Update();

			var sliderFunc = newSlider.GetComponentsInChildren<ConfigIntegerSlider>(true)[0];
			UnityEngine.Object.Destroy(sliderFunc);

			foreach (NGUILabelLocalizeSupport component in newSlider.GetComponentsInChildren<NGUILabelLocalizeSupport>(true))
			{
				UnityEngine.Object.Destroy(component);
			}

			UnityEngine.Object.Destroy(label.GetComponent<I2.Loc.Localize>());

			foreach (I2.Loc.Localize component in newSlider.GetComponentsInChildren<I2.Loc.Localize>(true))
			{
				UnityEngine.Object.Destroy(component);
			}

			var centerClick = newSlider.GetComponentsInChildren<UICenterOnClick2>(true).First();
			UnityEngine.Object.Destroy(centerClick);

			var keyNav = newSlider.GetComponentsInChildren<UIKeyNavigation5>(true).First();
			UnityEngine.Object.Destroy(keyNav);

			var buttonEdit = newSlider.GetComponentsInChildren<ButtonEdit>(true).First();
			UnityEngine.Object.Destroy(buttonEdit);

			foreach (UIButton component in newSlider.GetComponentsInChildren<UIButton>(true))
			{
				UnityEngine.Object.Destroy(component);
			}

			foreach (UIDragScrollView component in newSlider.GetComponentsInChildren<UIDragScrollView>(true))
			{
				UnityEngine.Object.Destroy(component);
			}

			return newSlider;
		}
		internal static GameObject BuildConfigControlDropDown(GameObject section) 
		{
			var newDropDown = NGUITools.AddChild(section, DropDown);
			newDropDown.name = "Dropdown";
			newDropDown.transform.localPosition = new Vector3(0,0,0);

			var label = newDropDown.GetComponentsInChildren<UILabel>(true).First(go => go.name.Equals("Title"));
			label.text = "New Dropdown";

			var label2 = newDropDown.GetComponentsInChildren<UILabel>(true).First(go => go.name.Equals("Label"));

			var dropdownFunc = newDropDown.GetComponentsInChildren<UIPopupList>(true).First();

			var newDropdownFunc = NGUITools.AddChild<UIPopupListFixed>(dropdownFunc.transform.parent.parent.gameObject);
			newDropdownFunc.trueTypeFont = dropdownFunc.trueTypeFont;
			newDropdownFunc.atlas = dropdownFunc.atlas;
			newDropdownFunc.backgroundSprite = dropdownFunc.backgroundSprite;
			newDropdownFunc.highlightSprite = dropdownFunc.highlightSprite;
			newDropdownFunc.mBackground = dropdownFunc.mBackground;
			newDropdownFunc.alignment = dropdownFunc.alignment;
			newDropdownFunc.eventReceiver = dropdownFunc.gameObject;
			newDropdownFunc.textColor = dropdownFunc.textColor;
			newDropdownFunc.textScale = dropdownFunc.textScale;
			newDropdownFunc.fontSize = dropdownFunc.fontSize;
			newDropdownFunc.highlightColor = dropdownFunc.highlightColor;
			newDropdownFunc.position = UIPopupList.Position.Below;

			var newUIbutton = newDropdownFunc.gameObject.AddComponent<UIButton>();
			newUIbutton.hover = new Color(1,1,1,1);

			newDropdownFunc.Start();

			var sprite = newDropdownFunc.gameObject.AddComponent<UISprite>();
			var sprite2 = dropdownFunc.GetComponent<UISprite>();

			sprite.atlas = sprite2.atlas;
			sprite.spriteName = sprite2.spriteName;
			sprite.width = sprite2.width;
			sprite.height = sprite2.height;
			sprite.type = sprite2.type;
			sprite.depth = sprite2.depth;

			sprite.pivot = sprite2.pivot;
			sprite.transform.position = sprite2.transform.position;

			var label3 = NGUITools.AddChild<UILabel>(newDropdownFunc.gameObject);
			label3.trueTypeFont = label2.trueTypeFont;
			label3.width = label2.width;
			label3.height = label2.height;
			label3.fontSize = label2.fontSize;
			label3.color = label2.color;
			label3.depth = label2.depth;
			label3.transform.position = label2.transform.position;
			newDropdownFunc.textLabel = label3;

			NGUITools.AddWidgetCollider(sprite.gameObject);

			UnityEngine.Object.Destroy(dropdownFunc.transform.parent.gameObject);

			foreach (NGUILabelLocalizeSupport component in newDropDown.GetComponentsInChildren<NGUILabelLocalizeSupport>(true))
			{
				UnityEngine.Object.Destroy(component);
			}

			foreach (I2.Loc.Localize component in newDropDown.GetComponentsInChildren<I2.Loc.Localize>(true))
			{
				UnityEngine.Object.Destroy(component);
			}

			return newDropDown;
		}
		internal static GameObject BuildConfigControlInputField(GameObject section) 
		{
			var rootObject = NGUITools.AddChild(section.gameObject).gameObject;
			rootObject.transform.parent = section.gameObject.transform;
			rootObject.transform.localPosition = new Vector3(0, 0);
			rootObject.name = "InputField";

			var newInputField = NGUITools.AddChild<UIInput>(rootObject);
			newInputField.name = "NewUIInput";

			var refTitleLabel = DropDown.GetComponentsInChildren<UILabel>(true).First(go => go.name.Equals("Title"));
			var titleLabel = NGUITools.AddChild<UILabel>(rootObject, refTitleLabel.gameObject);
			titleLabel.name = "TitleLabel";
			titleLabel.text = "New Inputfield";
			titleLabel.trueTypeFont = refTitleLabel.trueTypeFont;
			titleLabel.fontSize = refTitleLabel.fontSize;
			titleLabel.color = refTitleLabel.color;
			titleLabel.height = refTitleLabel.height;
			titleLabel.width = refTitleLabel.width;
			titleLabel.pivot = UIWidget.Pivot.TopLeft;

			var refsprite = DropDown.GetComponentsInChildren<UISprite>(true).First();
			var sprite = newInputField.gameObject.AddComponent<UISprite>();
			sprite.atlas = refsprite.atlas;
			sprite.spriteName = refsprite.spriteName;
			sprite.width = refsprite.width;
			sprite.height = refsprite.height;
			sprite.depth = refsprite.depth;
			sprite.type = refsprite.type;
			sprite.pivot = UIWidget.Pivot.Center;

			var spriteBorder = NGUITools.AddChild<UISprite>(sprite.gameObject);
			spriteBorder.atlas = sprite.atlas;
			spriteBorder.spriteName = sprite.spriteName;
			spriteBorder.width = (int)(sprite.width * 0.98f);
			spriteBorder.height = (int)(sprite.height * 0.95f);
			spriteBorder.depth = sprite.depth + 2;
			spriteBorder.type = sprite.type;
			spriteBorder.centerType = UIBasicSprite.AdvancedType.Invisible;
			spriteBorder.pivot = UIWidget.Pivot.Center;
			spriteBorder.color = new Color(0,0,0,0.8f);

			var refLabel = DropDown.GetComponentsInChildren<UILabel>(true).First(go => go.name.Equals("Label"));
			var label = NGUITools.AddWidget<UILabel>(newInputField.gameObject);
			label.trueTypeFont = refLabel.trueTypeFont;
			label.fontSize = refLabel.fontSize;
			label.width = sprite.width;
			label.height = sprite.height;
			label.color = new Color(0,0,0);
			label.depth = sprite.depth + 1;
			label.pivot = UIWidget.Pivot.Center;

			newInputField.inputType = UIInput.InputType.AutoCorrect;
			newInputField.label = label;
			newInputField.activeTextColor = new Color(0, 0, 0);
			newInputField.selectionColor = new Color(0, 0, 1, 0.2f);
			newInputField.caretColor = Color.black;
			newInputField.onReturnKey = UIInput.OnReturnKey.Submit;
			newInputField.transform.localPosition = new Vector3(0,0);

			//titleLabel.transform.localPosition = new Vector3(0, 0);
			titleLabel.transform.position = refTitleLabel.transform.position;
			sprite.transform.position = refsprite.transform.position;
			label.transform.localPosition = new Vector3(0, 0, 0);

			NGUITools.AddWidgetCollider(newInputField.gameObject);

			return rootObject.gameObject;
		}
	}
}
