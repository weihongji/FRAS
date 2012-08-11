using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class LogonUserBiz
	{
		public static bool Exist(string code, string password = "") {
			return LogonUserDao.Exist(code, password);
		}

		public static int Save(LogonUserInfo user) {
			int id;
			if (user.ID <= 0) {
				id = LogonUserDao.Add(user);
			}
			else {
				id = LogonUserDao.Update(user) ? user.ID : 0;
			}
			return id;
		}

		public static bool Delete(int ID) {
			return LogonUserDao.Delete(ID);
		}

		public static LogonUserInfo GetEntity(int ID) {
			return LogonUserDao.GetEntity(ID);
		}

		public static LogonUserInfo GetEntity(String name) {
			return LogonUserDao.GetEntity(name);
		}

		public static DataTable GetList() {
			int pageCount;
			return LogonUserDao.GetList(new SqlCondition[0], 0, 0, String.Empty, out pageCount);
		}
	}
}
