using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Mvvm;
using Microsoft.Win32;

namespace ShadowverseSwitcher.ViewModels
{
	public class MainWindowViewModel : WindowViewModel
	{
		public MainWindowViewModel()
		{
			this.Title = "Shadowverse Switcher Beta";
		}
		public void InitSetting()
		{
			if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
			{
				Registry.CurrentUser.DeleteSubKeyTree(@"Software\Cygames\Shadowverse");
			}
		}
	}
}
