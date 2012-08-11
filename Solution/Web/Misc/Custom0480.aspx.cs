using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

public partial class Misc_Custom0480 : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e) {
		if (Session["0480"] != "1") {
			Response.Redirect("Login0480.aspx");
		}
		if (!this.IsPostBack) {
			txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
		}
	}

	protected void btnSave_Click(object sender, EventArgs e) {
		bool success = CustomBiz.CreateAttendance("0480", Convert.ToDateTime(txtDate.Text), Convert.ToInt32(selRostering.SelectedValue));
		ClientScript.RegisterStartupScript(this.GetType(), "save_result", "showSaveResult(" + success.ToString().ToLower() + ")", true);
	}
}