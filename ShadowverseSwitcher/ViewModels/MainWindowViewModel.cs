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
using ShadowServant.Models;
using Livet.Messaging;

namespace ShadowServant.ViewModels
{
	public class MainWindowViewModel : WindowViewModel
	{
		#region Selected
		private Account _Selected;
		public Account Selected
		{
			get { return this._Selected; }
			set
			{
				if (this._Selected == value) return;
				this._Selected = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public MainWindowViewModel()
		{
			this.Title = "Shadow Servant!";
			this.AccountList = new List<Account>(Core.Current.FileManager.LoadedList);

			Core.Current.FileManager.ChangeList += () =>
			  {
				  this.AccountList = new List<Account>(Core.Current.FileManager.LoadedList);
			  };
		}
		private List<Account> _AccountList;
		public List<Account> AccountList
		{
			get { return this._AccountList; }
			set
			{
				this._AccountList = value;
				this.RaisePropertyChanged();
			}
		}
		public void InitSetting()
		{
			if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
			{
				Registry.CurrentUser.DeleteSubKeyTree(@"Software\Cygames\Shadowverse");
				MainNotifier.Current.Show(App.ProductInfo.Title, "섀도우버스의 계정연동 및 모든 설정정보를 초기화하였습니다", null);
			}
			else MainNotifier.Current.Show(App.ProductInfo.Title, "섀도우버스의 설정정보가 없습니다", null);
		}
		public void SaveSetting()
		{
			var popup = new AccountCreateWindowViewModel();
			var message = new TransitionMessage(popup, "Show/AccountCreateWindow");
			this.Messenger.RaiseAsync(message);
		}
		public void LoadSetting()
		{

		}
	}
}
