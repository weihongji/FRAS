using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Attendance_LeaveList : PrivilegePage
{
	protected string ListQS {
		get {
			string qs = Request.ServerVariables["QUERY_STRING"];
			return string.IsNullOrEmpty(qs) ? "" : "&" + qs;
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		if (GetQSInteger("show") == 1) {
			DataTable leaves = LeaveBiz.GetList(GetQSInteger("dept", -1), Request.QueryString["user"], GetQSInteger("state", 0));
			this.repeaterLeave.DataSource = leaves;
			this.repeaterLeave.DataBind();
		}

		DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}

	protected string GetRecordCheckbox(object ID, object flag) {
		if ((int)flag != 1 || this.LoginUserRole == 5) {
			return "<input type=\"checkbox\" class=\"p\" id=\"chk" + ID.ToString() + "\" value=\"" + ID.ToString() + "\" />";
		}
		else {
			return "";
		}
	}

	protected string GetEditLink(object ID, object flag) {
		if ((int)flag != 1 || this.LoginUserRole == 5) {
			return "<a href=\"LeaveForm.aspx?id=" + ID.ToString() + ListQS + "\">修改</a>";
		}
		else {
			return "";
		}
	}
}