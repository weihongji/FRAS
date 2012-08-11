using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Misc_Login0480 : System.Web.UI.Page
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			// Clear existing login sessions
			Session["0480"] = null;
		}
	}

	protected void btnLogin_Click(object sender, EventArgs e) {
		LoginUser(txtUserCode.Text, txtPassword.Text);
	}

	private void LoginUser(String userCode, String password) {
		if (Request.Form["txtUserCode"] == "0480" && Request.Form["txtPassword"] == "wodemima") {
			Session["0480"] = "1";
			Response.Redirect("Custom0480.aspx");
		}
	}
}