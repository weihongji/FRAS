using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class LeaveBiz
	{
		public static int Save(LeaveInfo leave) {
			int id;
			if (leave.ID <= 0) {
				id = LeaveDao.Add(leave);
			}
			else {
				id = LeaveDao.Update(leave) ? leave.ID : 0;
			}
			return id;
		}

		public static bool Delete(int ID) {
			return LeaveDao.Delete(ID);
		}

		public static bool Approve(int ID) {
			return LeaveDao.Approve(ID);
		}

		public static LeaveInfo GetEntity(int ID) {
			return LeaveDao.GetEntity(ID);
		}

		/// <summary>
		/// Get a list of leaves that match specified conditions
		/// </summary>
		/// <param name="deptId"></param>
		/// <param name="userId"></param>
		/// <param name="approvedFlag">
		/// 0 = Not approved
		/// 1 = Approved
		/// -1 = ALL</param>
		/// <returns></returns>
		public static DataTable GetList(int deptId = -1, string userId = "", int approvedFlag = -1) {
			List<SqlCondition> conditions = new List<SqlCondition>();
			if (!string.IsNullOrWhiteSpace(userId) && Utility.StringUtility.IsNumeric(userId) && Convert.ToInt32(userId)>0) {
				conditions.Add(new SqlCondition("userId", userId));
			}
			else if (deptId >= 0) {
				conditions.Add(new SqlCondition("deptId", deptId));
			}
			if (approvedFlag >= 0) {
				conditions.Add(new SqlCondition("flag", approvedFlag));
			}
			return LeaveDao.GetList(conditions.ToArray());
		}
	}
}
