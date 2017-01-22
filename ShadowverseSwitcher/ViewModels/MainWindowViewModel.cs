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
using System.Net;
using System.Collections.Specialized;
using System.Diagnostics;

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

		#region AccountList

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
		#endregion

		public MainWindowViewModel()
		{
			this.Title = "Shadow Servant!";
			this.AccountList = new List<Account>(Core.Current.FileManager.LoadedList);
			#region 이벤트
			Core.Current.FileManager.ChangeList += () =>
			{
				this.AccountList = new List<Account>(Core.Current.FileManager.LoadedList);
			};
			Core.Current.PopupManage.EndPopup += () =>
			{
				switch (Core.Current.PopupManage.Popup)
				{
					#region 초기화
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

									if (valList.Any(x => x == "M3F1YSNkOnF0_h4073495316"))
										key.DeleteValue("M3F1YSNkOnF0_h4073495316");
									if (valList.Any(x => x == "MHx5cg==_h786395497"))
										key.DeleteValue("MHx5cg==_h786395497");
									if (valList.Any(x => x == "NnB/ZDJpMHx5cg==_h354593472"))
										key.DeleteValue("NnB/ZDJpMHx5cg==_h354593472");

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
					#endregion
					#region 일/영 스위치
					case PopupKind.Switch:
						if (!Core.Current.PopupManage.IsUsed)
						{
							Core.Current.PopupManage.IsUsed = true;
							if (Core.Current.PopupManage.IsLeft)//영어
							{
								if (ShadowShifter.Patcher.SwitchingLanguage(Settings.Current.ShadowverseFolder, true))
									MainNotifier.Current.Show(App.ProductInfo.Title, "언어를 영어로 바꾸는데 성공했습니다.");
								else
									MainNotifier.Current.Show(App.ProductInfo.Title, "패치에 실패했습니다:\n" + ShadowShifter.Patcher.ErrorMsg);
							}
							else//일본어
							{
								if (ShadowShifter.Patcher.SwitchingLanguage(Settings.Current.ShadowverseFolder, false))
									MainNotifier.Current.Show(App.ProductInfo.Title, "언어를 일본어로 바꾸는데 성공했습니다.");
								else
									MainNotifier.Current.Show(App.ProductInfo.Title, "패치에 실패했습니다:\n" + ShadowShifter.Patcher.ErrorMsg);
							}
						}
						break;
					#endregion
					#region 데이터 로드
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

										key.Close();
										MainNotifier.Current.Show(App.ProductInfo.Title, "데이터 로드에 성공했습니다: " + Selected.SteamName, null);
									}
								}
								else MainNotifier.Current.Show(App.ProductInfo.Title, "선택된 데이터가 없습니다!", null);
							}
						}
						break;
						#endregion
				}

			};
			#endregion
		}
		public void PostTest()
		{
			try
			{
				string LoginURL = @"https://shadowverse-portal.com/api/v1/deck_code/publish?format=json&lang=en";

				WebClient webClient = new WebClient();

				NameValueCollection formData = new NameValueCollection();

				formData["hash"] = @"1.1.61jJY.61jJY.61jJY.61k2Q.61k2Q.61k2Q.61k32.61k32.61k32.61NLI.61QWQ.61QWQ.61QWQ.5zvAi.5zvAi.5zvAi.61oCC.61oCC.61oCC.65cLM.5zvvQ.5zvvQ.5zvvQ.65YC2.65YC2.65YC2.61qeS.61qeS.61qeS.61Q1A.61Q1A.61Q1A.61lmu.61lmu.65enI.61oww.61oww.61oww.61lmk.61lmk";
				formData["csrf_token"] = "";

				byte[] responseBytes = webClient.UploadValues(LoginURL, "POST", formData);
				string responsefromserver = Encoding.UTF8.GetString(responseBytes);

				webClient.Dispose();
				Debug.WriteLine(responsefromserver);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
		public void InitSetting()
		{
			if (!ShadowStalker.Stalker.FindShadow())
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
			else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 섀도우버스를 종료해주시기 바랍니다", null);
		}
		public void DeleteData()
		{
			if (this.Selected != null)
			{
				if (Core.Current.FileManager.RemoveUser(this.Selected.SteamName))
					MainNotifier.Current.Show(App.ProductInfo.Title, "성공적으로 삭제되었습니다", null);
				else MainNotifier.Current.Show(App.ProductInfo.Title, "파일 저장에 실패했습니다", null);
			}
			else
				MainNotifier.Current.Show(App.ProductInfo.Title, "선택된 계정이 없습니다.\n계정을 선택하고 다시 시도해주세요", null);
		}
		public void SaveSetting()
		{
			var popup = new AccountCreateWindowViewModel();
			var message = new TransitionMessage(popup, "Show/AccountCreateWindow");
			this.Messenger.RaiseAsync(message);
		}
		private void _steamPath(bool reSetting = false)
		{
			if(reSetting)
				MainNotifier.Current.Show("경로 설정 오류", "해당 경로에 섀도우버스가 설치되어있지 않습니다", null);
			string output;
			if (Settings.Current.SteamFolder != string.Empty)
				output = Settings.Current.SteamFolder;
			else
				output = "";

			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			if (output == "") dialog.Description = "섀도우버스가 설치된 SteamLibrary 폴더 혹은 스팀이 설치되어있는 폴더를 선택해주세요.\n" + @"예)C:\Program Files (x86)\Steam, \n   D:\SteamLibrary";
			else dialog.Description = "섀도우버스가 설치된 SteamLibrary 폴더 혹은 스팀이 설치되어있는 폴더를 선택해주세요.\n현재폴더: " + output;
			dialog.ShowNewFolderButton = true;
			if (Directory.Exists(Settings.Current.SteamFolder)) dialog.SelectedPath = Settings.Current.SteamFolder;
			var dia_result = dialog.ShowDialog();
			string selected = dialog.SelectedPath;

			//설정된 경로에 섀도우버스가 없으면 무한반복
			if (!Directory.Exists(Path.Combine(selected, "steamapps", "common", "Shadowverse")) && dia_result == System.Windows.Forms.DialogResult.OK)
			{
				_steamPath(true);
				return;
			}

			Settings.Current.SteamFolder = selected;
			Settings.Current.ShadowverseFolder = Path.Combine(selected, "steamapps", "common", "Shadowverse");
		}
		public void SteamPath()
		{
			_steamPath();
		}
		public void SetScreenShotFolder()
		{
			string output;
			if (Settings.Current.ScreenShotFolder != string.Empty)
				output = Settings.Current.ScreenShotFolder;
			else
				output = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.Description = "스크린샷을 저장할 폴더를 선택해주세요.\n현재폴더: " + output;
			dialog.ShowNewFolderButton = true;
			dialog.SelectedPath = Settings.Current.ScreenShotFolder;
			dialog.ShowDialog();
			string selected = dialog.SelectedPath;
			Settings.Current.ScreenShotFolder = selected;
		}
		public void LanguagePatch()
		{
			if (!ShadowStalker.Stalker.FindShadow())
			{
				if (Directory.Exists(Settings.Current.SteamFolder))
				{
					if (!Directory.Exists(Settings.Current.ShadowverseFolder))
						ShadowverseFolerSet();
					if (ShadowShifter.Patcher.PatchLanguage(Settings.Current.ShadowverseFolder))
						MainNotifier.Current.Show(App.ProductInfo.Title, "패치에 성공했습니다.");
					else
						MainNotifier.Current.Show(App.ProductInfo.Title, "패치에 실패했습니다:\n" + ShadowShifter.Patcher.ErrorMsg);
				}
				else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 Steam폴더 경로를 설정해주세요", null, null);

			}
			else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 섀도우버스를 종료해주시기 바랍니다", null);
		}

		private void ShadowverseFolerSet()
		{
			string output;
			if (Settings.Current.SteamFolder != string.Empty)
				output = Settings.Current.SteamFolder;
			else
				output = "";
			System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
			dialog.Description = "섀도우버스가 설치된 폴더를 선택해주세요.\n" + @"예)C:\Program Files (x86)\Steam\steamapps\common\Shadowverse";
			dialog.ShowNewFolderButton = true;
			if (Directory.Exists(Settings.Current.SteamFolder)) dialog.SelectedPath = Settings.Current.SteamFolder;
			dialog.ShowDialog();
			string selected = dialog.SelectedPath;
			Settings.Current.ShadowverseFolder = selected;
		}

		public void LanguageRollback()
		{
			if (!ShadowStalker.Stalker.FindShadow())
			{
				if (Directory.Exists(Settings.Current.SteamFolder))
				{
					if (!Directory.Exists(Settings.Current.ShadowverseFolder))
						ShadowverseFolerSet();
					if (ShadowShifter.Patcher.RollbackLanguage(Settings.Current.ShadowverseFolder))
						MainNotifier.Current.Show(App.ProductInfo.Title, "롤백에 성공했습니다.");
					else
						MainNotifier.Current.Show(App.ProductInfo.Title, "롤백에 실패했습니다");
				}
				else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 Steam폴더 경로를 설정해주세요", null, null);

			}
			else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 섀도우버스를 종료해주시기 바랍니다", null);
		}
		public void SwitchLanguage()
		{
			if (!ShadowStalker.Stalker.FindShadow())
			{
				if (Directory.Exists(Settings.Current.SteamFolder))
				{
					if (!Directory.Exists(Settings.Current.ShadowverseFolder))
						ShadowverseFolerSet();

					var popup = new DialogWindowViewModel();
					Core.Current.PopupManage.IsUsed = false;
					Core.Current.PopupManage.Popup = PopupKind.Switch;
					popup.MainMessage = "변경할 언어를 선택해주세요";
					popup.LeftMessage = "영어";
					popup.RightMessage = "일본어";
					var message = new TransitionMessage(popup, "Show/InitDialog");
					this.Messenger.RaiseAsync(message);
				}
				else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 Steam폴더 경로를 설정해주세요", null, null);

			}
			else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 섀도우버스를 종료해주시기 바랍니다", null);
		}
		public void LoadSetting()
		{
			if (!ShadowStalker.Stalker.FindShadow())
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
			else MainNotifier.Current.Show(App.ProductInfo.Title, "먼저 섀도우버스를 종료해주시기 바랍니다", null);
		}
	}
	public enum PopupKind
	{
		Init,
		Load,
		Switch,
	}
}
