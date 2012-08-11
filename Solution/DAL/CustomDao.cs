using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Entity;

namespace DAL
{
	public class CustomDao
	{
		static SqlDbHelper helper = new SqlDbHelper();

		public static bool CreateAttendance(string UserId, DateTime Date, int Rostering) {
			string date_fmt = "yyyy-MM-dd"; // Date format
			string fmt = date_fmt + " HH:mm:ss"; // Date & time format
			string dt = Date.ToString(date_fmt);
			DateTime[,] arrTime = new DateTime[30, 2];
			arrTime[0, 0] = Convert.ToDateTime(dt + " 08:50:32");
			arrTime[0, 1] = Convert.ToDateTime(dt + " 08:50:32");
			arrTime[1, 0] = Convert.ToDateTime(dt + " 11:50:25");
			arrTime[1, 1] = Convert.ToDateTime(dt + " 11:50:25");
			arrTime[2, 0] = Convert.ToDateTime(dt + " 08:48:32");
			arrTime[2, 1] = Convert.ToDateTime(dt + " 11:52:25");
			arrTime[3, 0] = Convert.ToDateTime(dt + " 08:42:00");
			arrTime[3, 1] = Convert.ToDateTime(dt + " 08:42:00");
			arrTime[4, 0] = Convert.ToDateTime(dt + " 08:42:00");
			arrTime[4, 1] = Convert.ToDateTime(dt + " 08:42:00");
			arrTime[5, 0] = Convert.ToDateTime(dt + " 08:42:30");
			arrTime[5, 1] = Convert.ToDateTime(dt + " 08:42:30");
			arrTime[6, 0] = Convert.ToDateTime(dt + " 08:45:38");
			arrTime[6, 1] = Convert.ToDateTime(dt + " 08:45:38");
			arrTime[7, 0] = Convert.ToDateTime(dt + " 08:48:22");
			arrTime[7, 1] = Convert.ToDateTime(dt + " 08:48:22");
			arrTime[8, 0] = Convert.ToDateTime(dt + " 08:50:07");
			arrTime[8, 1] = Convert.ToDateTime(dt + " 08:50:07");
			arrTime[9, 0] = Convert.ToDateTime(dt + " 08:50:43");
			arrTime[9, 1] = Convert.ToDateTime(dt + " 08:50:44");
			arrTime[10, 0] = Convert.ToDateTime(dt + " 08:51:20");
			arrTime[10, 1] = Convert.ToDateTime(dt + " 08:51:20");
			arrTime[11, 0] = Convert.ToDateTime(dt + " 08:51:31");
			arrTime[11, 1] = Convert.ToDateTime(dt + " 08:51:31");
			arrTime[12, 0] = Convert.ToDateTime(dt + " 09:12:22");
			arrTime[12, 1] = Convert.ToDateTime(dt + " 09:12:22");
			arrTime[13, 0] = Convert.ToDateTime(dt + " 09:13:43");
			arrTime[13, 1] = Convert.ToDateTime(dt + " 09:13:43");
			arrTime[14, 0] = Convert.ToDateTime(dt + " 09:16:21");
			arrTime[14, 1] = Convert.ToDateTime(dt + " 09:16:21");
			arrTime[15, 0] = Convert.ToDateTime(dt + " 09:16:33");
			arrTime[15, 1] = Convert.ToDateTime(dt + " 09:16:33");
			arrTime[16, 0] = Convert.ToDateTime(dt + " 09:45:15");
			arrTime[16, 1] = Convert.ToDateTime(dt + " 09:45:15");
			arrTime[17, 0] = Convert.ToDateTime(dt + " 10:13:15");
			arrTime[17, 1] = Convert.ToDateTime(dt + " 10:17:04");
			arrTime[18, 0] = Convert.ToDateTime(dt + " 10:18:41");
			arrTime[18, 1] = Convert.ToDateTime(dt + " 10:43:38");
			arrTime[19, 0] = Convert.ToDateTime(dt + " 10:44:02");
			arrTime[19, 1] = Convert.ToDateTime(dt + " 10:44:03");
			arrTime[20, 0] = Convert.ToDateTime(dt + " 10:46:12");
			arrTime[20, 1] = Convert.ToDateTime(dt + " 10:46:13");
			arrTime[21, 0] = Convert.ToDateTime(dt + " 10:47:26");
			arrTime[21, 1] = Convert.ToDateTime(dt + " 10:47:26");
			arrTime[22, 0] = Convert.ToDateTime(dt + " 10:47:59");
			arrTime[22, 1] = Convert.ToDateTime(dt + " 10:47:59");
			arrTime[23, 0] = Convert.ToDateTime(dt + " 10:49:18");
			arrTime[23, 1] = Convert.ToDateTime(dt + " 10:49:19");
			arrTime[24, 0] = Convert.ToDateTime(dt + " 10:53:24");
			arrTime[24, 1] = Convert.ToDateTime(dt + " 10:53:25");
			arrTime[25, 0] = Convert.ToDateTime(dt + " 10:54:03");
			arrTime[25, 1] = Convert.ToDateTime(dt + " 10:54:03");
			arrTime[26, 0] = Convert.ToDateTime(dt + " 10:55:23");
			arrTime[26, 1] = Convert.ToDateTime(dt + " 10:55:23");
			arrTime[27, 0] = Convert.ToDateTime(dt + " 10:56:32");
			arrTime[27, 1] = Convert.ToDateTime(dt + " 10:56:32");
			arrTime[28, 0] = Convert.ToDateTime(dt + " 10:57:46");
			arrTime[28, 1] = Convert.ToDateTime(dt + " 10:57:46");
			arrTime[29, 0] = Convert.ToDateTime(dt + " 10:58:00");
			arrTime[29, 1] = Convert.ToDateTime(dt + " 10:58:00");

			int offset = 0;
			if (Rostering == 0) { // 零点班
				offset = -6;
			}
			else if (Rostering == 2) { // 四点班
				offset = 8;
			}
			if (offset != 0) {
				for (int i = 0; i < 30; i++) {
					arrTime[i, 0] = arrTime[i, 0].AddHours(offset);
					arrTime[i, 1] = arrTime[i, 1].AddHours(offset);
				}
			}

			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@UserId", SqlDbType.VarChar, 6);
			parameters[0].Value = UserId;
			parameters[1] = new SqlParameter("@Date", SqlDbType.VarChar, 10);
			parameters[1].Value = Date.ToString(date_fmt);

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("IF NOT EXISTS(SELECT * FROM AccessLog WHERE userId = @UserId AND date = @Date) BEGIN");
			sql.AppendLine("	INSERT INTO AccessLog (userId, datetime, date, accessFlag, recResult, state, devNum, recPhotoPath)");
			sql.AppendLine("	VALUES(@UserId, '" + arrTime[0, 0].ToString(fmt) + "', @Date, 0, 1, 0, 4, 'D:\\software\\服务端程序20120602\\SavePic\\2012-06-04\\4\\480_VPhoto_20126485032.jpg')");
			sql.AppendLine("");
			sql.AppendLine("	INSERT INTO AccessLog (userId, datetime, date, accessFlag, recResult, state, devNum, recPhotoPath)");
			sql.AppendLine("	VALUES(@UserId, '" + arrTime[1, 0].ToString(fmt) + "', @Date, 0, 1, 0, 4, 'D:\\software\\服务端程序20120602\\SavePic\\2012-06-04\\4\\480_VPhoto_201264115025.jpg')");
			sql.AppendLine("");
			sql.AppendLine("	INSERT INTO WorkDuration VALUES (@UserId, @Date, 0, '3:0:5', '9184', 4, '1.0', 0, 0, '" + arrTime[2, 0].ToString(fmt) + "',  '" + arrTime[2, 1].ToString(fmt) + "', 17)");
			sql.AppendLine("END");

			bool success = (int)helper.ExecuteNonQuery(sql.ToString(), parameters) > 0;
			if (success) {
				SqlDbHelper helper2 = new SqlDbHelper(ConfigurationManager.ConnectionStrings["conn0480"].ConnectionString);
				sql.Clear();
				sql.AppendLine("insert into RegionReport values(898,1,5,20,'" + arrTime[3, 0].ToString(fmt) + "','" + arrTime[3, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,1,2,17,'" + arrTime[4, 0].ToString(fmt) + "','" + arrTime[4, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,1,4,19,'" + arrTime[5, 0].ToString(fmt) + "','" + arrTime[5, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,1,2,22,'" + arrTime[6, 0].ToString(fmt) + "','" + arrTime[6, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,2,4,24,'" + arrTime[7, 0].ToString(fmt) + "','" + arrTime[7, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,2,4,28,'" + arrTime[8, 0].ToString(fmt) + "','" + arrTime[8, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,5,29,'" + arrTime[9, 0].ToString(fmt) + "','" + arrTime[9, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,4,30,'" + arrTime[10, 0].ToString(fmt) + "','" + arrTime[10, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,4,2,37,'" + arrTime[11, 0].ToString(fmt) + "','" + arrTime[11, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,6,4,30,'" + arrTime[12, 0].ToString(fmt) + "','" + arrTime[12, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,4,3,49,'" + arrTime[13, 0].ToString(fmt) + "','" + arrTime[13, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,8,2,51,'" + arrTime[14, 0].ToString(fmt) + "','" + arrTime[14, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,9,3,52,'" + arrTime[15, 0].ToString(fmt) + "','" + arrTime[15, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,9,2,54,'" + arrTime[16, 0].ToString(fmt) + "','" + arrTime[16, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,11,1,53,'" + arrTime[17, 0].ToString(fmt) + "','" + arrTime[17, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,11,3,52,'" + arrTime[18, 0].ToString(fmt) + "','" + arrTime[18, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,9,2,51,'" + arrTime[19, 0].ToString(fmt) + "','" + arrTime[19, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,8,3,49,'" + arrTime[20, 0].ToString(fmt) + "','" + arrTime[20, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,4,4,30,'" + arrTime[21, 0].ToString(fmt) + "','" + arrTime[21, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,5,29,'" + arrTime[22, 0].ToString(fmt) + "','" + arrTime[22, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,2,26,'" + arrTime[23, 0].ToString(fmt) + "','" + arrTime[23, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,2,26,'" + arrTime[24, 0].ToString(fmt) + "','" + arrTime[24, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,4,28,'" + arrTime[25, 0].ToString(fmt) + "','" + arrTime[25, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,2,4,24,'" + arrTime[26, 0].ToString(fmt) + "','" + arrTime[26, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,3,2,22,'" + arrTime[27, 0].ToString(fmt) + "','" + arrTime[27, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,1,4,19,'" + arrTime[28, 0].ToString(fmt) + "','" + arrTime[28, 1].ToString(fmt) + "')");
				sql.AppendLine("insert into RegionReport values(898,1,5,20,'" + arrTime[29, 0].ToString(fmt) + "','" + arrTime[29, 1].ToString(fmt) + "')");

				success = (int)helper2.ExecuteNonQuery(sql.ToString()) > 0;
			}
			return success;
		}
	}
}
