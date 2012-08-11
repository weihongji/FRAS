<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="LeaveForm.aspx.cs" Inherits="Attendance_LeaveForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	var leaveId = "<%=GetQSInteger("id") %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#selDept").change(function() {
			$("#selUser").emptySelect(true); // clear user list

			if ($("#selDept").val() == "-1") return;

			$.get("../System/UserAjax.aspx?type=brief_list", {deptId: $("#selDept").val()}, function(response) {
				eval("var users = " + response);
				$("#selUser").loadSelect(users, true);
			});
		});

		$("#txtStart").datepicker($.datepicker.regional['zh-CN']);
		$("#txtEnd").datepicker($.datepicker.regional['zh-CN']);

		$("#btnSave").click(function () {
			if ($("#selDept:visible").length>0 && $("#selDept").val()<0) {
				alert("请选择部门");
				$("#selDept").focus();
				return;
			}
			if ($("#selUser:visible").length>0 && isEmpty($("#selUser").val())) {
				alert("请选择用户");
				$("#selUser").focus();
				return;
			}
			if (isEmpty($("#txtStart").val())) {
				alert("请输入起始日期");
				$("#txtStart").focus();
				return;
			}
			else if (!isDate($("#txtStart").val())) {
				alert("起始日期无效");
				$("#txtStart").focus();
				return;
			}
			if (isEmpty($("#txtEnd").val())) {
				alert("请输入结束日期");
				$("#txtEnd").focus();
				return;
			}
			else if (!isDate($("#txtEnd").val())) {
				alert("结束日期无效");
				$("#txtEnd").focus();
				return;
			}
			if ($("#selType").val()<0) {
				alert("请选择请假类别");
				$("#selType").focus();
				return;
			}

			$.post("LeaveAjax.aspx?type=save"
				, { id: leaveId
					, userId: $("#selUser").val()
					, startDate: $("#txtStart").val()
					, endDate: $("#txtEnd").val()
					, type: $("#selType").val()
				}
				, function (response) {
					if (response == "0") {
						if ($("#txtStart").val() == $("#txtEnd").val()) {
							alert("该用户在" + $("#txtStart").val() + "已经有请假，不能填写重复的请假。");
						}
						else {
							alert("该用户在" + $("#txtStart").val() + "到" + $("#txtEnd").val() + "期间已经有请假，不能填写重复请假。");
						}
						return;
					}
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "LeaveList.aspx" + qs;
		});

		if($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
			$("#selDept").change();
		}

		loadLeave(leaveId);
	});

	function loadLeave(id) {
		if (id == null || id == 0) {
			return;
		}

		$("#selDept").parent().parent().hide();

		$.get("LeaveAjax.aspx?type=get", {id: id}, function(response) {
			eval("var leave = " + response);
			$("#selUser").hide().after("<input type='text' class='textbox' value='" + leave.userName + "' disabled='true'>");
			$("#txtStart").val(leave.startDate);
			$("#txtEnd").val(leave.endDate);
			$("#selType").val(leave.type);
		});
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
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
			<td>姓名</td>
			<td><select id="selUser" class="default">
				<option value="">-选择姓名-</option>
			</select></td>
		</tr>
		<tr>
			<td style="width:60px;">起始日期</td>
			<td><input type="text" id="txtStart" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>结束日期</td>
			<td><input type="text" id="txtEnd" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>请假类别</td>
			<td><select id="selType" class="default">
				<option value="-1">-选择类型-</option>
				<asp:Repeater ID="repeaterType" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>
