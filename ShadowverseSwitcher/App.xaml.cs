using Livet;
using ShadowServant.Models;
using ShadowServant.Models.Notifier;
using ShadowServant.ViewModels;
using ShadowServant.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace ShadowServant
{
	/// <summary>
	/// App.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class App : Application
	{
		static string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
		public static MainWindowViewModel ViewModelRoot { get; private set; }
		public static ProductInfo ProductInfo { get; private set; }

		static App()
		{
			AppDomain.CurrentDomain.UnhandledException += (sender, args) => ReportException(sender, args.ExceptionObject as Exception);
		}
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			ProductInfo = new ProductInfo();
			this.DispatcherUnhandledException += (sender, args) =>
			{
				ReportException(sender, args.Exception);
				args.Handled = true;
			};

			DispatcherHelper.UIDispatcher = this.Dispatcher;
			MainNotifier.Current.Initialize();
			Core.Current.FileManager.Init();
			ViewModelRoot = new MainWindowViewModel();
			this.MainWindow = new MainWindow { DataContext = ViewModelRoot };
			this.MainWindow.Show();
		}
		protected override void OnExit(ExitEventArgs e)
		{
			base.OnExit(e);
			MainNotifier.Current.Dispose();
		}
		#region 리포트
		private static void ReportException(object sender, Exception exception)
		{
			#region const
			const string messageFormat = @"
===========================================================
ERROR, date = {0}, sender = {1},
{2}
";
			string path = Path.Combine(MainFolder, "error.log");
			#endregion

			try
			{
				var message = string.Format(messageFormat, DateTimeOffset.Now, sender, exception);

				Debug.WriteLine(message);
				File.AppendAllText(path, message);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		}
		#endregion
	}
}
