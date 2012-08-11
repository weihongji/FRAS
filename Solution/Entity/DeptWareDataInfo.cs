using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
	//预警模型数据采集类
	public class DeptWareDataInfo
	{
		private DateTime m_Year;                     //选择的年份
		private Double  m_GDP;                        //GDP
		private Decimal m_StateRevenue;              //城建收入
		private Decimal m_AvailableSR;               //预算可用财力安排的城建资金额
		private Decimal m_UrbanConstructionPay;      //城建支出
		private Decimal m_GovernmentCredit;          //当年政府信用资金用于城建资金额
		private Decimal m_ArrearsBalance;            //当期末累计拖欠地方政府债务外债转贷款本息余额
		private Decimal m_PreArrearsBalance;         //上期末累计拖欠政府外债转贷款本息余额
		private Decimal m_LocalAvailable;            //当地同期可用财力
		private Decimal m_GovernPrepareMoney;        //各级财政部门还贷准备金期末余额
		private Decimal m_FondBudgetIncome;          //基金预算收入
		private Decimal m_ExtraBudgetaryIncome;      //预算外收入
		private Decimal m_GeneralBudgetIncome;       //一般预算收入
		private Decimal m_ExtraFondBudgetPay;        //基金预算支出
		private Decimal m_ExtraBudgetPay;            //预算外支出
		private Decimal m_GeneralBAM;                //一般预算可用财力
		private Decimal m_CurrentYFR;                //当年财政收入
		private Decimal m_CurrentYFP;                //当年财政支出
    	//生成get和set方法
		public DateTime Year
		{
			get
			{
				return m_Year;
			}
			set
			{
				m_Year = value;
			}
		}
		public Double GDP
		{
			get
			{
				return m_GDP;
			}
			set
			{
				m_GDP = value;
			}
		}
		public Decimal StateRevenue
		{
			get
			{
				return m_StateRevenue;
			}
			set
			{
				m_StateRevenue = value;
			}
		}
		public Decimal AvailableSR
		{
			get
			{
				return m_AvailableSR;
			}
			set
			{
				m_AvailableSR = value;
			}
		}
		public Decimal UrbanConstructionPay
		{
			get
			{
				return m_UrbanConstructionPay;
			}
			set
			{
				m_UrbanConstructionPay = value;
			}
		}
		public Decimal GovernmentCredit
		{
			get
			{
				return m_GovernmentCredit;
			}
			set
			{
				m_GovernmentCredit = value;
			}
		}
		public Decimal ArrearsBalance
		{
			get
			{
				return m_ArrearsBalance;
			}
			set
			{
				m_ArrearsBalance = value;
			}
		}
		public Decimal PreArrearsBalance
		{
			get
			{
				return m_PreArrearsBalance;
			}
			set
			{
				m_PreArrearsBalance = value;
			}
		}
		public Decimal LocalAvailable
		{
			get
			{
				return m_LocalAvailable;
			}
			set
			{
				m_LocalAvailable = value;
			}
		}
		public Decimal GovernPrepareMoney
		{
			get
			{
				return m_GovernPrepareMoney;
			}
			set
			{
				m_GovernPrepareMoney = value;
			}
		}
		public Decimal FondBudgetIncome
		{
			get
			{
				return m_FondBudgetIncome;
			}
			set
			{
				m_FondBudgetIncome = value;
			}
		}
		public Decimal ExtraBudgetaryIncome
		{
			get
			{
				return m_ExtraBudgetaryIncome;
			}
			set
			{
				m_ExtraBudgetaryIncome = value;
			}
		}
		public Decimal GeneralBudgetIncome
		{
			get
			{
				return m_GeneralBudgetIncome;
			}
			set
			{
				m_GeneralBudgetIncome = value;
			}
		}
		public Decimal ExtraFondBudgetPay
		{
			get
			{
				return m_ExtraFondBudgetPay;
			}
			set
			{
				m_ExtraFondBudgetPay = value;
			}
		}
		public Decimal ExtraBudgetPay
		{
			get
			{
				return m_ExtraBudgetPay;
			}
			set
			{
				m_ExtraBudgetPay = value;
			}
		}
		public Decimal GeneralBAM
		{
			get
			{
				return m_GeneralBAM;
			}
			set
			{
				m_GeneralBAM = value;
			}
		}
		public Decimal CurrentYFR
		{
			get
			{
				return m_CurrentYFR;
			}
			set
			{
				m_CurrentYFR = value;
			}
		}
		public Decimal CurrentYFP
		{
			get
			{
				return m_CurrentYFP;
			}
			set
			{
				m_CurrentYFP = value;
			}
		}
    }
}
