<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LogonUserForm.aspx.cs" Inherits="System_LogonUserForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	var userId = "<%=GetQSInteger("id") %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#selRole").change(function() {
			$("#selDept").attr("disabled", $("#selRole").val() != "19").val("-1");
		});

		$("#btnSave").click(function () {
			if (isEmpty($("#txtCode").val())) {
				alert("请输入帐号");
				$("#txtCode").focus();
				return;
			}
			if (isEmpty($("#txtPassword").val())) {
				alert("请输入密码");
				$("#txtPassword").focus();
				return;
			}
			if ($("#selRole").val() <= 0) {
				alert("请选择角色");
				$("#selRole").focus();
				return;
			}
			if (!$("#selDept").attr("disabled") && $("#selDept").val()<0) {
				alert("请选择部门");
				$("#selDept").focus();
				return;
			}

			$.post("LogonUserAjax.aspx?type=save"
				, { id: userId
					, code: $("#txtCode").val()
					, password: $("#txtPassword").val()
					, role: $("#selRole").val()
					, dept: $("#selDept").val()
					, active: $("#chkActive").attr("checked")
				}
				, function (response) {
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			location.href = "LogonUserList.aspx";
		});

		loadUser(userId);
	});

	function loadUser(id) {
		if (id == null || id == 0) {
			return;
		}

		$.get("LogonUserAjax.aspx?type=get", {id: id}, function(response) {
			eval("var user = " + response);
			$("#txtCode").val(user.code);
			$("#txtPassword").val(user.password);
			$("#selRole").val(user.roleType).change();
			$("#selDept").val(user.deptId);
			$("#chkActive").attr("checked", user.active);
		});
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:30px;">帐号</td>
			<td><input type="text" id="txtCode" class="textbox" maxlength="20" /></td>
		</tr>
		<tr>
			<td>密码</td>
			<td><input type="text" id="txtPassword" class="textbox" maxlength="20" /></td>
		</tr>
		<tr>
			<td>角色</td>
			<td><select id="selRole" class="default">
				<option value="0">-选择角色-</option>
				<option value="5">管理员</option>
				<option value="6">监控员</option>
				<option value="7">考勤员</option>
				<option value="19">办事员</option>
			</select></td>
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
			<td>&nbsp;</td>
			<td><label><input type="checkbox" id="chkActive" checked="checked" class="p" style="margin: 10px 5px 0px 0px;" />是否有效</label></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>

