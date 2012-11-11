using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class DeviceInfo : BaseInfo
	{
		int m_ID;
		string m_IP;
		string m_Port;
		string m_DeviceType;
		string m_UserName;
		string m_Password;
		int m_AntNo;
		int m_AccessFlag;
		string m_Location;
		bool m_Active;
		string m_DeviceTypeName;
		string m_AccessFlagName;
		string m_ActiveName;

		public DeviceInfo() {
		}

		public DeviceInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_IP = row["devIp"].ToString().Trim();
			m_Port = row["devPort"].ToString().Trim();
			m_DeviceType = row["devType"].ToString().Trim();
			m_UserName = row["devUserName"].ToString().Trim();
			m_Password = row["devPassword"].ToString().Trim();
			m_AntNo = (int)row["antNo"];
			m_AccessFlag = (int)row["accessFlag"];
			m_Location = row["Location"].ToString().Trim();
			m_Active = (int)row["flag"] == 1;
			m_DeviceTypeName = row["DevTypeName"].ToString().Trim();
			m_ActiveName = row["ActiveName"].ToString().Trim();
			m_AccessFlagName = row["AccessFlagName"].ToString().Trim();
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string IP {
			get { return m_IP; }
			set { m_IP = value.Trim(); }
		}

		public string Port {
			get { return m_Port; }
			set { m_Port = value.Trim(); }
		}

		public string DeviceType {
			get { return m_DeviceType; }
			set { m_DeviceType = value.Trim(); }
		}

		public string UserName {
			get { return m_UserName; }
			set { m_UserName = value.Trim(); }
		}

		public string Password {
			get { return m_Password; }
			set { m_Password = value.Trim(); }
		}

		public int AntNo {
			get { return m_AntNo; }
			set { m_AntNo = value; }
		}

		public int AccessFlag {
			get { return m_AccessFlag; }
			set { m_AccessFlag = value; }
		}

		public string Location {
			get { return m_Location; }
			set { m_Location = value.Trim(); }
		}

		public bool Active {
			get { return m_Active; }
			set { m_Active = value; }
		}

		public string ActiveName {
			get { return m_ActiveName; }
		}

		public string DeviceTypeName {
			get { return m_DeviceTypeName; }
		}

		public string AccessFlagName {
			get { return m_AccessFlagName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", ip: '" + m_IP + "'");
			s.Append(", port: '" + m_Port + "'");
			s.Append(", deviceType: '" + m_DeviceType + "'");
			s.Append(", userName: '" + m_UserName + "'");
			s.Append(", password: '" + m_Password + "'");
			s.Append(", antNo: " + m_AntNo);
			s.Append(", accessFlag: " + m_AccessFlag);
			s.Append(", location: '" + m_Location + "'");
			s.Append(", active: " + (m_Active ? "true" : "false"));
			s.Append(", activeName: '" + m_ActiveName + "'");
			s.Append(", deviceTypeName: '" + m_DeviceTypeName + "'");
			s.Append(", accessFlagName: '" + m_AccessFlagName + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
