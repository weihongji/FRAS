<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="HolidayForm.aspx.cs" Inherits="Attendance_HolidayForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	var holidayId = "<%=GetQSInteger("id") %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#txtStart").datepicker($.datepicker.regional['zh-CN']);
		$("#txtEnd").datepicker($.datepicker.regional['zh-CN']);

		$("#btnSave").click(function () {
			if (isEmpty($("#txtName").val())) {
				alert("请输入节假日名称");
				$("#txtName").focus();
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

			$.post("HolidayAjax.aspx?type=save"
				, { id: holidayId
					, name: $("#txtName").val()
					, startDate: $("#txtStart").val()
					, endDate: $("#txtEnd").val()
					, active: $("#chkActive").attr("checked")
				}
				, function (response) {
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "HolidayList.aspx" + qs;
		});

		loadHoliday(holidayId);
	});

	function loadHoliday(id) {
		if (id == null || id == 0) {
			return;
		}

		$.get("HolidayAjax.aspx?type=get", {id: id}, function(response) {
			eval("var holiday = " + response);
			$("#txtName").val(holiday.name);
			$("#txtStart").val(holiday.startDate);
			$("#txtEnd").val(holiday.endDate);
			$("#chkActive").attr("checked", holiday.active)
		});
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:70px;">节假日名称</td>
			<td><input type="text" id="txtName" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>起始日期</td>
			<td><input type="text" id="txtStart" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>结束日期</td>
			<td><input type="text" id="txtEnd" class="textbox" maxlength="10" /></td>
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
