using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShadowShifter
{
	public class Deflate
	{
		#region singleton

		private static Deflate current = new Deflate();

		public static Deflate Current
		{
			get { return current; }
		}

		#endregion

		public static string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

		public void ExtractZip(string fileLocation, string ExtractLocation,bool decrypt)
		{
			if(decrypt)
			{
				var bytes = File.ReadAllBytes(fileLocation);
				byte[] key = new byte[30];
				var decrypted = Decrypt(bytes, key);
				File.WriteAllBytes(Path.Combine(MainFolder, "patch", "temp.sv"), decrypted);
				ZipFile.ExtractToDirectory(Path.Combine(MainFolder, "patch", "temp.sv"), ExtractLocation);
				File.Delete(Path.Combine(MainFolder, "patch", "temp.sv"));
			}
			else
			{
				ZipFile.ExtractToDirectory(fileLocation, ExtractLocation);
			}
		}
		public static byte[] Decrypt(byte[] toDecrypt, byte[] key)
		{
			byte[] keyArray = key;
			byte[] toEncryptArray = toDecrypt;
			RijndaelManaged rDel = new RijndaelManaged();
			rDel.Key = keyArray;
			rDel.Mode = CipherMode.ECB;
			rDel.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = rDel.CreateDecryptor();
			return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		}
		/// <summary>
		/// 폴더 복사 작업이 끝나면 sourceFolder를 삭제한다.
		/// </summary>
		/// <param name="sourceFolder">복사할 폴더가 있는 경로</param>
		/// <param name="destFolder">붙여넣기할 경로</param>
		public void CopyFolder(string sourceFolder, string destFolder)
		{
			if (!Directory.Exists(destFolder))
				Directory.CreateDirectory(destFolder);

			string[] files = Directory.GetFiles(sourceFolder);
			string[] folders = Directory.GetDirectories(sourceFolder);

			foreach (string file in files)
			{
				string name = Path.GetFileName(file);
				string dest = Path.Combine(destFolder, name);
				File.Copy(file, dest, true);

			}

			// foreach 안에서 재귀 함수를 통해서 폴더 복사 및 파일 복사 진행 완료
			foreach (string folder in folders)
			{
				string name = Path.GetFileName(folder);
				string dest = Path.Combine(destFolder, name);
				CopyFolder(folder, dest);
			}
			Directory.Delete(sourceFolder, true);
		}

	}
}
