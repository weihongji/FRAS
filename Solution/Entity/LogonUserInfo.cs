using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class LogonUserInfo : BaseInfo
	{
		int m_ID;
		string m_Code;
		string m_Password;
		int m_RoleType;
		int m_DeptID;
		bool m_Active;
		/* for read-only properties */
		string m_RoleName;
		string m_DeptName;
		string m_ActiveName;

		public LogonUserInfo() {
		}

		public LogonUserInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_Code = row["logonUser"].ToString().Trim();
			m_Password = row["passwd"].ToString().Trim();
			m_RoleType = (int)row["roleType"];
			if (row["depId"] == DBNull.Value) {
				m_DeptID =  -1;
			}
			else {
				m_DeptID =  (int)row["depId"];
			}
			m_Active = (int)row["flag"] == 1;
			m_RoleName = row["RoleName"].ToString().Trim();
			m_DeptName = row["DeptName"].ToString().Trim();
			m_ActiveName = row["ActiveName"].ToString().Trim();
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string Code {
			get { return m_Code; }
			set { m_Code = value.Trim(); }
		}

		public string Password {
			get { return m_Password; }
			set { m_Password = value.Trim(); }
		}

		public int RoleType {
			get { return m_RoleType; }
			set { m_RoleType = value; }
		}

		public int DeptID {
			get { return m_DeptID; }
			set { m_DeptID = value; }
		}

		public bool Active {
			get { return m_Active; }
			set { m_Active = value; }
		}

		public string RoleName {
			get { return m_RoleName; }
		}

		public string DeptName {
			get { return m_DeptName; }
		}

		public string ActiveName {
			get { return m_ActiveName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", code: '" + m_Code + "'");
			s.Append(", password: '" + m_Password + "'");
			s.Append(", roleType: " + m_RoleType);
			s.Append(", deptId: " + m_DeptID);
			s.Append(", active: " + (m_Active ? "true" : "false"));
			s.Append(", activeName: '" + m_ActiveName + "'");
			s.Append(", roleName: '" + m_RoleName + "'");
			s.Append(", deptName: '" + m_DeptName + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
