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
using System.Windows.Forms;

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

		#region AccountName
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
		#endregion

		#region Memo
		private string _Memo;
		public string Memo
		{
			get { return this._Memo; }
			set
			{
				if (this._Memo == value) return;
				this._Memo = value;
			}
		}
		#endregion

		#region ErrorMessage
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
		#endregion

		#region LoadedList
		private List<Account> _LoadedList;
		public List<Account> LoadedList
		{
			get { return this._LoadedList; }
			set
			{
				this._LoadedList = value;
			}
		}
		#endregion

		public string GetSteamID()
		{
			string ID = "";
			string tmp, time;
			int value_compare = int.MaxValue;

			Dictionary<string, int> list = new Dictionary<string, int>();

			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.ShowDialog();
			var path = dialog.SelectedPath;
			TextReader tr = null;
			try
			{
				tr = new StreamReader(Path.Combine(path, "config", "loginusers.vdf"));
				try
				{
					while (true)
					{
						tmp = tr.ReadLine();
						if (tmp.Contains("AccountName"))
						{
							var IDstring = tmp.Split('\"');
							ID = IDstring[IDstring.Count() - 2];
							tr.ReadLine();
							tr.ReadLine();
							var timestring = tr.ReadLine().Split('\"');
							time = timestring[timestring.Count() - 2];
							list.Add(ID, int.Parse(time));
						}
					}
				}
				catch { }
			}
			catch
			{ Console.WriteLine("Error Reading steam account Info"); }
			if (tr != null)
				tr.Close();
			if (list.Count == 0) return "";
			foreach (var item in list)
			{
				if (item.Value < value_compare)
					ID = item.Key;
			}
			return ID;
		}
		public void Init()
		{
			LoadedList = new List<Account>();
			FileLoad();
		}
		private bool FileLoad()
		{
			return Core.Current.BinReader.ReadBin();
		}
		private bool FileWrite()
		{
			return Core.Current.BinWriter.SaveBin();
		}
		public bool Save()
		{
			try
			{
				Account temp = new Account();
				if (LoadedList.Any(x => x.SteamName == AccountName))
				{
					this.ErrorMessage = "동일한 계정명이 이미 존재합니다";
					return false;
				}
				temp.SteamName = this.AccountName;
				temp.Memo = this.Memo;
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
						this.ErrorMessage = "이미 저장되어있는 계정정보입니다: \n계정명: " + LoadedList.Where(x => x.NnB.SequenceEqual(nnb) && x.MHx5cg.SequenceEqual(mhx) && x.M3F1YS.SequenceEqual(m3f)).FirstOrDefault().SteamName;
						return false;
					}
					temp.MHx5cg = mhx;
					temp.M3F1YS = m3f;
					temp.NnB = nnb;
					temp.Header = new byte[0];
					temp.Header = temp.Header.Concat(BitConverter.GetBytes(temp.M3F1YS.Count())).ToArray();
					temp.Header = temp.Header.Concat(BitConverter.GetBytes(temp.NnB.Count())).ToArray();
					temp.Header = temp.Header.Concat(BitConverter.GetBytes(temp.MHx5cg.Count())).ToArray();
					key.Close();
				}
				temp.Created = DateTime.Now;
				LoadedList.Add(temp);
				//TODO: 바이너리 형태로 파일을 써넣어 저장하는 기능이 필요
				this.ChangeList();
				if (!FileWrite())
				{
					this.ErrorMessage = "파일 기록을 실패했습니다";
					return false;
				}
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
