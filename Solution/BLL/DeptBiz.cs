using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class DeptBiz
	{
		public static bool Exist(int id) {
			return DeptDao.Exist(id);
		}

		public static bool Exist(int id, string name) {
			return DeptDao.Exist(id, name);
		}

		public static bool Save(DeptInfo dept) {
			bool success = false;
			if (DeptDao.Exist(dept.ID)) {
				success = DeptDao.Update(dept);
			}
			else {
				success = DeptDao.Add(dept);
			}
			return success;
		}

		public static bool Delete(int ID) {
			return DeptDao.Delete(ID);
		}

		public static DeptInfo GetEntity(int ID) {
			return DeptDao.GetEntity(ID);
		}

		public static DataTable GetList() {
			return DeptDao.GetList();
		}

		public static DataTable GetBriefList(int ID = -1) {
			return DeptDao.GetBriefList(ID);
		}
	}
}
