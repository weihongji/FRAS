using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Query_AccessQuery : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		if (GetQSInteger("show") == 1) {
			DataTable table = AccessBiz.GetList(Request.QueryString["name"], GetQSInteger("usertype", -1), GetQSInteger("dept", -1), GetQSInteger("device", 0), GetQSInteger("state", -1), Convert.ToDateTime(Request.QueryString["start"]), Convert.ToDateTime(Request.QueryString["end"]));
			this.repeaterAccess.DataSource = table;
			this.repeaterAccess.DataBind();
			this.litRowCount.Text = String.Format("总共{0}条记录", table.Rows.Count);
		}

		DataTable depts = DeptBiz.GetBriefList(LoginUserDeptID);
		this.repeaterDept.DataSource = depts;
		this.repeaterDept.DataBind();
	}
}