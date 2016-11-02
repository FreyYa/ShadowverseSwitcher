using Microsoft.Win32;
using ShadowServant.Models.Notifier;
using Livet;
using ShadowServant.ViewModels;
using ShadowServant.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.Models
{
	public class FileManager : NotificationObject
	{
		#region EventHandler
		/// <summary>
		/// 이벤트 핸들러
		/// </summary>
		public delegate void EventHandler();
		public EventHandler ChangeList;
		#endregion
		private string _AccountName;
		public string AccountName
		{
			get { return this._AccountName; }
			set
			{
				if (this._AccountName == value) return;
				this._AccountName = value;
			}
		}

		private string _ErrorMessage;
		public string ErrorMessage
		{
			get { return this._ErrorMessage; }
			set
			{
				if (this._ErrorMessage == value) return;
				this._ErrorMessage = value;
			}
		}
		private List<Account> _LoadedList;
		public List<Account> LoadedList
		{
			get { return this._LoadedList; }
			set
			{
				this._LoadedList = value;
			}
		}
		public FileManager()
		{
			LoadedList = new List<Account>();
		}
		public bool Save()
		{
			try
			{
				Account temp = new Account();
				if (LoadedList.Any(x => x.Name == AccountName))
				{
					this.ErrorMessage = "동일한 계정명이 이미 존재합니다";
					return false;
				}
				temp.Name = AccountName;
				if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
				{
					var key = Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false);

					byte[] mhx = (byte[])key.GetValue("MHx5cg==_h786395497");
					byte[] m3f = (byte[])key.GetValue("M3F1YSNkOnF0_h4073495316");
					byte[] nnb = (byte[])key.GetValue("NnB/ZDJpMHx5cg==_h354593472");

					if (mhx == null || m3f == null || nnb == null)
					{
						this.ErrorMessage = "계정정보가 정확하지 않습니다";
						return false;
					}
					if (LoadedList.Any(x => x.NnB.SequenceEqual(nnb) && x.MHx5cg.SequenceEqual(mhx) && x.M3F1YS.SequenceEqual(m3f)))
					{
						this.ErrorMessage = "이미 저장되어있는 계정정보입니다: \n계정명: "+LoadedList.Where(x => x.NnB.SequenceEqual(nnb) && x.MHx5cg.SequenceEqual(mhx) && x.M3F1YS.SequenceEqual(m3f)).FirstOrDefault().Name;
						return false;
					}
					temp.MHx5cg = mhx;
					temp.M3F1YS = m3f;
					temp.NnB = nnb;

					//	var list = key.GetValueNames();
					//	List<string> output = new List<string>();
					//	output.Add("Windows Registry Editor Version 5.00");
					//	output.Add("");
					//	output.Add("[" + @key.ToString() + "]");
					//	string input = string.Empty;
					//	foreach (var item in list)
					//	{
					//		if (item == "MHx5cg==_h786395497" || item == "M3F1YSNkOnF0_h4073495316" || item == "NnB/ZDJpMHx5cg==_h354593472")
					//		{
					//			var kind = key.GetValueKind(item);
					//			switch (kind)
					//			{
					//				case RegistryValueKind.String:
					//					break;
					//				case RegistryValueKind.ExpandString:
					//					break;
					//				case RegistryValueKind.Binary:
					//					byte[] bytelist = (byte[])key.GetValue(item);
					//					string hex = BitConverter.ToString(bytelist).Replace('-', ',');
					//					input = "hex:" + hex;
					//					break;
					//				case RegistryValueKind.DWord:
					//					var dword = key.GetValue(item);
					//					input = "dword:" + String.Format("0x{0:X8}", dword);
					//					break;
					//				case RegistryValueKind.MultiString:
					//					break;
					//				case RegistryValueKind.QWord:
					//					break;
					//				case RegistryValueKind.Unknown:
					//					break;
					//				case RegistryValueKind.None:
					//					break;
					//				default:
					//					break;
					//			}
					//			output.Add("\"" + item + "\"=" + input);
					//		}

					//	}
					//	File.WriteAllLines("test.reg", output);
					key.Close();
				}
				LoadedList.Add(temp);
				this.ChangeList();
				MainNotifier.Current.Show(App.ProductInfo.Title, "현재 섀도우버스 계정의 연동정보를 저장하였습니다", null);
				AccountCreateWindow.Current.Close();
				return true;
			}
			catch (Exception e)
			{
				MainNotifier.Current.Show(App.ProductInfo.Title, "오류: " + e, null);
				return false;
			}
		}
	}
}
