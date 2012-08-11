using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;

namespace BLL
{
	public class WorkDurationBiz
	{
		public static DataTable GetList(string userName, int userType, int dept, int device, DateTime start, DateTime end) {
			return WorkDurationDao.GetList(userName, userType, dept, device, start, end);
		}

		public static DataTable GetWorkingUserList(int DeptID, DateTime Date) {
			return WorkDurationDao.GetWorkingUserList(DeptID, Date);
		}

		public static int GetWorkingUserCount(DateTime Date) {
			return WorkDurationDao.GetWorkingUserCount(Date);
		}

		public static DataTable GetDailyAttendance(DateTime Date) {
			return WorkDurationDao.GetDailyAttendance(Date);
		}
	}
}
