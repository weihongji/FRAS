using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class Attendance_NewUserForm : PrivilegePage
{
	public int LeaveID {
		get {
			return this.GetQSInteger("id");
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		DataTable depts = DeptBiz.GetBriefList();
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();

		DataTable ranks = RankBiz.GetBriefList();
		this.repeaterRank.DataSource = ranks;
		this.repeaterRank.DataBind();

		DataTable devices = DeviceBiz.GetList();
		this.repeaterDevice.DataSource = devices;
		this.repeaterDevice.DataBind();
	}

	protected string GetDeviceName(object AntNo, object IP) {
		return AntNo.ToString() + " (" + IP.ToString().Trim() + ")";
	}
}