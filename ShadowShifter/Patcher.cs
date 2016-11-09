using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowShifter
{
    public class Patcher
    {
		/// <summary>
		/// 언어팩을 적용합니다
		/// </summary>
		/// <param name="TargetPath">게임이 설치되어있는 경로</param>
		public static bool PatchLanguage(string TargetPath)
		{
			return true;
		}
		/// <summary>
		/// 원본 원어팩을 재적용합니다.
		/// </summary>
		/// <param name="TargetPath">게임이 설치되어있는 경로</param>
		public static bool RollbackLanguage(string TargetPath)
		{
			return true;
		}
		/// <summary>
		/// dll파일을 바꿔치기하여 영/일 스위칭을 합니다.
		/// </summary>
		/// <param name="TargetPath"></param>
		/// <param name="TargetLanguage">타겟 언어를 설정합니다</param>
		public static bool SwitchingLanguage(string TargetPath,bool IsEnglish)
		{
			return true;
		}
    }
}
