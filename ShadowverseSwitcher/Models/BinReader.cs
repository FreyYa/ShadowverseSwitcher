using ShadowServant.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.Models
{
	public class BinReader
	{
		private string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
		//sort : http://bravochoi.tistory.com/entry/ListT-Sorting%ED%95%98%EA%B8%B0
		public bool ReadBin()
		{
			if (!Directory.Exists(Path.Combine(MainFolder, "Data")))
				Directory.CreateDirectory(Path.Combine(MainFolder, "Data"));

			var binPath = Path.Combine(Path.Combine(MainFolder, "Data", "AccountInfo"));
			if (File.Exists(binPath))
			{
				try
				{
					var bytes = File.ReadAllBytes(binPath);

					using (var memoryStream = new MemoryStream(bytes))
					using (var reader = new BinaryReader(memoryStream))
					{
						while (memoryStream.Position < memoryStream.Length)
						{
							Account temp = new Account();
							temp.SteamName = reader.ReadString();
							temp.Memo = reader.ReadString();
							var header = reader.ReadBytes(12);
							temp.M3F1YS = reader.ReadBytes(BitConverter.ToInt32(header, 0));
							temp.NnB = reader.ReadBytes(BitConverter.ToInt32(header, 4));
							temp.MHx5cg = reader.ReadBytes(BitConverter.ToInt32(header, 8));
							temp.Created = DateTime.Parse(reader.ReadString());
							Core.Current.FileManager.LoadedList.Add(temp);
						}
					}
					return true;
				}
				catch
				{
					return false;
				}
			}
			else { return false; }

		}
	}
}
