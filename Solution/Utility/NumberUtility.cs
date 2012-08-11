using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;

namespace Utility
{
	public class NumberUtility
	{
		/// <summary>
		/// 将数字按货币形式显示
		/// </summary>
		/// <param name="val"></param>
		/// <returns></returns>
		static public string FormatMoney(Object val) {
			if (val == DBNull.Value || val == null) {
				return "";
			}
			else {
				return (((Decimal)val)).ToString("#,0.00");
			}
		}
	}
}
