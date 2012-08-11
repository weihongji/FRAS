using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class Attendance_AttendanceForm : PrivilegePage
{
	public int AttendanceID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return AttendanceID == 0 ? "新增出勤记录" : "修改出勤记录";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;

		if (GetQSInteger("id") == 0) {
			DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
			this.repeaterDept.DataSource = depts;
			this.repeaterDept.DataBind();
		}
	}
}