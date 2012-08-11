using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class UserBiz
	{
		public static bool Exist(string ID) {
			if (string.IsNullOrWhiteSpace(ID)) {
				return false;
			}
			else {
				return UserDao.Exist(ID.Trim());
			}
		}

		public static string Save(UserInfo user) {
			string id;
			if (!UserDao.Exist(user.ID)) {
				id = UserDao.Add(user);
			}
			else {
				id = UserDao.Update(user) ? user.ID : "0";
			}
			return id;
		}

		public static bool UpdateCard(string ID, int CopyType) {
			return UserDao.UpdateCard(ID, CopyType);
		}

		public static bool Delete(string ID) {
			if (string.IsNullOrWhiteSpace(ID)) {
				return false;
			}
			else {
				return UserDao.Delete(ID.Trim());
			}
		}

		public static UserInfo GetEntity(string ID) {
			if (string.IsNullOrWhiteSpace(ID)) {
				return null;
			}
			else {
				UserInfo user = UserDao.GetEntity(ID.Trim());
				if (user != null) {
					user.CardID = UserCardDao.GetEntityByUserID(user.ID);
				}
				return user;
			}
		}

		public static UserInfo GetEntityByName(string name) {
			if (string.IsNullOrWhiteSpace(name)) {
				return null;
			}

			List<SqlCondition> conditions = new List<SqlCondition>();
			if (!string.IsNullOrWhiteSpace(name)) {
				conditions.Add(new SqlCondition("name", name.Trim()));
			}
			DataTable list = UserDao.GetList(conditions.ToArray());
			if (list.Rows.Count > 0) {
				return GetEntity(list.Rows[0]["userId"].ToString());
			}

			return null;
		}

		public static DataTable GetList(int deptId = -1, string name = "") {
			List<SqlCondition> conditions = new List<SqlCondition>();
			if (deptId >= 0) {
				conditions.Add(new SqlCondition("deptId", deptId));
			}
			if (!string.IsNullOrWhiteSpace(name)) {
				if (Utility.StringUtility.IsNumeric(name)) {
					conditions.Add(new SqlCondition("id", name));
				}
				else {
					conditions.Add(new SqlCondition("name", name));
				}
			}
			return UserDao.GetList(conditions.ToArray());
		}

		public static DataTable GetList(string UserIDs, string SortColumn = "UserId") {
			SqlCondition[] conditions = new SqlCondition[1];
			conditions[0] = new SqlCondition("id", UserIDs.Split(','), SqlCondition.EnumConstraintType.In);
			return UserDao.GetList(conditions.ToArray(), SortColumn);
		}

		public static DataTable GetBriefList(int DeptID) {
			return UserDao.GetBriefList(DeptID);
		}

		public static DataTable GetBriefListForRostering(int DeptID) {
			return UserDao.GetBriefListForRostering(DeptID);
		}
	}
}
