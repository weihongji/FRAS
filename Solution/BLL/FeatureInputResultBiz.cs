using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class FeatureInputResultBiz
	{
		public static bool Exist(string token) {
			return FeatureInputResultDao.Exist(token);
		}

		public static FeatureInputResultInfo GetEntity(string token) {
			return FeatureInputResultDao.GetEntity(token);
		}
	}
}
