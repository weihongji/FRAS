<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="Misc_Upload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta content="IE=8" http-equiv="X-UA-Compatible"/>
	<title>文件上传</title>
	<base target="_self" />
	<script type="text/javascript" src="../Script/jquery.min.js"></script>
	<script type="text/javascript">
		function window_onload() {
			var result = $("#<%=lblResult.ClientID %>").text();
			if (result.length > 0 && result.substring(0, 2) == "OK") {
				window.returnValue = result.substring(2);
				if ($.browser.safari) { // To fix the Google Chrome bug
					if (window.opener) {
						window.opener.returnValue = window.returnValue;
					}
				}
				window.close();
			}
		}
	</script>
</head>
<body onload="window_onload();">
	<form id="form1" runat="server">
	<div>
		<div>
			<asp:FileUpload ID="fileUpload1" runat="server" />
			<asp:Button ID="btnUpload" Text="上传" OnClick="btnUpload_Click" runat="server" />
		</div>
		<div>
			<asp:Label ID="lblResult" runat="server"></asp:Label>
		</div>
	</div>
	</form>
</body>
</html>
