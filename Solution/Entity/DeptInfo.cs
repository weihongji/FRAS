using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class DeptInfo : BaseInfo
	{
		int m_ID;
		string m_Name;
		int m_UpID;
		int m_Level;

		public DeptInfo() {
			m_UpID = 0;
			m_Level = 1;
		}

		public DeptInfo(DataRow row) {
			m_ID = (int)row["deptId"];
			m_Name = row["deptName"].ToString().Trim();
			m_UpID = (int)row["upID"];
			m_Level = (int)row["level"];
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string Name {
			get { return m_Name; }
			set { m_Name = value.Trim(); }
		}

		public int UpID {
			get { return m_UpID; }
			set { m_UpID = value; }
		}

		public int Level {
			get { return m_Level; }
			set { m_Level = value; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", name: '" + m_Name + "'");
			s.Append(", upID: " + m_UpID);
			s.Append(", level: " + m_Level);
			return "{" + s.ToString() + "}";
		}
	}
}
