using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class ParamBiz
	{
		public static ParamInfo GetEntity(int ID) {
			return ParamDao.GetEntity(ID);
		}

		public static DataTable GetList(XEnum.ParamType type = XEnum.ParamType.All) {
			return ParamDao.GetList(type);
		}

		public static DataTable GetDeviceBriefList() {
			return ParamDao.GetBriefList(XEnum.ParamType.Device);
		}

		public static DataTable GetRoleBriefList() {
			return ParamDao.GetBriefList(XEnum.ParamType.Role);
		}

		public static DataTable GetLeaveBriefList() {
			return ParamDao.GetBriefList(XEnum.ParamType.Leave);
		}
	}
}
