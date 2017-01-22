using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowShifter
{
	public class Patcher
	{
		public static string MainFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

		public static string ErrorMsg { get; set; }
		public static Deflate Deflate { get; private set; }

		public Patcher()
		{
			ErrorMsg = "";
		}
		/// <summary>
		/// 언어팩을 적용합니다
		/// </summary>
		/// <param name="TargetPath">게임이 설치되어있는 경로</param>
		public static bool PatchLanguage(string TargetPath)
		{
			var CyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low", "Cygames");
			string file_name = string.Empty;

			if (!Directory.Exists(Path.Combine(CyPath, "Shadowverse")))
			{
				ErrorMsg = "패치 대상 폴더가 존재하지않습니다";
				return false;
			}

			if (File.Exists(Path.Combine(MainFolder, "Patch", "ko-kr.sw")))
				file_name = Path.Combine(MainFolder, "Patch", "ko-kr.sw");
			else file_name = string.Empty;

			if (file_name != string.Empty)
			{
				var patch_temp_path = Path.Combine(MainFolder, "Patch", "temp");

				if (!Directory.Exists(patch_temp_path))
					Directory.CreateDirectory(patch_temp_path);

				Deflate.Current.ExtractZip(file_name, patch_temp_path,true);
				Console.WriteLine("압축해제 완료");

				Deflate.Current.CopyFolder(Path.Combine(MainFolder, "Patch", "temp", "Shadowverse"), Path.Combine(CyPath, "Shadowverse"));

				var datapath = Path.Combine(TargetPath, "Shadowverse_Data");
				if (!Directory.Exists(datapath))
				{
					ErrorMsg = "패치 대상 폴더가 존재하지않습니다";
					return false;
				}

				File.Copy(Path.Combine(MainFolder, "Patch", "temp","Steam", "resources.assets"), Path.Combine(datapath, "resources.assets"), true);
				Deflate.Current.CopyFolder(Path.Combine(MainFolder, "Patch", "temp", "Steam", "StreamingAssets"), Path.Combine(datapath));

				if (Directory.Exists(patch_temp_path))
					Directory.Delete(patch_temp_path, true);
			}
			else
			{
				ErrorMsg = "패치 파일이 없습니다";
				return false;
			}

			return true;
		}
		/// <summary>
		/// 원본 원어팩을 재적용합니다.
		/// </summary>
		/// <param name="TargetPath">게임이 설치되어있는 경로</param>
		public static bool RollbackLanguage(string TargetPath)
		{
			var CyPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low", "Cygames");

			if (!Directory.Exists(Path.Combine(CyPath, "Shadowverse")))
			{
				ErrorMsg = "롤백 대상 폴더가 존재하지않습니다";
				return false;
			}
			var file_name = string.Empty;

			if (File.Exists(Path.Combine(MainFolder, "Patch", "ko-kr.sw")))
				file_name = Path.Combine(MainFolder, "Patch", "ko-kr.sw");
			else file_name = string.Empty;

			if (file_name != string.Empty)
			{
				var patch_temp_path = Path.Combine(MainFolder, "Patch", "temp");

				if (!Directory.Exists(patch_temp_path))
					Directory.CreateDirectory(patch_temp_path);

				Deflate.Current.ExtractZip(file_name, patch_temp_path,true);

				//패치 파일을 삭제
				if (Directory.Exists(Path.Combine(patch_temp_path, "Shadowverse")))
					Directory.Delete(Path.Combine(patch_temp_path, "Shadowverse"), true);

				if (Directory.Exists(Path.Combine(patch_temp_path, "Steam")))
					Directory.Delete(Path.Combine(patch_temp_path, "Steam"),true);

				Deflate.Current.ExtractZip(Path.Combine(MainFolder, "Patch", "temp", "Shadowverse_jp_origin.zip"), patch_temp_path,false);
				Console.WriteLine("압축해제 완료");

				Deflate.Current.CopyFolder(Path.Combine(MainFolder, "Patch", "temp", "Shadowverse"), Path.Combine(CyPath, "Shadowverse"));

				var datapath = Path.Combine(TargetPath, "Shadowverse_Data");
				if (!Directory.Exists(datapath))
				{
					ErrorMsg = "패치 대상 폴더가 존재하지않습니다";
					return false;
				}

				File.Copy(Path.Combine(MainFolder, "Patch", "temp","Steam", "resources.assets"), Path.Combine(datapath, "resources.assets"), true);
				Deflate.Current.CopyFolder(Path.Combine(MainFolder, "Patch", "temp", "Steam", "StreamingAssets"), Path.Combine(datapath));

				if (Directory.Exists(patch_temp_path))
					Directory.Delete(patch_temp_path, true);
			}
			else
			{
				ErrorMsg = "패치 파일이 없습니다";
				return false;
			}

			return true;
		}
		/// <summary>
		/// dll파일을 바꿔치기하여 영/일 스위칭을 합니다.
		/// </summary>
		/// <param name="TargetPath"></param>
		/// <param name="TargetLanguage">타겟 언어를 설정합니다</param>
		public static bool SwitchingLanguage(string TargetPath, bool IsEnglish)
		{
			var dllpath = Path.Combine(MainFolder, "dll");
			var jppath = Path.Combine(MainFolder, "dll", "jp");
			var enpath = Path.Combine(MainFolder, "dll", "en");
			var datapath = Path.Combine(TargetPath, "Shadowverse_Data", "Managed");

			string path = enpath;

			if (!Directory.Exists(dllpath) ||
				!Directory.Exists(jppath) ||
				!Directory.Exists(enpath))
			{
				ErrorMsg = "언어 교체 파일이 없습니다";
				return false;
			}

			if (!IsEnglish) path = jppath;

			if (!File.Exists(Path.Combine(path, "Assembly-CSharp.dll")) ||
				!File.Exists(Path.Combine(path, "Assembly-CSharp-firstpass.dll")))
			{
				ErrorMsg = "언어 교체 파일이 없습니다";
				return false;
			}

			File.Copy(Path.Combine(path, "Assembly-CSharp.dll"), Path.Combine(datapath, "Assembly-CSharp.dll"), true);
			File.Copy(Path.Combine(path, "Assembly-CSharp-firstpass.dll"), Path.Combine(datapath, "Assembly-CSharp-firstpass.dll"), true);

			return true;
		}
	}
}
