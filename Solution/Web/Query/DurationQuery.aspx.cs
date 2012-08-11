using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Query_DurationQuery : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		if (GetQSInteger("show") == 1) {
			DataTable table = WorkDurationBiz.GetList(Request.QueryString["name"], GetQSInteger("usertype", -1), GetQSInteger("dept", -1), GetQSInteger("device", 0), Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
			this.repeaterDuration.DataSource = table;
			this.repeaterDuration.DataBind();
			this.litRowCount.Text = String.Format("总共{0}条记录", table.Rows.Count);
		}

		DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}