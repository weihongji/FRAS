using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DAL;

namespace BLL
{
	public class AccessBiz
	{
		public static DataTable GetList(string userName, int userType, int dept, int device, int state, DateTime start, DateTime end) {
			return AccessDao.GetList(userName, userType, dept, device, state, start, end);
		}
	}
}
