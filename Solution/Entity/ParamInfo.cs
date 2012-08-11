using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class ParamInfo : BaseInfo
	{
		int m_ID;
		string m_Name;
		string m_Value;
		int m_Type;

		public ParamInfo() {
		}

		public ParamInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_Name = row["paraName"].ToString().Trim();
			m_Value = row["paraValue"].ToString().Trim();
			m_Type = (int)row["paraType"];
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string Name {
			get { return m_Name; }
			set { m_Name = value.Trim(); }
		}

		public string Value {
			get { return m_Value; }
			set { m_Value = value.Trim(); }
		}

		public int Type {
			get { return m_Type; }
			set { m_Type = value; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", name: '" + m_Name + "'");
			s.Append(", value: '" + m_Value + "'");
			s.Append(", type: " + m_Type);
			return "{" + s.ToString() + "}";
		}
	}
}
