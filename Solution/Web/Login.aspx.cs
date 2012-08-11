using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity;
using BLL;


public partial class Login : BasePage
{
	protected void Page_Load(object sender, EventArgs e) {
		if (!IsPostBack) {
			// Clear existing login sessions
			Session["UserID"] = null;
			Session["UserCode"] = null;
			Session["UserRole"] = null;
			Session["DeptID"] = null;
		}
	}

	protected void btnLogin_Click(object sender, EventArgs e) {
		LoginUser(txtUserCode.Text, txtPassword.Text);
	}

	private void LoginUser(String userCode, String password) {
		if (LogonUserBiz.Exist(userCode, password)) {
			LogonUserInfo user = LogonUserBiz.GetEntity(userCode);
			if (!user.Active) {
				return;
			}
			Session["UserID"] = user.ID; // 登录用户编号
			Session["UserCode"] = user.Code; // 登录用户帐号
			Session["UserRole"] = user.RoleType; // 登录用户角色
			Session["DeptID"] = user.DeptID; // 登录用户部门
			Response.Redirect("Default.aspx");
		}
	}
}