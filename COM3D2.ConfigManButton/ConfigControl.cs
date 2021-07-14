using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.GUIAPI
{
	public abstract class ConfigControl
	{
		internal string Name { get; set; }
		internal GameObject MainObject { get; set; }
		internal ConfigSection Parent { get; set; }
		internal bool DoneCreating { get; set; }

		internal abstract void CreateControl();

		internal abstract void DeleteControl();
	}
}
