using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class RankInfo : BaseInfo
	{
		int m_ID;
		string m_Name;

		public RankInfo() {
		}

		public RankInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_Name = row["Rank"].ToString().Trim();
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string Name {
			get { return m_Name; }
			set { m_Name = value.Trim(); }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", name: '" + m_Name + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
