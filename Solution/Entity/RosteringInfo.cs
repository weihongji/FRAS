using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class RosteringInfo : BaseInfo
	{
		int m_ID;
		string m_Name;
		string m_StartTime;
		string m_EndTime;
		int m_EarlyRange;
		int m_LateRange;
		int m_MultipleType;
		int m_NextID;
		decimal m_Duration;
		int m_NightWork;
		bool m_Active;
		/* for read-only properties */
		string m_MultipleTypeName;
		string m_NightWorkName;
		string m_ActiveName;

		public RosteringInfo() {
			m_NightWork = 0;
		}

		public RosteringInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_Name = row["bcName"].ToString().Trim();
			m_StartTime = row["startTime"].ToString().Trim();
			m_EndTime = row["endTime"].ToString().Trim();
			m_EarlyRange = (int)row["earlyRange"];
			m_LateRange = (int)row["lateRange"];
			m_MultipleType = (int)row["multType"];
			if (row["nextID"] == DBNull.Value) {
				m_NextID = 0;
			}
			else {
				m_NextID = (int)row["nextID"];
			}
			m_Duration = (decimal)row["mulripleDur"];
			m_NightWork = (int)row["nightWork"];
			m_Active = (int)row["flag"] == 1;
			m_MultipleTypeName = row["MultipleTypeName"].ToString().Trim();
			m_NightWorkName = row["NightWorkName"].ToString().Trim();
			m_ActiveName = row["ActiveName"].ToString().Trim();
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string Name {
			get { return m_Name; }
			set { m_Name = value.Trim(); }
		}

		public string StartTime {
			get { return m_StartTime; }
			set { m_StartTime = value.Trim(); }
		}

		public string EndTime {
			get { return m_EndTime; }
			set { m_EndTime = value.Trim(); }
		}

		public int EarlyRange {
			get { return m_EarlyRange; }
			set { m_EarlyRange = value; }
		}

		public int LateRange {
			get { return m_LateRange; }
			set { m_LateRange = value; }
		}

		public string RealStartTime {
			get {
				return Convert.ToDateTime("2000/1/1 " + m_StartTime).AddMinutes((-1)*m_EarlyRange).ToString("HH:mm");
			}
		}

		public string RealEndTime {
			get {
				return Convert.ToDateTime("2000/1/1 " + m_EndTime).AddMinutes(m_LateRange).ToString("HH:mm");
			}
		}

		public bool Active {
			get { return m_Active; }
			set { m_Active = value; }
		}

		public int MultipleType {
			get { return m_MultipleType; }
			set { m_MultipleType = value; }
		}

		public int NextID {
			get { return m_NextID; }
			set { m_NextID = value; }
		}

		public decimal Duration {
			get { return m_Duration; }
			set { m_Duration = value; }
		}

		public int NightWork {
			get { return m_NightWork; }
			set { m_NightWork = value; }
		}

		public string MultipleTypeName {
			get { return m_MultipleTypeName; }
		}

		public string NightWorkName {
			get { return m_NightWorkName; }
		}

		public string ActiveName {
			get { return m_ActiveName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", name: '" + m_Name + "'");
			s.Append(", startTime: '" + m_StartTime + "'");
			s.Append(", endTime: '" + m_EndTime + "'");
			s.Append(", earlyRange: " + m_EarlyRange);
			s.Append(", lateRange: " + m_LateRange);
			s.Append(", realStartTime: '" + RealStartTime + "'");
			s.Append(", realEndTime: '" + RealEndTime + "'");
			s.Append(", multipleType: " + m_MultipleType);
			s.Append(", nextID: " + m_NextID);
			s.Append(", duration: '" + m_Duration.ToString("0.0") + "'");
			s.Append(", nightWork: " + m_NightWork);
			s.Append(", active: " + (m_Active ? "true" : "false"));
			s.Append(", multipleTypeName: '" + m_MultipleTypeName + "'");
			s.Append(", nightWorkName: '" + m_NightWorkName + "'");
			s.Append(", activeName: '" + m_ActiveName + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
