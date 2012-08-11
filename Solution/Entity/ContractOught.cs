using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
////////////////////////////////////////////
//当年偿还数类
namespace Entity
{
	public class ContractOught
	{
		private int m_ContractID;
		private DateTime m_OughtPayDateTime;
		private int m_OughtPay;
		private int m_OughtPaySub1;
		private Decimal m_OughtPayInput;
		private int m_UserID;
		private DateTime m_DTStamp;
		/// <summary>
		/// 生成get和set方法
		/// </summary>
		public int ContractID
		{
			get
			{
				return m_ContractID;
			}
			set
			{
				m_ContractID = value;
			}
		}
		public DateTime OughtPayDateTime
		{
			get
			{
				return m_OughtPayDateTime;
			}
			set
			{
				m_OughtPayDateTime = value;
			}
		}
		public int OughtPay
		{
			get
			{
				return m_OughtPay;
			}
			set
			{
				m_OughtPay = value;
			}
		}
		public int OughtPaySub1
		{
			get
			{
				return m_OughtPaySub1;
			}
			set
			{
				m_OughtPaySub1 = value;
			}
		}
		public Decimal OughtPayInput
		{
			get
			{
				return m_OughtPayInput;
			}
			set
			{
				m_OughtPayInput = value;
			}
		}
		public int UserID
		{
			get
			{
				return m_UserID;
			}
			set
			{
				m_UserID = value;
			}
		}
		public DateTime DTStamp
		{
			get
			{
				return m_DTStamp;
			}
			set
			{
				m_DTStamp = value;
			}
		}
	}
}
