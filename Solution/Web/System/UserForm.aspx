<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserForm.aspx.cs" Inherits="System_UserForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	var userId = "<%=UserID %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#btnSave").click(function () {
			if (isEmpty($("#txtId").val())) {
				alert("请输入工号");
				$("#txtId").focus();
				return;
			}
			if (isNaN($("#txtId").val()) || $("#txtId").val()<=0) {
				alert("工号无效");
				$("#txtId").select().focus();
				return;
			}
			if (isEmpty($("#txtName").val())) {
				alert("请输入姓名");
				$("#txtName").focus();
				return;
			}
			if ($("#selDept").val()<0) {
				alert("请选择部门");
				$("#selDept").focus();
				return;
			}
			if ($("#selRank").val() < 0) {
				alert("请选择职位");
				$("#selRank").focus();
				return;
			}

			$.post("UserAjax.aspx?type=save"
				, { id: $("#txtId").val()
					, name: $("#txtName").val()
					, deptId: $("#selDept").val()
					, rankId: $("#selRank").val()
					, senderId: $("#txtSender").val()
					, type: $("#selType").val()
				}
				, function (response) {
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "UserList.aspx" + qs;
		});

		setIdState();
		loadUser(userId);
	});

	function loadUser(id) {
		if (id == null || id == 0) {
			return;
		}

		$.get("UserAjax.aspx?type=get", {id: id}, function(response) {
			eval("var user = " + response);
			$("#txtId").val(user.id);
			$("#txtName").val(user.name);
			$("#selDept").val(user.deptId);
			$("#selRank").val(user.rankId);
			$("#txtSender").val(user.senderId);
			$("#selType").val(user.type);
		});
	}

	function setIdState() {
		if (userId > 0) {
			$("#txtId").attr("disabled", true);
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:50px;">工号</td>
			<td><input type="text" id="txtId" class="textbox" maxlength="6" /></td>
		</tr>
		<tr>
			<td>姓名</td>
			<td><input type="text" id="txtName" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>部门</td>
			<td><select id="selDept" class="default">
				<option value="-1">-选择部门-</option>
				<asp:Repeater ID="repeaterDept" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></td>
		</tr>
		<tr>
			<td>职位</td>
			<td><select id="selRank" class="default">
				<option value="-1">-选择职位-</option>
				<asp:Repeater ID="repeaterRank" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></td>
		</tr>
		<tr>
			<td>射频号</td>
			<td><input type="text" id="txtSender" class="textbox" maxlength="6" /></td>
		</tr>
		<tr>
			<td>类型</td>
			<td><select id="selType" class="default">
				<option value="-1">-选择类型-</option>
				<option value="0">正式工</option>
				<option value="1">劳务工</option>
				<option value="2">中煤中宇</option>
			</select></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>
