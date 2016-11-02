using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Mvvm;
using Microsoft.Win32;
using ShadowServant.Models.Notifier;
using System.IO;

namespace ShadowServant.ViewModels
{
	public class MainWindowViewModel : WindowViewModel
	{
		public MainWindowViewModel()
		{
			this.Title = "Shadow Servant!";
		}
		public void InitSetting()
		{
			if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
			{
				Registry.CurrentUser.DeleteSubKeyTree(@"Software\Cygames\Shadowverse");
			}
			MainNotifier.Current.Show(App.ProductInfo.Title, "섀도우버스의 계정연동 및 모든 설정정보를 초기화하였습니다", null);
		}
		public void SaveSetting()
		{
			if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
			{
				var key = Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false);
				var list = key.GetValueNames();
				List<string> output = new List<string>();
				output.Add("Windows Registry Editor Version 5.00");
				output.Add("");
				output.Add("["+@key.ToString()+"]");
				string input=string.Empty;
				foreach (var item in list)
				{
					if(item== "MHx5cg==_h786395497" ||item== "M3F1YSNkOnF0_h4073495316" || item== "NnB/ZDJpMHx5cg==_h354593472")
					{
						var kind = key.GetValueKind(item);
						switch (kind)
						{
							case RegistryValueKind.String:
								break;
							case RegistryValueKind.ExpandString:
								break;
							case RegistryValueKind.Binary:
								byte[] bytelist = (byte[])key.GetValue(item);
								string hex = BitConverter.ToString(bytelist).Replace('-', ',');
								input = "hex:" + hex;
								break;
							case RegistryValueKind.DWord:
								var dword = key.GetValue(item);
								input = "dword:" + String.Format("0x{0:X8}", dword);
								break;
							case RegistryValueKind.MultiString:
								break;
							case RegistryValueKind.QWord:
								break;
							case RegistryValueKind.Unknown:
								break;
							case RegistryValueKind.None:
								break;
							default:
								break;
						}
						output.Add("\"" + item + "\"=" + input);
					}
					
				}
				File.WriteAllLines("test.reg", output);
				key.Close();
			}
			
			MainNotifier.Current.Show(App.ProductInfo.Title, "섀도우버스의 현재 계정연동 및 모든 설정정보를 저장하였습니다", null);
		}
		public string ConvertStringToHex(string asciiString)
		{
			string hex = "";
			foreach (char c in asciiString)
			{
				int tmp = c;
				hex += String.Format("{0:x2}", (uint)System.Convert.ToUInt32(tmp.ToString()));
			}

			return hex;
		}
		public string ConvertHexToString(string HexValue)
		{
			string StrValue = "";
			while (HexValue.Length > 0)
			{
				StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
				HexValue = HexValue.Substring(2, HexValue.Length - 2);
			}

			return StrValue;
		}
	}
}
