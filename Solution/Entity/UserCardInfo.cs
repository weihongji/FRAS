using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Entity
{
	public class UserCardInfo : BaseInfo
	{
		int m_ID;
		string m_CardID;
		string m_UserID;

		public UserCardInfo() {
		}

		public UserCardInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_CardID = row["cardId"].ToString().Trim();
			m_UserID = row["userId"].ToString().Trim();
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string CardID {
			get { return m_CardID; }
			set { m_CardID = value.Trim(); }
		}

		public string UserID {
			get { return m_UserID; }
			set { m_UserID = value.Trim(); }
		}

		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: '" + m_ID + "'");
			s.Append(", cardId: '" + m_CardID + "'");
			s.Append(", userId: '" + m_UserID + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
