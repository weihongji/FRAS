using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ServiceModel;
using System.Threading;
using Client;
using BLL;
using Entity;

public partial class Attendance_NewUserFeature : BasePage
{
	protected void Page_Load(object sender, EventArgs e) {
		string queryType = Request.QueryString["type"];
		switch (queryType) {
			case "get":
				string result = GetPostBack();
				Response.Write(result);
				break;
			case "send":
				CallWCF();
				Response.Write("true");
				break;
			default:
				Response.Write("Unknown query type.");
				break;
		}
	}

	private void CallWCF() {
		MessageTable.UserType.UserId = Request.Form["userId"];
		MessageTable.UserType.IP =  Request.Form["ip"];
		MessageTable.UserType.DevNum = GetFormInteger("devNum");
		MessageTable.UserType.Token = Request.Form["token"];

        //ThreadPool.QueueUserWorkItem(new WaitCallback(delegate {
        //    ClientCallback callBack = new ClientCallback();
        //    InstanceContext context = new InstanceContext(callBack);
        //    string url = System.Configuration.ConfigurationManager.AppSettings["KQService"];
        //    KQHandlerClient client = new KQHandlerClient(context, new WSDualHttpBinding(), new EndpointAddress(url));
        //    client.Open();
        //    client.InputFeature(MessageTable.UserType);
        //}));
        string url = System.Configuration.ConfigurationManager.AppSettings["KQService"];
        KQHandlerClient client = new KQHandlerClient( new BasicHttpBinding(), new EndpointAddress(url));
        client.InputFeature(MessageTable.UserType);
	}

	private string GetPostBack() {
		string token = Request.QueryString["token"];
		if (string.IsNullOrEmpty(token)) {
			return "{id:0, status:0, message: 'Invalid token: " + token + "' }";
		}

		FeatureInputResultInfo result = FeatureInputResultBiz.GetEntity(token);
		if (result == null) {
			return "{}";
		}
		else {
			return result.ToJson();
		}
	}
}