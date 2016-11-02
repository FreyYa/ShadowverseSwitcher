using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowServant.Models
{
	public class Account
	{
		/// <summary>
		/// MHx5cg==_h786395497
		/// </summary>
		public byte[] MHx5cg { get; set; }
		/// <summary>
		/// M3F1YSNkOnF0_h4073495316
		/// </summary>
		public byte[] M3F1YS { get; set; }
		/// <summary>
		/// NnB/ZDJpMHx5cg==_h354593472
		/// </summary>
		public byte[] NnB { get; set; }
		/// <summary>
		/// 계정명, 혹은 메모
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 세이브 데이터 생성일시
		/// </summary>
		public DateTime Created { get; set; }
	}
}
