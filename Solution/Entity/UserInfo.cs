using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class UserInfo : BaseInfo
	{
		string m_ID;
		string m_Name;
		int m_DeptID;
		int m_FeatureID;
		int m_RankID;
		string m_SenderID;
		int m_RosteringID;
		int m_Type;
		int m_CopyType;
		int m_Flag;

		/* for read-only properties */
		string m_DeptName;
		string m_RankName;
		string m_TypeName;
		string m_CopyTypeName;

		/* variables that will not be populate in constructors */
		UserCardInfo m_CardID;

		public UserInfo() {
			m_FeatureID = 0;
			m_RosteringID = 0;
		}

		public UserInfo(DataRow row) {
			m_ID = row["userId"].ToString().Trim();
			m_Name = row["userName"].ToString().Trim();
			m_DeptID = (int)row["deptId"];
			m_FeatureID = (int)row["featureId"];
			m_RankID = (int)row["rankId"];
			m_SenderID = row["senderId"].ToString().Trim();
			m_RosteringID = (int)row["rosteringId"];
			m_Type = (int)row["type"];
			if (row["copyType"] == DBNull.Value) {
				m_CopyType = -1;
			}
			else {
				m_CopyType = (int)row["copyType"];
			}
			if (row["flag"] == DBNull.Value) {
				m_Flag = -1;
			}
			else {
				m_Flag = (int)row["flag"];
			}

			m_DeptName = row["DeptName"].ToString().Trim();
			m_RankName = row["RankName"].ToString().Trim();
			m_TypeName = row["TypeName"].ToString().Trim();
			m_CopyTypeName = row["CopyTypeName"].ToString().Trim();
		}

		public string ID {
			get { return m_ID; }
			set { m_ID = value.Trim(); }
		}

		public string Name {
			get { return m_Name; }
			set { m_Name = value.Trim(); }
		}

		public int DeptID {
			get { return m_DeptID; }
			set { m_DeptID = value; }
		}

		public int FeatureID {
			get { return m_FeatureID; }
			set { m_FeatureID = value; }
		}

		public int RankID {
			get { return m_RankID; }
			set { m_RankID = value; }
		}

		public string SenderID {
			get { return m_SenderID; }
			set { m_SenderID = value.Trim(); }
		}

		public int RosteringID {
			get { return m_RosteringID; }
			set { m_RosteringID = value; }
		}

		public int Type {
			get { return m_Type; }
			set { m_Type = value; }
		}

		public int CopyType {
			get { return m_CopyType; }
			set { m_CopyType = value; }
		}

		public int Flag {
			get { return m_Flag; }
			set { m_Flag = value; }
		}

		public UserCardInfo CardID {
			get { return m_CardID; }
			set { m_CardID = value; }
		}
		
		public string DeptName {
			get { return m_DeptName; }
		}

		public string RankName {
			get { return m_RankName; }
		}

		public string TypeName {
			get { return m_TypeName; }
		}

		public string CopyTypeName {
			get { return m_CopyTypeName; }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: '" + m_ID + "'");
			s.Append(", name: '" + m_Name + "'");
			s.Append(", deptId: " + m_DeptID);
			s.Append(", featureId: " + m_FeatureID);
			s.Append(", rankId: " + m_RankID);
			s.Append(", senderId: '" + m_SenderID + "'");
			s.Append(", rosteringId: " + m_RosteringID);
			s.Append(", type: " + m_Type);
			s.Append(", copyType: " + m_CopyType);
			s.Append(", flag: " + m_Flag);
			s.Append(", deptName: '" + m_DeptName + "'");
			s.Append(", rankName: '" + m_RankName + "'");
			s.Append(", typeName: '" + m_TypeName + "'");
			s.Append(", copyTypeName: '" + m_CopyTypeName + "'");
			s.Append(", cardId: '" + (m_CardID == null ? "" : m_CardID.CardID) + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
