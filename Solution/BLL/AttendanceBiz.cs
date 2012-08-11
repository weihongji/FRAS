using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class AttendanceBiz
	{
		public static int Save(AttendanceInfo attendance, bool checkDuplication = true) {
			int id;
			if (attendance.ID <= 0) {
				id = AttendanceDao.Add(attendance, checkDuplication);
			}
			else {
				id = AttendanceDao.Update(attendance) ? attendance.ID : 0;
			}
			return id;
		}

		public static bool BatchAdd(string UserIDs, string date, decimal Duration, bool InWell, int NightWork, out string Message) {
			bool result = AttendanceDao.BatchAdd(UserIDs, date, Duration, InWell, NightWork, out Message);

			if (!string.IsNullOrEmpty(Message)) {
				DataTable table = UserBiz.GetList(Message, "UserName");
				StringBuilder userNames = new StringBuilder();
				foreach (DataRow row in table.Rows) {
					if (userNames.Length > 0) { userNames.Append(", "); }
					userNames.Append(row["userName"].ToString().Trim());
				}
				Message = userNames.Length > 0 ? userNames.ToString() : "";
			}

			return result;
		}

		public static bool Delete(int ID) {
			return AttendanceDao.Delete(ID);
		}

		public static bool Approve(int ID) {
			return AttendanceDao.Approve(ID);
		}

		public static int Import(string filePath, out string Message) {
			int result = AttendanceDao.Import(filePath, out Message);

			if (!string.IsNullOrEmpty(Message)) {
				DataTable table = UserBiz.GetList(Message, "UserName");
				StringBuilder userNames = new StringBuilder();
				foreach (DataRow row in table.Rows) {
					if (userNames.Length > 0) { userNames.Append(", "); }
					userNames.Append(row["userName"].ToString().Trim());
				}
				Message = userNames.Length > 0 ? userNames.ToString() : "";
			}

			return result;
		}

		public static AttendanceInfo GetEntity(int ID) {
			return AttendanceDao.GetEntity(ID);
		}

		/// <summary>
		/// Get a list of attendance records that match specified conditions
		/// </summary>
		/// <param name="deptId"></param>
		/// <param name="userId"></param>
		/// <param name="approvedFlag">
		/// 0 = Not approved
		/// 1 = Approved
		/// -1 = ALL</param>
		/// <returns></returns>
		public static DataTable GetList(int deptId = -1, string userId = "", int approvedFlag = -1) {
			List<SqlCondition> conditions = new List<SqlCondition>();
			if (!string.IsNullOrWhiteSpace(userId) && Utility.StringUtility.IsNumeric(userId) && Convert.ToInt32(userId) > 0) {
				conditions.Add(new SqlCondition("userId", userId));
			}
			else if (deptId >= 0) {
				conditions.Add(new SqlCondition("deptId", deptId));
			}
			if (approvedFlag >= 0) {
				conditions.Add(new SqlCondition("state", approvedFlag));
			}
			return AttendanceDao.GetList(conditions.ToArray());
		}

		public static DataTable GetReportList(int dept, int device, int userType, DateTime start, DateTime end) {
			return AttendanceDao.GetReportList(dept, device, userType, start, end, 1900, 1);
		}

		public static DataTable GetReportList(int dept, int device, int userType, int Year, int Month) {
			return AttendanceDao.GetReportList(dept, device, userType, new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), Year, Month);
		}
	}
}
