<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AttendanceOverviewContent.aspx.cs" EnableViewState="false" Inherits="Query_AttendanceOverviewContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<meta content="IE=8" http-equiv="X-UA-Compatible"/>
	<title></title>
	<link type="text/css" rel="stylesheet" href="~/Style/common.css" />
	<style type="text/css">
		#divPic .pic {
			float:left;
			margin:5px;
		}
		
		#divPic img {
			width:120px;
			height:160px;
			border:1px solid black;
		}
		
		#divPic .picTitle {
			margin-top:5px;
			text-align:center;
		}
	</style>
</head>
<body>
	<form id="form1" runat="server">
	<div>
		<div id="divPic" style="width:98%; min-height:200px;">
		<asp:Repeater ID="photoRepeater" runat="server">
			<ItemTemplate>
			<div class="pic">
				<img src="../Picture/<%#GetField(Container.DataItem, "photo") %>" alt="" />
				<div class="picTitle">姓名：<%#GetField(Container.DataItem, "userName")%><br />工号：<%#GetField(Container.DataItem, "userId")%></div>
			</div>
			</ItemTemplate>
		</asp:Repeater>
			<div style="clear:both;"></div>
		</div>
		<hr />
		<div style="width:98%;">
			<asp:Literal ID="litMsg" runat="server"></asp:Literal>
		</div>
	</div>
	</form>
</body>
</html>
