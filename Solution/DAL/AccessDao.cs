using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
	public class AccessDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static DataTable GetList(string userName, int userType, int dept, int device, int state, DateTime start, DateTime end) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT * FROM AccessLogView");
			sql.AppendLine("WHERE recResult=1");
			sql.AppendLine("	AND date BETWEEN '" + start.ToString("yyyy-MM-dd") + "' AND '" + end.ToString("yyyy-MM-dd") + "'");
			if (userName != null && userName.Trim().Length > 0) {
				sql.AppendLine("	AND UserName like '%" + userName.Replace("'", "''").Trim() + "%'");
			}
			if (userType >= 0) {
				sql.AppendLine("	AND UserType = " + userType.ToString());
			}
			if (dept >= 0) {
				sql.AppendLine("	AND deptid = " + dept.ToString());
			}
			if (device > 0) {
				sql.AppendLine("	AND devNum = " + device.ToString());
			}
			if (state >= 0) {
				sql.AppendLine("	AND state = " + state.ToString());
			}
			sql.AppendLine("ORDER BY userid, datetime");
			return helper.ExecuteDataTable(sql.ToString());
		}
	}
}
