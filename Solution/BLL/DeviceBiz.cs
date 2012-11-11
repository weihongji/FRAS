using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Entity;
using DAL;

namespace BLL
{
	public class DeviceBiz
	{
		public static int Save(DeviceInfo device) {
			int id;
			if (device.ID <= 0) {
				id = DeviceDao.Add(device);
			}
			else {
				id = DeviceDao.Update(device) ? device.ID : 0;
			}
			return id;
		}

		public static bool Delete(int ID) {
			return DeviceDao.Delete(ID);
		}

		public static DeviceInfo GetEntity(int ID) {
			return DeviceDao.GetEntity(ID);
		}

		public static DeviceInfo GetEntity(String name) {
			return DeviceDao.GetEntity(name);
		}

		public static DataTable GetList(bool ActiveOnly = false) {
			return DeviceDao.GetList(ActiveOnly);
		}

		public static string GetMonitorText() {
			StringBuilder text = new StringBuilder();
			DataTable table = DeviceDao.GetListForMonitor();
			foreach (DataRow row in table.Rows) {
				if (text.Length > 0) {
					text.Append(", ");
				}
				text.Append("{text:'" + row["Location"].ToString() + "', value:'" + row["Status"].ToString() + "'}");

			}
			return "[" + text.ToString() + "]";
		}
	}
}
