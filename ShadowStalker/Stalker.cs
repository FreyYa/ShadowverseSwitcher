using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowStalker
{
	public class Stalker
	{
		static public bool FindShadow()
		{
			foreach (Process process in Process.GetProcesses())
			{
				if (process.ProcessName.StartsWith("Shadowverse"))
				{
					return true;
				}
			}
			return false;
		}
	}
}
