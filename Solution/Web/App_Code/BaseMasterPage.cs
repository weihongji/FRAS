using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BaseMasterPage
/// </summary>
public class BaseMasterPage : System.Web.UI.MasterPage
{
	public BaseMasterPage() {

	}

	public String LoginUserCode {
		get {
			if (Session["UserCode"] != null) {
				return Session["UserCode"].ToString();
			}
			else {
				return "";
			}
		}
	}

	public int LoginUserRole {
		get {
			if (Session["UserRole"] != null) {
				return (int)Session["UserRole"];
			}
			else {
				return 0;
			}
		}
	}

	public string CurrentModule {
		get {
			if (Session["CurrentModule"] == null) {
				return "Query";
			}
			else {
				return (string)Session["CurrentModule"];
			}
		}
		set {
			Session["CurrentModule"] = value;
		}
	}

	public String CurrentFeature {
		get {
			if (Session["CurrentFeature"] == null) {
				return "";
			}
			else {
				return Session["CurrentFeature"].ToString();
			}
		}
		set {
			Session["CurrentFeature"] = value;
		}
	}
}