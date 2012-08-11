<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login0480.aspx.cs" Inherits="Misc_Login0480" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta content="IE=8" http-equiv="X-UA-Compatible"/>
	<title>张家峁矿业公司考勤管理系统-登陆页</title>
	<link type="text/css" rel="stylesheet" href="~/Style/login.css" />
	<script type="text/javascript">
		function btnLogin_onclick() {
			if (document.getElementById("txtUserCode").value.length == 0) {
				alert("未输入帐户名！请输入帐户名和密码后登录。");
				document.getElementById("txtUserCode").focus();
				return false;
			}
			else if (document.getElementById("txtPassword").value.length == 0) {
				alert("未输入密码！请输入帐户名和密码后登录。");
				document.getElementById("txtPassword").focus();
				return false;
			}
			return true;
		}

		function window_onload() {
			document.getElementById("txtUserCode").focus();
		}
	</script>
</head>
<body onload="window_onload()">
	<form id="form1" runat="server">
	<div class="zt">
		<div class="jg">
			<div class="d01">
				<img src="../Image/login_1.gif" alt="" /></div>
			<div class="d02">
				<img src="../Image/login_2.gif" alt="" /></div>
			<div class="d03">
				<img src="../Image/login_3.gif" alt="" /></div>
			<div class="d04">
				<div class="dlbd">
					账&nbsp;&nbsp;户：
					<asp:TextBox ID="txtUserCode" runat="server" class="wbk" MaxLength="30" autocomplete="off" TabIndex="1"></asp:TextBox>
					<br />
					密&nbsp;&nbsp;码：
					<asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="wbk" MaxLength="30" autocomplete="off" TabIndex="2"></asp:TextBox>
					<br />
					<asp:ImageButton ID="btnLogin" runat="server" class="tc" src="../Image/login_button.gif" OnClientClick="return btnLogin_onclick();" OnClick="btnLogin_Click" />
				</div>
			</div>
			<div class="d05">
				<img src="../Image/login_5.gif" alt="" /></div>
			<div class="d06">
				<img src="../Image/login_6.gif" alt="" /></div>
			<div class="d07">
				<img src="../Image/login_7.gif" alt="" /></div>
		</div>
	</div>
	</form>
</body>
</html>
