using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PrivilegePage
/// </summary>
public class PrivilegePage : BasePage
{
	public PrivilegePage() {
		this.Load += new EventHandler(PrivilegePage_Load);
	}

	protected void PrivilegePage_Load(object sender, EventArgs e) {
		VerifyPrivilege();
	}

	public void VerifyPrivilege() {
		if (LoginUserID <= 0) {
			Response.Redirect("~/Login.aspx", true);
		}
	}
}