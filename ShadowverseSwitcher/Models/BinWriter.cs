using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ShadowServant.Models;

namespace ShadowServant.Models
{
	public class BinWriter
	{
		private string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
		public bool SaveBin()
		{
			try
			{
				var binPath = Path.Combine(MainFolder, "Data", "AccountInfo");

				using (var fileStream = new FileStream(binPath, FileMode.Create, FileAccess.Write, FileShare.None))
				using (var writer = new BinaryWriter(fileStream))
				{
					foreach (var item in Core.Current.FileManager.LoadedList)
					{
						writer.Write(item.SteamName);
						writer.Write(item.Memo);
						writer.Write(item.Header);
						writer.Write(item.M3F1YS);
						writer.Write(item.NnB);
						writer.Write(item.MHx5cg);
						writer.Write(item.Created.ToString());
					}
					fileStream.Dispose();
					fileStream.Close();
					writer.Dispose();
					writer.Close();
				}
				return true;
			}
			catch(Exception e)
			{
				Debug.WriteLine(e);
				return false;
			}
			
		}
	}
}
