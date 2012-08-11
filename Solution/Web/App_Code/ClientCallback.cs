using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SNDService.Contracts;
using System.ServiceModel;
using System.Web;

namespace Client
{
    public class MessageTable
    {
		public static List<string> MsgList = new List<string>();
		public static SNDService.Contracts.UserInfoType UserType = new SNDService.Contracts.UserInfoType();
	}

    //public class ClientCallback : IKQHandlerCallback
    //{
    //    public ClientCallback() {
    //    }

    //    public void ExcuteResult(int statues, string msg) {
    //        MessageTable.MsgList.Add(statues.ToString());
    //        MessageTable.MsgList.Add(msg.ToString());
    //    }
    //}

}
