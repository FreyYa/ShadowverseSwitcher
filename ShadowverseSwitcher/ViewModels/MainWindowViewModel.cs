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
			Core.Current.PopupManage.EndPopup += () =>
			  {
				  switch (Core.Current.PopupManage.Popup)
				  {
					  case PopupKind.Init:
						  if (!Core.Current.PopupManage.IsUsed)
						  {
							  Core.Current.PopupManage.IsUsed = true;
							  if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
							  {
								  if (Core.Current.PopupManage.IsLeft)
								  {

									  Registry.CurrentUser.DeleteSubKeyTree(@"Software\Cygames\Shadowverse");
									  MainNotifier.Current.Show(App.ProductInfo.Title, "섀도우버스의 계정연동 및 모든 설정정보를 초기화하였습니다", null);

								  }
								  else
								  {
									  var key = Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", true);

									  var valList = key.GetValueNames().ToList();
									  if (valList.Any(x => x == "HOME_CARD_INDEX_h3315159488"))
										  key.DeleteValue("HOME_CARD_INDEX_h3315159488");
									  if (valList.Any(x => x == "LAST_BATTLE_DECK_ID_h4121485982"))
										  key.DeleteValue("LAST_BATTLE_DECK_ID_h4121485982");
									  if (valList.Any(x => x == "LAST_BATTLE_IS_DEFDECK_h50217934"))
										  key.DeleteValue("LAST_BATTLE_IS_DEFDECK_h50217934");
									  if (valList.Any(x => x == "LAST_BATTLE_LEADER_ID_h581834284"))
										  key.DeleteValue("LAST_BATTLE_LEADER_ID_h581834284");
									  if (valList.Any(x => x == "LAST_SELECT_DECK_ID_h4260907292"))
										  key.DeleteValue("LAST_SELECT_DECK_ID_h4260907292");
									  if (valList.Any(x => x == "LAST_SELECT_IS_DEFDECK_h1993347724"))
										  key.DeleteValue("LAST_SELECT_IS_DEFDECK_h1993347724");
									  if (valList.Any(x => x == "LastTraceLog1_h3744039899"))
										  key.DeleteValue("LastTraceLog1_h3744039899");
									  if (valList.Any(x => x == "LastTraceLog2_h3744039896"))
										  key.DeleteValue("LastTraceLog2_h3744039896");

									  MainNotifier.Current.Show(App.ProductInfo.Title, "로컬 계정연동 데이터 삭제에 성공했습니다", null);
								  }
							  }
							  else MainNotifier.Current.Show(App.ProductInfo.Title, "섀도우버스의 설정정보가 없습니다", null);
						  }
						  break;
					  case PopupKind.Load:
						  if (!Core.Current.PopupManage.IsUsed)
						  {
							  Core.Current.PopupManage.IsUsed = true;
							  if (Core.Current.PopupManage.IsLeft)
							  {
								  if (this.Selected != null)
								  {
									  if (Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", false) != null)
									  {
										  var key = Registry.CurrentUser.OpenSubKey(@"Software\Cygames\Shadowverse", true);
										  key.SetValue("M3F1YSNkOnF0_h4073495316", Selected.M3F1YS, RegistryValueKind.Binary);
										  key.SetValue("MHx5cg==_h786395497", Selected.MHx5cg, RegistryValueKind.Binary);
										  key.SetValue("NnB/ZDJpMHx5cg==_h354593472", Selected.NnB, RegistryValueKind.Binary);

										  var valList = key.GetValueNames().ToList();
										  if(valList.Any(x=>x== "HOME_CARD_INDEX_h3315159488"))
											  key.DeleteValue("HOME_CARD_INDEX_h3315159488");
										  if (valList.Any(x => x == "LAST_BATTLE_DECK_ID_h4121485982"))
											  key.DeleteValue("LAST_BATTLE_DECK_ID_h4121485982");
										  if (valList.Any(x => x == "LAST_BATTLE_IS_DEFDECK_h50217934"))
											  key.DeleteValue("LAST_BATTLE_IS_DEFDECK_h50217934");
										  if (valList.Any(x => x == "LAST_BATTLE_LEADER_ID_h581834284"))
											  key.DeleteValue("LAST_BATTLE_LEADER_ID_h581834284");
										  if (valList.Any(x => x == "LAST_SELECT_DECK_ID_h4260907292"))
											  key.DeleteValue("LAST_SELECT_DECK_ID_h4260907292");
										  if (valList.Any(x => x == "LAST_SELECT_IS_DEFDECK_h1993347724"))
											  key.DeleteValue("LAST_SELECT_IS_DEFDECK_h1993347724");
										  if (valList.Any(x => x == "LastTraceLog1_h3744039899"))
											  key.DeleteValue("LastTraceLog1_h3744039899");
										  if (valList.Any(x => x == "LastTraceLog2_h3744039896"))
											  key.DeleteValue("LastTraceLog2_h3744039896");

										  key.Close();
										  MainNotifier.Current.Show(App.ProductInfo.Title, "데이터 로드에 성공했습니다: " + Selected.SteamName, null);
									  }
								  }
								  else MainNotifier.Current.Show(App.ProductInfo.Title, "선택된 데이터가 없습니다!", null);
							  }
						  }
						  break;
				  }

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
			var popup = new DialogWindowViewModel();
			Core.Current.PopupManage.IsUsed = false;
			Core.Current.PopupManage.Popup = PopupKind.Init;
			popup.MainMessage = "설정정보를 모두 삭제하시겠습니까?";
			popup.LeftMessage = "전체삭제";
			popup.RightMessage = "로컬 계정정보만 삭제";
			var message = new TransitionMessage(popup, "Show/InitDialog");
			this.Messenger.RaiseAsync(message);
		}
		public void SaveSetting()
		{
			var popup = new AccountCreateWindowViewModel();
			var message = new TransitionMessage(popup, "Show/AccountCreateWindow");
			this.Messenger.RaiseAsync(message);
		}
		public void LoadSetting()
		{
			if (this.Selected == null)
			{
				MainNotifier.Current.Show(App.ProductInfo.Title, "선택된 계정이 없습니다.\n계정을 선택하고 다시 시도해주세요", null);
				return;
			}
			var popup = new DialogWindowViewModel();
			Core.Current.PopupManage.IsUsed = false;
			Core.Current.PopupManage.Popup = PopupKind.Load;
			popup.MainMessage = "데이터를 로드합니다: " + this.Selected.SteamName;
			popup.LeftMessage = "로드";
			popup.RightMessage = "취소";
			var message = new TransitionMessage(popup, "Show/InitDialog");
			this.Messenger.RaiseAsync(message);
		}
	}
	public enum PopupKind
	{
		Init,
		Load
	}
}
