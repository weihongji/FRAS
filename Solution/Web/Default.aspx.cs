using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class _Default : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		Response.Redirect("~/Query/AttendanceOverview.aspx", true);
	}
}