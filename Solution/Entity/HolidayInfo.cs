using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utility;

namespace Entity
{
	public class HolidayInfo : BaseInfo
	{
		int m_ID;
		string m_Name;
		string m_StartDate;
		string m_EndDate;
		bool m_Active;
		/* for read-only properties */
		string m_ActiveName;

		public HolidayInfo() {
		}

		public HolidayInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_Name = row["holidayName"].ToString().Trim();
			m_StartDate = row["startDate"].ToString().Trim();
			m_EndDate = row["endDate"].ToString().Trim();
			m_Active = (int)row["flag"] == 1;
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

		public string StartDate {
			get { return m_StartDate; }
			set { m_StartDate = value.Trim(); }
		}

		public string EndDate {
			get { return m_EndDate; }
			set { m_EndDate = value.Trim(); }
		}

		public bool Active {
			get { return m_Active; }
			set { m_Active = value; }
		}

		public int Days {
			get {
				return DateUtility.DateDiff(DateUtility.DateInterval.Day, Convert.ToDateTime(m_StartDate), Convert.ToDateTime(m_EndDate)) + 1;
			}
		}
		
		public string ActiveName {
			get { return m_ActiveName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", name: '" + m_Name + "'");
			s.Append(", startDate: '" + m_StartDate + "'");
			s.Append(", endDate: '" + m_EndDate + "'");
			s.Append(", active: " + (m_Active ? "true" : "false"));
			s.Append(", days: " + Days);
			s.Append(", activeName: '" + m_ActiveName + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
