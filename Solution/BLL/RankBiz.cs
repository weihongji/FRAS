using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class RankBiz
	{
		public static bool Exist(int id, string name) {
			return RankDao.Exist(id, name);
		}

		public static int Save(RankInfo rank) {
			int id;
			if (rank.ID <= 0) {
				id = RankDao.Add(rank);
			}
			else {
				id = RankDao.Update(rank) ? rank.ID : 0;
			}
			return id;
		}

		public static bool Delete(int ID) {
			return RankDao.Delete(ID);
		}

		public static RankInfo GetEntity(int ID) {
			return RankDao.GetEntity(ID);
		}

		public static DataTable GetList() {
			return RankDao.GetList();
		}

		public static DataTable GetBriefList() {
			return RankDao.GetBriefList();
		}
	}
}
