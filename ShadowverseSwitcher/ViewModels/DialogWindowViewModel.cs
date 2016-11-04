using Livet;
using ShadowServant.Models;
using ShadowServant.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.ViewModels
{
	public class DialogWindowViewModel : WindowViewModel
	{
		#region MainMessage
		private string _MainMessage;
		public string MainMessage
		{
			get { return this._MainMessage; }
			set
			{
				if (this._MainMessage == value) return;
				this._MainMessage = value;
				this.RaisePropertyChanged();
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

		#region LeftMessage
		private string _LeftMessage;
		public string LeftMessage
		{
			get { return this._LeftMessage; }
			set
			{
				if (this._LeftMessage == value) return;
				this._LeftMessage = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region RightMessage
		private string _RightMessage;
		public string RightMessage
		{
			get { return this._RightMessage; }
			set
			{
				if (this._RightMessage == value) return;
				this._RightMessage = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		#region IsLeft
		private bool _IsLeft;
		public bool IsLeft
		{
			get { return this._IsLeft; }
			set
			{
				if (this._IsLeft == value) return;
				this._IsLeft = value;
				this.RaisePropertyChanged();
			}
		}
		#endregion

		public void Left()
		{
			Core.Current.PopupManage.IsLeft = true;
			DialogWindow.Current.Close();
		}
		public void Right()
		{
			Core.Current.PopupManage.IsLeft = false;
			DialogWindow.Current.Close();
		}
		public DialogWindowViewModel()
		{

		}
	}
}
