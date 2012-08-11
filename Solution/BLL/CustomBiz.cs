using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class CustomBiz
	{
		public static bool CreateAttendance(string UserId, DateTime Date, int Rostering) {
			return CustomDao.CreateAttendance(UserId, Date, Rostering);
		}
	}
}
