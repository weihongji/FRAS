using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
	public class WorkDurationDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static DataTable GetList(string userName, int userType, int dept, int device, DateTime start, DateTime end) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT * FROM WorkDurationView");
			sql.AppendLine("WHERE date BETWEEN '" + start.ToString("yyyy-MM-dd") + "' AND '" + end.ToString("yyyy-MM-dd") + "'");
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
			sql.AppendLine("ORDER BY deptId, userid, date, bak2");
			return helper.ExecuteDataTable(sql.ToString());
		}

		public static DataTable GetWorkingUserList(int DeptID, DateTime Date) {
			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@DeptId", DeptID);
			parameters[1] = new SqlParameter("@Date", Date);
			return helper.ExecuteDataTable("spGetWorkingUser", CommandType.StoredProcedure, parameters);
		}

		public static int GetWorkingUserCount(DateTime Date) {
			StringBuilder sql = new StringBuilder();
			sql.AppendLine("SELECT COUNT(*) FROM UserInfo U WITH (NOLOCK) INNER JOIN WorkDuration W WITH (NOLOCK) ON U.userId = W.userId");
			sql.AppendLine("WHERE W.duration=0 AND W.date='" + Date.ToString("yyyy-MM-dd") + "'");
			int count = (int) helper.ExecuteScalar(sql.ToString());
			return count;
		}

		public static DataTable GetDailyAttendance(DateTime Date) {
			SqlParameter[] parameters = new SqlParameter[1];
				parameters[0] = new SqlParameter("@Date", SqlDbType.DateTime);
				parameters[0].Value = Date;
			return helper.ExecuteDataTable("spDailyAttendance", CommandType.StoredProcedure, parameters);
		}
	}
}
