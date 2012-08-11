<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttendanceForm.aspx.cs" Inherits="Attendance_AttendanceForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>

<script type="text/javascript">
	var attendanceId = "<%=GetQSInteger("id") %>";
	var m_checkDuplicate = true;
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

		$("#txtDate").datepicker($.datepicker.regional['zh-CN']);
		$("#txtDate").val(formatDate(new Date()));

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
			if (isEmpty($("#txtDate").val())) {
				alert("请输入日期");
				$("#txtDate").focus();
				return;
			}
			else if (!isDate($("#txtDate").val())) {
				alert("日期无效");
				$("#txtDate").focus();
				return;
			}

			$.post("AttendanceAjax.aspx?type=save"
				, { id: attendanceId
					, userId: $("#selUser").val()
					, date: $("#txtDate").val()
					, duration: $("#selDuration").val()
					, nightWork: $("#selType").val()
					, inWell: $("#chkInWell").attr("checked")
					, checkDuplicate: m_checkDuplicate
				}
				, function (response) {
					if (response == "0") {
						var insist2save = confirm("该用户在" + $("#txtDate").val() + "已经有报工，是否仍然报工？");
						if (insist2save) {
							m_checkDuplicate = false;
							$("#btnSave").click();
						}
					}
					else {
						$("#btnBack").click();
					}
				}
			);
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "AttendanceList.aspx" + qs;
		});

		if($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
			$("#selDept").change();
		}

		loadAttendance(attendanceId);
	});

	function loadAttendance(id) {
		if (id == null || id == 0) {
			return;
		}

		$("#selDept").parent().parent().hide();

		$.get("AttendanceAjax.aspx?type=get", {id: id}, function(response) {
			eval("var attendance = " + response);
			$("#selUser").hide().after("<input type='text' class='textbox' value='" + attendance.userName + "' disabled='true'>");
			$("#txtDate").val(attendance.date);
			$("#selDuration").val(attendance.duration);
			$("#selType").val(attendance.nightWork);
			$("#chkInWell").attr("checked", attendance.inWell);
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
			<td style="width:60px;">日期</td>
			<td><input type="text" id="txtDate" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>工长</td>
			<td><select id="selDuration" class="default">
				<option value="0">0</option>
				<option value="0.5" selected="selected">0.5</option>
				<option value="1.0">1.0</option>
				<option value="1.5">1.5</option>
			</select></td>
		</tr>
		<tr>
			<td>夜班类型</td>
			<td><select id="selType" class="default">
				<option value="0">无</option>
				<option value="1">前夜</option>
				<option value="2">后夜</option>
				<option value="3">前&后夜</option>
			</select></td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td><label><input type="checkbox" id="chkInWell" checked="checked" class="p" style="margin: 10px 5px 0px 0px;" />是否入井</label></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>
