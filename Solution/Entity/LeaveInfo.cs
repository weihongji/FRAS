using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utility;

namespace Entity
{
	public class LeaveInfo : BaseInfo
	{
		int m_ID;
		string m_UserID;
		string m_StartDate;
		string m_EndDate;
		int m_Type;
		bool m_Approved;
		/* for read-only properties */
		string m_UserName;
		string m_TypeName;
		string m_ApprovedName;

		public LeaveInfo() {
			m_Approved = false;
		}

		public LeaveInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_UserID = row["userId"].ToString().Trim();
			m_StartDate = row["startDate"].ToString().Trim();
			m_EndDate = row["endDate"].ToString().Trim();
			m_Type = (int)row["type"];
			m_Approved = (int)row["flag"] == 1;
			m_UserName = row["UserName"].ToString().Trim();
			m_TypeName = row["TypeName"].ToString().Trim();
			m_ApprovedName = row["ApprovedName"].ToString().Trim();
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string UserID {
			get { return m_UserID; }
			set { m_UserID = value.Trim(); }
		}

		public string StartDate {
			get { return m_StartDate; }
			set { m_StartDate = value.Trim(); }
		}

		public string EndDate {
			get { return m_EndDate; }
			set { m_EndDate = value.Trim(); }
		}

		public int Days {
			get {
				return DateUtility.DateDiff(DateUtility.DateInterval.Day, Convert.ToDateTime(m_StartDate), Convert.ToDateTime(m_EndDate)) + 1;
			}
		}

		public int Type {
			get { return m_Type; }
			set { m_Type = value; }
		}

		public bool Approved {
			get { return m_Approved; }
			set { m_Approved = value; }
		}

		public string UserName {
			get { return m_UserName; }
		}

		public string TypeName {
			get { return m_TypeName; }
		}

		public string ApprovedName {
			get { return m_ApprovedName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", userId: '" + m_UserID + "'");
			s.Append(", startDate: '" + m_StartDate + "'");
			s.Append(", endDate: '" + m_EndDate + "'");
			s.Append(", type: " + m_Type);
			s.Append(", approved: " + (m_Approved ? "true" : "false"));
			s.Append(", days: " + Days);
			s.Append(", userName: '" + m_UserName + "'");
			s.Append(", typeName: '" + m_TypeName + "'");
			s.Append(", approvedName: '" + m_ApprovedName + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
