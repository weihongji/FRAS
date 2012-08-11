using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class HolidayBiz
	{
		public static int Save(HolidayInfo holiday) {
			int id;
			if (holiday.ID <= 0) {
				id = HolidayDao.Add(holiday);
			}
			else {
				id = HolidayDao.Update(holiday) ? holiday.ID : 0;
			}
			return id;
		}

		public static bool Delete(int ID) {
			return HolidayDao.Delete(ID);
		}

		public static HolidayInfo GetEntity(int ID) {
			return HolidayDao.GetEntity(ID);
		}

		public static DataTable GetList() {
			return HolidayDao.GetList();
		}
	}
}
