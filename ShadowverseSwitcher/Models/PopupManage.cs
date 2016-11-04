using ShadowServant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.Models
{
	public class PopupManage
	{

		#region EventHandler
		/// <summary>
		/// 이벤트 핸들러
		/// </summary>
		public delegate void EventHandler();
		public EventHandler EndPopup;
		#endregion

		public PopupManage()
		{
			this.IsUsed = false;
		}
		public PopupKind Popup { get; set; }
		public bool IsUsed { get; set; }
		private bool _IsLeft;
		public bool IsLeft
		{
			get { return this._IsLeft; }
			set
			{
				this._IsLeft = value;
				this.EndPopup();
			}
		}
	}
}
