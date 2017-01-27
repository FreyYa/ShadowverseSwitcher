using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ShadowServant.Models.Data.Xml;
using Livet;
using System.Windows;

namespace ShadowServant.Models
{
	[Serializable]
	public class Settings : NotificationObject
	{
		#region static members

		private static readonly string filePath = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"FreyYa.tistory.com",
			"ShadowServant",
			"Settings.xml");
		private static readonly string CurrentSettingsVersion = "1.0";
		public static Settings Current { get; set; }
		public static void Load()
		{
			try
			{
				Current = filePath.ReadXml<Settings>();
				if (Current.SettingsVersion != CurrentSettingsVersion)
					Current = GetInitialSettings();
			}
			catch (Exception ex)
			{
				Current = GetInitialSettings();
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}

		public static Settings GetInitialSettings()
		{
			return new Settings
			{
				SettingsVersion = CurrentSettingsVersion,
				ScreenShotFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
			};
		}

		#endregion

		#region SettingsVersion

		private string _SettingsVersion;

		public string SettingsVersion
		{
			get { return this._SettingsVersion; }
			set
			{
				if (this._SettingsVersion != value)
				{
					this._SettingsVersion = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region 스크린샷 폴더 저장

		private string _ScreenShotFolder;

		public string ScreenShotFolder
		{
			get { return this._ScreenShotFolder; }
			set
			{
				if (this._ScreenShotFolder != value)
				{
					this._ScreenShotFolder = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		#region 섀도우버스 폴더 저장

		private string _ShadowverseFolder;

		public string ShadowverseFolder
		{
			get { return this._ShadowverseFolder; }
			set
			{
				if (this._ShadowverseFolder != value)
				{
					this._ShadowverseFolder = value;
					this.RaisePropertyChanged();
				}
			}
		}
		#endregion

		public void Save()
		{
			try
			{
				this.WriteXml(filePath);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex);
			}
		}
	}
}
