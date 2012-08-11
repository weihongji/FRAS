using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;

public partial class System_DeviceForm : PrivilegePage
{
	public int DeviceID {
		get {
			return this.GetQSInteger("id");
		}
	}

	public string PageTitle {
		get {
			return DeviceID == 0 ? "新增设备信息" : "修改设备信息";
		}
	}

	protected void Page_Load(object sender, EventArgs e) {
		this.Page.Title = PageTitle;
		DataTable devices = ParamBiz.GetDeviceBriefList();
		this.repeaterDeviceType.DataSource = devices;
		this.repeaterDeviceType.DataBind();
	}
}