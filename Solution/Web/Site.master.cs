using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Entity;
using BLL;
using DAL;

public partial class SiteMaster : BaseMasterPage
{
	private DataTable m_FeatureTable;

	public bool IsShowAttendMenu {
		get {
			bool result = false;
			if (LoginUserRole == (int)XEnum.LoginUserRoleType.Administrator || LoginUserRole == (int)XEnum.LoginUserRoleType.KaoQinYuan || LoginUserRole == (int)XEnum.LoginUserRoleType.BanShiYuan) {
				result = true;
			}
			return result;
		}
	}

	public bool IsShowSystemMenu {
		get {
			bool result = false;
			if (LoginUserRole == (int)XEnum.LoginUserRoleType.Administrator) {
				result = true;
			}
			return result;
		}
	}

	public bool IsShowAttendAdvancedItem {
		get {
			bool result = false;
			if (LoginUserRole == (int)XEnum.LoginUserRoleType.Administrator || LoginUserRole == (int)XEnum.LoginUserRoleType.KaoQinYuan) {
				result = true;
			}
			return result;
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
	}

	public string FeatureUrl(string module, int index) {
		if (m_FeatureTable == null) {
			m_FeatureTable = (DataTable)Application["Features"];
		}
		return m_FeatureTable.Select("Module='" + module + "'")[index]["Url"].ToString();
	}

	public string FeatureName(string module, int index) {
		if (m_FeatureTable == null) {
			m_FeatureTable = (DataTable)Application["Features"];
		}
		return m_FeatureTable.Select("Module='" + module + "'")[index]["Name"].ToString();
	}
}
