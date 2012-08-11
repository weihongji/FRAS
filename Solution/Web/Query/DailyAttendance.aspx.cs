using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class Query_DailyAttendance : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		if (GetQSInteger("show") == 1) {
			DataTable table = WorkDurationBiz.GetDailyAttendance(Convert.ToDateTime(Request.QueryString["start"]));
			this.repeaterDuration.DataSource = table;
			this.repeaterDuration.DataBind();
			this.litRowCount.Text = String.Format("总共{0}条记录", table.Rows.Count);
		}
	}
}