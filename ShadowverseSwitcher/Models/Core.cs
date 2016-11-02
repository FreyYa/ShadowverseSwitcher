using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.Models
{
	public class Core
	{
		#region singleton

		private static Core current = new Core();

		public static Core Current
		{
			get { return current; }
		}

		#endregion

		public FileManager FileManager { get; set; }
		public Core()
		{
			this.FileManager = new FileManager();
		}
	}
}
