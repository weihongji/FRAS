<%@ Page Title="员工信息管理" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserList.aspx.cs" Inherits="System_UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	$(function () {
		$.ajaxSettings.async = false;

		initValues();
		setButtonState();

		$("#txtName").keypress(function (event) {
			if (event.keyCode == 13) {
				$("#btnSearch").click();
				return false;
			}
		});

		$("#btnSearch").click(function () {
			var url = location.href;
			url = removeQSItem(url, "dept");
			url = removeQSItem(url, "name");
			if ($("#selDept").val() >= 0) {
				url = setQSItem(url, "dept", $("#selDept").val());
			}
			if ($("#txtName").val().length > 0) {
				url = setQSItem(url, "name", $("#txtName").val());
			}
			url = setQSItem(url, "show", "1");
			location.href = url;
		});

		$("#btnSelectAll").click(function () {
			if ($("#btnSelectAll").val() == "全选") {
				$(".list :checkbox").attr("checked", true);
				$("#btnSelectAll").val("取消");
			}
			else {
				$(".list :checkbox").attr("checked", false);
				$("#btnSelectAll").val("全选");
			}
			setButtonState();
		});

		$("#btnAdd").click(function () {
			location.href = "UserForm.aspx?id=0<%=ListQS%>";
		});

		$("#btnDelete").click(function () {
			var selectedCount = $(".list :checkbox:checked").length;
			var msg = "你确定要删除选中的员工信息吗？";
			if (selectedCount > 1) {
				msg = "你确定要删除选中的（" + $(".list :checkbox:checked").length + "个）员工信息吗？"
			}
			if (!confirm(msg)) {
				return;
			}
			$(".list :checkbox:checked").each(function () {
				var id = this.value;
				$.post("UserAjax.aspx?type=delete", { id: id }, function (response) {
					if (response != "true") {
						alert("删除失败！没有找到编号为" + id + "的员工信息。");
					}
				});
			});
			document.location.reload();
		});

		$(".list :checkbox").click(function () {
			if (this.checked) {
				if ($(".list :checkbox").not(":checked").length == 0) {
					$("#btnSelectAll").val("取消");
				}
			}
			else {
				if ($("#btnSelectAll").val() == "取消") {
					$("#btnSelectAll").val("全选");
				}
			}
			setButtonState();
		});
	});

	function setButtonState() {
		$("#btnDelete").attr("disabled", $(".list :checkbox:checked").length == 0);
	}

	function initValues() {
		$("#selDept").val("<%=GetQSInteger("dept", -1) %>");
		$("#txtName").val("<%=Request.QueryString["name"] %>");
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<fieldset style="margin-bottom:20px; width:500px;">
		<legend>检索</legend>
		<div>
			<div style="float:left;">部门：<select id="selDept" class="default">
				<option value="-1">-所有部门-</option>
				<asp:Repeater ID="repeaterDept" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></div>
			<div style="float:left; margin-left:30px;">姓名/工号：<input type="text" id="txtName" class="textbox" /></div>
			<div style="float:left; margin-left:30px;"><input type="button" class="button2" id="btnSearch" value="检索" /></div>
			<div style="clear:both;"></div>
		</div>
	</fieldset>
	<div style="margin-bottom:10px;">
		<input type="button" class="button2" id="btnSelectAll" value="全选" />
		<input type="button" class="button2" id="btnAdd" value="新增" />
		<input type="button" class="button2" id="btnDelete" value="删除" />
	</div>
	<table class="list">
		<thead>
			<tr>
				<th style="width: 50px;">选择</th>
				<th style="width: 60px;">工号</th>
				<th style="width: 80px;">姓名</th>
				<th style="width: 110px;">部门</th>
				<th style="width: 150px;">职位</th>
				<th style="width: 60px;">射频号</th>
				<th style="width: 60px;">类别</th>
				<th style="width: 50px;">修改</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterUser" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><input type="checkbox" class="p" id="chk<%#Eval("userId") %>" value="<%#Eval("userId") %>" /></td>
				<td><%#Eval("userId") %></td>
				<td><%#Eval("userName")%></td>
				<td><%#Eval("DeptName")%></td>
				<td><%#Eval("RankName")%></td>
				<td><%#Eval("senderId")%></td>
				<td class="c"><%#Eval("TypeName")%></td>
				<td class="c"><a href="UserForm.aspx?id=<%#Eval("userId").ToString().Trim() %><%=ListQS%>">修改</a></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
</asp:Content>

