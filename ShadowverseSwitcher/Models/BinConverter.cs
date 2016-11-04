using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowverseSwitcher.Models
{
	public class BinConverter
	{
		//#region singleton

		//private static BinConverter current = new BinConverter();

		//public static BinConverter Current
		//{
		//	get { return current; }
		//}

		//#endregion
		public BinConverter()
		{
			//current = this;
		}
		string MainFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "Data");
		byte[] utf8Bom = { 0xEF, 0xBB, 0xBF };
		public void BinToCsv()
		{
			ModulesCore.Current.BinReader.ReadBin();
			foreach (var items in ModulesCore.Current.BinReader.Lists.ToList())
			{
				var FileName = items.Key;
				var csvPath = Path.Combine(MainFolder, FileName + ".csv");
				using (var fileStream = new FileStream(csvPath, FileMode.Create, FileAccess.Write, FileShare.None))
				using (var writer = new BinaryWriter(fileStream))
				{
					writer.Write(utf8Bom);
				}
				foreach (var item in items.Value)
				{
					var result = item.Date.ToString() + "," + item.PageCount.ToString();
					using (StreamWriter w = File.AppendText(csvPath))
					{
						w.WriteLine(result);
					}
				}
			}
		}
		public void CsvToBin()
		{
			CsvReader.Current.ReadCsv();
		}
	}
}
