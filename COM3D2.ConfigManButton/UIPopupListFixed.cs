using COM3D2.GUIAPI.Helper;
using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public class UIPopupListFixed : UIPopupList
	{
		private new void OnClick() 
		{
			if (this.openOn == UIPopupList.OpenOn.DoubleClick)
			{
				return;
			}
			if (this.openOn == UIPopupList.OpenOn.RightClick && UICamera.currentTouchID != -2)
			{
				return;
			}
			this.Show();
		}

		public new void Show()
		{
			if (base.enabled && NGUITools.GetActive(base.gameObject) && this.mChild == null && this.atlas != null && this.isValid && this.items.Count > 0)
			{
				this.mLabelList.Clear();
				if (this.mPanel == null)
				{
					this.mPanel = UIPanel.Find(base.transform);
					if (this.mPanel == null)
					{
						return;
					}
				}
				this.handleEvents = true;
				Transform transform = base.transform;
				this.mChild = new GameObject("Drop-down List");
				this.mChild.layer = base.gameObject.layer;
				Transform transform2 = this.mChild.transform;
				transform2.parent = transform.parent;
				Vector3 vector;
				Vector3 v;
				Vector3 vector2;
				if (this.openOn == UIPopupList.OpenOn.Manual && UICamera.selectedObject != base.gameObject)
				{
					base.StopCoroutine("CloseIfUnselected");
					vector = transform2.parent.InverseTransformPoint(this.mPanel.anchorCamera.ScreenToWorldPoint(UICamera.lastTouchPosition));
					v = vector;
					transform2.localPosition = vector;
					vector2 = transform2.position;
					base.StartCoroutine("CloseIfUnselected");
				}
				else
				{
					Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(transform.parent, transform, false, false);
					vector = bounds.min;
					v = bounds.max;
					transform2.localPosition = vector;
					vector2 = transform.position;
				}
				transform2.localRotation = Quaternion.identity;
				transform2.localScale = Vector3.one;
				this.mBackground = NGUITools.AddSprite(this.mChild, this.atlas, this.backgroundSprite);
				this.mBackground.pivot = UIWidget.Pivot.TopLeft;
				this.mBackground.depth = NGUITools.CalculateNextDepth(this.mPanel.gameObject);
				this.mBackground.color = this.backgroundColor;
				Vector4 border = this.mBackground.border;
				this.mBgBorder = border.y;
				this.mBackground.cachedTransform.localPosition = new Vector3(0f, border.y, 0f);
				this.mHighlight = NGUITools.AddSprite(this.mChild, this.atlas, this.highlightSprite);
				this.mHighlight.pivot = UIWidget.Pivot.TopLeft;
				this.mHighlight.color = this.highlightColor;
				UISpriteData atlasSprite = this.mHighlight.GetAtlasSprite();
				if (atlasSprite == null)
				{
					return;
				}
				float num = (float)atlasSprite.borderTop;
				float num2 = (float)this.activeFontSize;
				float activeFontScale = this.activeFontScale;
				float num3 = num2 * activeFontScale;
				float num4 = 0f;
				float num5 = -this.padding.y;
				List<UILabel> list = new List<UILabel>();
				if (!this.items.Contains(this.mSelectedItem))
				{
					this.mSelectedItem = null;
				}
				int i = 0;
				int count = this.items.Count;
				while (i < count)
				{
					string text = this.items[i];
					UILabel uilabel = NGUITools.AddWidget<UILabel>(this.mChild);
					uilabel.name = i.ToString();
					uilabel.pivot = UIWidget.Pivot.TopLeft;
					uilabel.bitmapFont = this.bitmapFont;
					uilabel.trueTypeFont = this.trueTypeFont;
					uilabel.fontSize = this.fontSize;
					uilabel.fontStyle = this.fontStyle;
					uilabel.text = text;
					uilabel.color = this.textColor;
					uilabel.cachedTransform.localPosition = new Vector3(border.x + this.padding.x - uilabel.pivotOffset.x, num5, -1f);
					uilabel.overflowMethod = UILabel.Overflow.ResizeFreely;
					uilabel.alignment = this.alignment;
					list.Add(uilabel);
					if (this.isLocalized)
					{
						string term = text.Replace("\r", string.Empty);
						if (this.itemTerms != null && this.itemTerms.Count == this.items.Count && !string.IsNullOrEmpty(this.itemTerms[i]))
						{
							term = this.itemTerms[i];
						}
						Localize localize = uilabel.gameObject.AddComponent<Localize>();
						localize.ForceOverFlowShrink = false;
						localize.SetTerm(term);
						NGUILabelLocalizeSupport component = uilabel.gameObject.GetComponent<NGUILabelLocalizeSupport>();
						if (component != null)
						{
							component.overRidePropertys.overFlow.value = UILabel.Overflow.ResizeFreely;
						}
					}
					num5 -= num3;
					num5 -= this.padding.y;
					num4 = Mathf.Max(num4, uilabel.printedSize.x);
					UIEventListener uieventListener = UIEventListener.Get(uilabel.gameObject);
					uieventListener.onHover = new UIEventListener.BoolDelegate(this.OnItemHover);
					uieventListener.onPress = new UIEventListener.BoolDelegate(this.OnItemPress);
					uieventListener.onClick = new UIEventListener.VoidDelegate(this.OnItemClick);
					uieventListener.parameter = text;
					if (this.mSelectedItem == text || (i == 0 && string.IsNullOrEmpty(this.mSelectedItem)))
					{
						this.Highlight(uilabel, true);
					}
					this.mLabelList.Add(uilabel);
					i++;
				}
				num4 = Mathf.Max(num4, (v.x - vector.x) * activeFontScale - (border.x + this.padding.x) * 2f);
				float num6 = num4;
				Vector3 vector3 = new Vector3(num6 * 0.5f, -num3 * 0.5f, 0f);
				Vector3 vector4 = new Vector3(num6, num3 + this.padding.y, 1f);
				int j = 0;
				int count2 = list.Count;
				while (j < count2)
				{
					UILabel uilabel2 = list[j];
					NGUITools.AddWidgetCollider(uilabel2.gameObject);
					uilabel2.autoResizeBoxCollider = false;
					BoxCollider component2 = uilabel2.GetComponent<BoxCollider>();
					if (component2 != null)
					{
						vector3.z = component2.center.z;
						component2.center = vector3;
						component2.size = vector4;
					}
					else
					{
						BoxCollider2D component3 = uilabel2.GetComponent<BoxCollider2D>();
						component3.offset = vector3;
						component3.size = vector4;
					}
					j++;
				}
				int width = Mathf.RoundToInt(num4);
				num4 += (border.x + this.padding.x) * 2f;
				num5 -= border.y;
				this.mBackground.width = Mathf.RoundToInt(num4);
				this.mBackground.height = Mathf.RoundToInt(-num5 + border.y);
				int k = 0;
				int count3 = list.Count;
				while (k < count3)
				{
					UILabel uilabel3 = list[k];
					uilabel3.overflowMethod = UILabel.Overflow.ShrinkContent;
					uilabel3.width = width;
					k++;
				}
				float num7 = 2f * this.atlas.pixelSize;
				float f = num4 - (border.x + this.padding.x) * 2f + (float)atlasSprite.borderLeft * num7;
				float f2 = num3 + num * num7;
				this.mHighlight.width = Mathf.RoundToInt(f);
				this.mHighlight.height = Mathf.RoundToInt(f2);
				bool flag = this.position == UIPopupList.Position.Above;
				if (this.position == UIPopupList.Position.Auto)
				{
					UICamera uicamera = UICamera.FindCameraForLayer((UICamera.selectedObject ?? base.gameObject).layer);
					if (uicamera != null)
					{
						flag = (uicamera.cachedCamera.WorldToViewportPoint(vector2).y < 0.5f);
					}
				}
				if (this.isAnimated)
				{
					float bottom = num5 + num3;
					this.Animate(this.mHighlight, flag, bottom);
					int l = 0;
					int count4 = list.Count;
					while (l < count4)
					{
						this.Animate(list[l], flag, bottom);
						l++;
					}
					this.AnimateColor(this.mBackground);
					this.AnimateScale(this.mBackground, flag, bottom);
				}
				if (flag)
				{
					transform2.localPosition = new Vector3(vector.x, v.y - num5 - border.y, vector.z);
				}
				vector = transform2.localPosition;
				v.x = vector.x + (float)this.mBackground.width;
				v.y = vector.y - (float)this.mBackground.height;
				v.z = vector.z;
				Vector3 b = this.mPanel.CalculateConstrainOffset(vector, v);
				transform2.localPosition += b;

				//Our fix is simple yet stupid. We check if the dropdown has a UISprite component and append our dropdown list to the bottom border of that. Results speak for themselves.
				var backgroundSprite = transform.gameObject.GetComponent<UISprite>();

				if (backgroundSprite)
				{
					if (position == UIPopupList.Position.Below) {

						mChild.transform.position = new Vector3(backgroundSprite.worldCorners[0].x, backgroundSprite.worldCorners[0].y);
					} else if(position == UIPopupList.Position.Above)
					{
						var labelHeight = (UIHelperFuncs.GetVerticalDistanceToMiddle(mLabelList.First()) * 2) * (mLabelList.Count+1);

						//Not perfect for some reason but it gets the job done.
						mChild.transform.position = new Vector3(backgroundSprite.worldCorners[1].x, backgroundSprite.worldCorners[1].y + labelHeight);
					}
				}
			}
			else
			{
				this.OnSelect(false);
			}

		}
	}
}
