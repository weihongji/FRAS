using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Entity;

public partial class System_DeviceList : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {
		DataTable devices = DeviceBiz.GetList();
		this.repeaterDevice.DataSource = devices;
		this.repeaterDevice.DataBind();
	}
}