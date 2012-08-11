using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Utility;

namespace Entity
{
	public class FeatureInputResultInfo : BaseInfo
	{
		int m_ID;
		string m_Token;
		string m_UserId;
		int m_Status;
		string m_Message;
		DateTime m_DTStamp;

		public FeatureInputResultInfo() {
		}

		public FeatureInputResultInfo(DataRow row) {
			m_ID = (int)row["ID"];
			m_Token = row["Token"].ToString();
			m_UserId = row["UserId"].ToString().Trim();
			m_Status = (int)row["Status"];
			if (row["Message"] == DBNull.Value) {
				m_Message = null;
			}
			else {
				m_Message = row["Message"].ToString();
			}
			m_DTStamp = (DateTime) row["DTStamp"];
		}

		public int ID {
			get { return m_ID; }
			set { m_ID = value; }
		}

		public string Token {
			get { return m_Token; }
			set { m_Token = value.Trim(); }
		}

		public string UserId {
			get { return m_UserId; }
			set { m_UserId = value.Trim(); }
		}

		public int Status {
			get { return m_Status; }
			set { m_Status = value; }
		}

		public string Message {
			get { return m_Message; }
			set { m_Message = value.Trim(); }
		}

		public DateTime DTStamp {
			get { return m_DTStamp; }
			set { m_DTStamp = value; }
		}
		
		public override string ToJson() {
			StringBuilder s = new StringBuilder();
			s.Append("id: " + m_ID);
			s.Append(", token: '" + m_Token + "'");
			s.Append(", userId: '" + m_UserId + "'");
			s.Append(", status: " + m_Status.ToString());
			s.Append(", message: '" + (string.IsNullOrEmpty(m_Message) ? "" : m_Message) + "'");
			return "{" + s.ToString() + "}";
		}
	}
}
