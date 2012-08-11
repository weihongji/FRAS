using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class Attendance_LeaveForm : PrivilegePage
{
	public int LeaveID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return LeaveID == 0 ? "新增请假" : "修改请假";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;

		if (GetQSInteger("id") == 0) {
			DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
			this.repeaterDept.DataSource = depts;
			this.repeaterDept.DataBind();
		}

		DataTable types = ParamBiz.GetLeaveBriefList();
		this.repeaterType.DataSource = types;
		this.repeaterType.DataBind();
	}
}