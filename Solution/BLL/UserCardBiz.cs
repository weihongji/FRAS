using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class UserCardBiz
	{
		public static int Add(UserCardInfo card) {
			return UserCardDao.Add(card);;
		}

		public static bool Update(UserCardInfo card) {
			return UserCardDao.Update(card); ;
		}

		public static bool Save(UserCardInfo card) {
			if (card == null || String.IsNullOrEmpty(card.UserID)) {
				return false;
			}
			bool success = UserCardDao.Update(card);
			if (!success) {
				int id = UserCardDao.Add(card);
				success = id > 0;
			}
			return success;
		}

		public static UserCardInfo GetEntityByUserID(string UserID) {
			return UserCardDao.GetEntityByUserID(UserID);
		}
	}
}
