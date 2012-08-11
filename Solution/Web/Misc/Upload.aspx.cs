using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Misc_Upload : PrivilegePage
{
	protected void Page_Load(object sender, EventArgs e) {

	}

	protected void btnUpload_Click(object sender, EventArgs e) {
		if (fileUpload1.HasFile) {
			string filePath = Server.MapPath("~/Upload") + "\\" + this.fileUpload1.FileName;
			this.fileUpload1.SaveAs(filePath);
			this.lblResult.Text = "OK" + this.fileUpload1.FileName;
		}
	}
}