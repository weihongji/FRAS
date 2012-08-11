<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Custom0480.aspx.cs" Inherits="Misc_Custom0480" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>出勤管理</title>
	<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
	<style type="text/css">
		a:link, a:visited {
			color: #11569B;
			text-decoration: none;
		}

		a:hover {
			color: #1d60ff;
			text-decoration: underline;
		}

		a:active {
			color: #034af3;
		}
	</style>
	<script type="text/javascript" src="../Script/jquery.min.js"></script>
	<script type="text/javascript" src="../Script/jquery-ui.js"></script>
	<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
	<script type="text/javascript" src="../Script/fras.js"></script>
	<script type="text/javascript">
		$(function () {
			$("#txtDate").datepicker($.datepicker.regional['zh-CN']);
		});

		function btnSave_onclick() {
			if (isEmpty($("#txtDate").val())) {
				alert("请输入起始日期");
				$("#txtDate").focus();
				return false;
			}
			return true;
		}

		function showSaveResult(success) {
			if (success) {
				alert("保存成功！");
			}
			else {
				alert("保存失败！\n可能当天的出勤记录已经存在。");
			}
		}
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<div>
			<div style="float:left; font-size:1.5em; font-weight:bold;">出勤管理</div>
			<div style="float:left; margin-left:200px; line-height:1.5em"><a href="Login0480.aspx">退出</a></div>
			<div style="clear:both"></div>
		</div>
		<hr />
		<br />
		<fieldset>
			<table cellpadding="5" cellspacing="0">
				<tr>
					<td>日期</td>
					<td><asp:TextBox ID="txtDate" MaxLength="10" Width="80px" runat="server"></asp:TextBox></td>
					<td>班次</td>
					<td><asp:DropDownList ID="selRostering" runat="server">
						<asp:ListItem Value="1">八点班</asp:ListItem>
						<asp:ListItem Value="2">四点班</asp:ListItem>
						<asp:ListItem Value="0">零点班</asp:ListItem>
					</asp:DropDownList></td>
					<td>&nbsp;</td>
					<td><asp:Button ID="btnSave" OnClientClick="return btnSave_onclick();" OnClick="btnSave_Click" runat="server" Text="保存" style="width:60px;" /></td>
				</tr>
			</table>
		</fieldset>
	</div>
	</form>
</body>
</html>
