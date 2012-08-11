using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class AttendanceInfo : BaseInfo
	{
		int m_ID;
		string m_UserID;
		string m_Date;
		decimal m_Duration;
		bool m_InWell;
		int m_NightWork;
		bool m_Approved;
		/* for read-only properties */
		string m_UserName;
		string m_DeptName;
		string m_InWellName;
		string m_NightWorkName;
		string m_ApprovedName;

		public AttendanceInfo() {
			m_Approved = false;
		}

		public AttendanceInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_UserID = row["userId"].ToString().Trim();
			m_Date = row["DATE"].ToString().Trim();
			m_Duration = (decimal)row["workDurs"];
			m_InWell = (int)row["ifIn"] == 1;
			m_NightWork = (int)row["nightWork"];
			m_Approved = (int)row["state"] == 1;
			m_UserName = row["UserName"].ToString().Trim();
			m_DeptName = row["DeptName"].ToString().Trim();
			m_InWellName = row["InWellName"].ToString().Trim();
			m_NightWorkName = row["NightWorkName"].ToString().Trim();
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

		public string Date {
			get { return m_Date; }
			set { m_Date = value.Trim(); }
		}

		public decimal Duration {
			get { return m_Duration; }
			set { m_Duration = value; }
		}

		public bool InWell {
			get { return m_InWell; }
			set { m_InWell = value; }
		}

		public int NightWork {
			get { return m_NightWork; }
			set { m_NightWork = value; }
		}

		public bool Approved {
			get { return m_Approved; }
			set { m_Approved = value; }
		}

		public string UserName {
			get { return m_UserName; }
		}

		public string DeptName {
			get { return m_DeptName; }
		}

		public string InWellName {
			get { return m_InWellName; }
		}

		public string NightWorkName {
			get { return m_NightWorkName; }
		}

		public string ApprovedName {
			get { return m_ApprovedName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", userId: '" + m_UserID + "'");
			s.Append(", date: '" + m_Date + "'");
			s.Append(", duration: '" + m_Duration.ToString("0.0") + "'");
			s.Append(", inWell: " + (m_InWell ? "true" : "false"));
			s.Append(", nightWork: " + m_NightWork);
			s.Append(", approved: " + (m_Approved ? "true" : "false"));
			s.Append(", userName: '" + m_UserName + "'");
			s.Append(", deptName: '" + m_DeptName + "'");
			s.Append(", inWellName: '" + m_InWellName + "'");
			s.Append(", nightWorkName: '" + m_NightWorkName + "'");
			s.Append(", approvedName: '" + m_ApprovedName + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
