using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Query_AttendanceReport : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		if (GetQSInteger("show") == 1) {
			DateTime start, end;
			if (String.IsNullOrEmpty(Request.QueryString["year"])) {
				start = Convert.ToDateTime(Request.QueryString["start"]);
				end = Convert.ToDateTime(Request.QueryString["end"]);
			}
			else {
				start = new DateTime(GetQSInteger("year"), GetQSInteger("month"), 1);
				end = Utility.DateUtility.GetLastDay(start);
			}
			DataTable table = AttendanceBiz.GetReportList(GetQSInteger("dept", -1), GetQSInteger("device", 0), GetQSInteger("usertype", -1), start, end);
			this.repeaterAttendance.DataSource = table;
			this.repeaterAttendance.DataBind();
			this.litRowCount.Text = String.Format("总共{0}条记录", table.Rows.Count);
		}

		DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}