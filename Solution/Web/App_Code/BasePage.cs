using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Entity;
using DAL;
using BLL;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
	public BasePage() {
		this.Load += new EventHandler(BasePage_Load);
	}

	protected void BasePage_Load(object sender, EventArgs e) {
		if (this.Header != null) { // Exclude ajax background pages
			SetModuleMenu();
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

	public int LoginUserID {
		get {
			if (Session["UserID"] != null) {
				return (int)(Session["UserID"]);
			}
			else {
				return 0;
			}
		}
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
				return (int) Session["UserRole"];
			}
			else {
				return 0;
			}
		}
	}

	public int LoginUserDeptID {
		get {
			if (Session["DeptID"] != null) {
				return (int)Session["DeptID"];
			}
			else {
				return -1;
			}
		}
	}

	/// <summary>
	/// Encode HTML characters in a string so that it can be displayed as normal text rather than interpreted as HTML.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public String HtmlToText(Object value) {
		if (value == null || String.IsNullOrEmpty(value.ToString())) {
			return "";
		}
		else {
			return System.Web.HttpUtility.HtmlEncode(value.ToString());
		}
	}

	/// <summary>
	/// Try to parse an integer number from a Query String item.
	/// </summary>
	/// <param name="itemName">Name of query string item</param>
	/// <returns>Return 0 if item value is not a valid number</returns>
	public int GetQSInteger(String itemName, int defaultValue = 0) {
		int number;
		if (int.TryParse(Request.QueryString[itemName], out number)) {
			return number;
		}
		else {
			return defaultValue;
		}
	}

	/// <summary>
	/// Try to parse an integer number from a posted Form item.
	/// </summary>
	public int GetFormInteger(String itemName, int defaultValue = 0) {
		int number;
		if (int.TryParse(Request.Form[itemName], out number)) {
			return number;
		}
		else {
			return defaultValue;
		}
	}

	private void SetModuleMenu() {
		// Set value to Current Module variable
		string url = Request.Path.ToLower();
		if (url.IndexOf("/query/") >= 0) {
			Session["CurrentModule"] = "Query";
		}
		else if (url.IndexOf("/attendance/") >= 0) {
			Session["CurrentModule"] = "Attendance";
		}
		else if (url.IndexOf("/system/") >= 0) {
			Session["CurrentModule"] = "System";
		}
		else {
			//Keep previous value
		}

		// Set value to Current Menu variable
		DataTable features = (DataTable)Application["Features"];
		if (features.Select("Name = '" + this.Title + "'").Length > 0) {
			this.CurrentFeature = this.Title;
		}
	}
}