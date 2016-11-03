using Microsoft.Win32;
using ShadowServant.Models;
using ShadowServant.Models.Notifier;
using ShadowServant.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.ViewModels
{
	public class AccountCreateWindowViewModel:WindowViewModel
	{
		#region AccountName
		public string AccountName
		{
			get { return Core.Current.FileManager.AccountName; }
			set
			{
				if (Core.Current.FileManager.AccountName == value) return;
				Core.Current.FileManager.AccountName = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region Memo
		public string Memo
		{
			get { return Core.Current.FileManager.Memo; }
			set
			{
				if (Core.Current.FileManager.Memo == value) return;
				Core.Current.FileManager.Memo = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region Message
		private string _Message;
		public string Message
		{
			get { return this._Message; }
			set
			{
				if (this._Message == value) return;
				this._Message = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public AccountCreateWindowViewModel()
		{
			this.Title = "새 계정정보 추가";
			this.AccountName = "";
		}
		public void Confirm()
		{
			if (this.AccountName == "")
			{
				this.Message = "계정이름을 입력해주시기 바랍니다";
				return;
			}
			if (!Core.Current.FileManager.Save())
				this.Message = "저장에 실패하였습니다! : "+Core.Current.FileManager.ErrorMessage;
		}
		public void Cancel()
		{
			AccountCreateWindow.Current.Close();
		}
	}
}
