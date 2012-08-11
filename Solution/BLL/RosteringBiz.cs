using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class RosteringBiz
	{
		public static int Save(RosteringInfo rostering, RosteringInfo rostering2 = null) {
			int id;
			if (rostering.ID <= 0) {
				id = RosteringDao.Add(rostering, rostering2);
			}
			else {
				id = RosteringDao.Update(rostering, rostering2) ? rostering.ID : 0;
			}
			return id;
		}

		public static bool Delete(int ID) {
			return RosteringDao.Delete(ID);
		}

		public static RosteringInfo GetEntity(int ID) {
			return RosteringDao.GetEntity(ID);
		}

		public static DataTable GetList() {
			return RosteringDao.GetList();
		}

		public static DataTable GetPreRostering(string UserId) {
			return RosteringDao.GetPreRostering(UserId);
		}

		public static bool SavePreRostering(string UserIDs, int RosteringId, int Start, int End) {
			return RosteringDao.SavePreRostering(UserIDs, RosteringId, Start, End);
		}
	}
}
