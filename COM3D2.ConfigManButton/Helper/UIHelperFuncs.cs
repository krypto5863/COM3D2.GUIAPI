using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI.Helper
{
	class UIHelperFuncs
	{
		public static float GetHorizontalDistanceToMiddle(UIRect rect1) 
		{
			return Vector3.Distance(rect1.worldCorners[1], rect1.worldCorners[2])/2;
		}
		public static float GetVerticalDistanceToMiddle(UIRect rect1)
		{
			return Vector3.Distance(rect1.worldCorners[0], rect1.worldCorners[1])/2;
		}
	}
}
