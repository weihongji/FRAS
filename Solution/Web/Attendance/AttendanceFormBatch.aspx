<%@ Page Title="新增出勤记录" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttendanceFormBatch.aspx.cs" Inherits="Attendance_AttendanceFormBatch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	$.ajaxSettings.async = false;

	$(function () {
		$("#selDept").change(function () {
			$("#selUser,#selUserSelected").emptySelect(); // clear user list

			if ($("#selDept").val() == "-1") return;

			$.get("../System/UserAjax.aspx?type=brief_list", { deptId: $("#selDept").val() }, function (response) {
				eval("var users = " + response);
				$("#selUser").loadSelect(users);
			});
		});

		$("#btnRight").click(function () {
			$("#selUser option:selected").each(function () {
				$("#selUserSelected").addOption(this);
				$("#selUser").removeOption(this.value);
			});
			setButtonState();
		});

		$("#btnRightAll").click(function () {
			$("#selUser option").attr("selected", true);
			$("#btnRight").click();
		});

		$("#btnLeft").click(function () {
			$("#selUserSelected option:selected").each(function () {
				$("#selUser").addOption(this);
				$("#selUserSelected").removeOption(this.value);
			});
			setButtonState();
		});

		$("#btnLeftAll").click(function () {
			$("#selUserSelected option").attr("selected", true);
			$("#btnLeft").click();
		});

		$("#txtDate").datepicker($.datepicker.regional['zh-CN']);
		$("#txtDate").val(formatDate(new Date()));

		$("#btnSave").click(function () {
			if ($("#selDept").length > 0 && $("#selDept").val() < 0) {
				alert("请选择部门");
				$("#selDept").focus();
				return;
			}
			if ($("#selUserSelected option").length == 0) {
				alert("请选择员工");
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

			$.post("AttendanceAjax.aspx?type=batch_save"
				, { userIds: getSelectedUserIds()
					, date: $("#txtDate").val()
					, duration: $("#selDuration").val()
					, nightWork: $("#selType").val()
					, inWell: $("#chkInWell").attr("checked")
				}
				, function (response) {
					if (response == "true") {
						$("#btnBack").click();
					}
					else if (response && response.indexOf("EXIST|") == 0) {
						alert("用户" + response.substring(6) + "在" + $("#txtDate").val() + "已经有报工。\n请核对报工信息后，重新报工。");
					}
					else {
						alert("保存失败！" + response.substring(0, 100));
					}
				}
			);
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "AttendanceList.aspx" + qs;
		});

		if ($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
			$("#selDept").change();
		}
	});

	function setButtonState() {
		$("#btnSave").attr("disabled", $("#selUserSelected").attr("options").length == 0);
	}

	function getSelectedUserIds() {
		var arrId = new Array();
		$("#selUserSelected option").each(function () {
			arrId.push(this.value);
		});
		return arrId.join(",");
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:40px;">部门</td>
			<td><select id="selDept" class="w180">
				<option value="-1">-选择部门-</option>
				<asp:Repeater ID="repeaterDept" runat="server">
				<ItemTemplate>
				<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
				</ItemTemplate>
				</asp:Repeater>
			</select></td>
		</tr>
		<tr>
			<td valign="top">员工</td>
			<td>
				<table cellpadding="0" cellspacing="0">
					<tr>
						<td style="position:relative;">
							<select id="selUser" class="w180" multiple="multiple" size="15"></select>
							<img id="imgUser" style="position:absolute; top: 30px; left: 100px; display:none;" src="../Image/ajax-loader.gif" alt="" />
						</td>
						<td style="width:80px; text-align:center;">
							<div style="margin-top:10px;"><input type="button" class="button2" id="btnRight" value="&gt;" /></div>
							<div style="margin-top:20px;"><input type="button" class="button2" id="btnRightAll" value="&gt;&gt;" /></div>
							<div style="margin-top:20px;"><input type="button" class="button2" id="btnLeft" value="&lt;" /></div>
							<div style="margin-top:20px;"><input type="button" class="button2" id="btnLeftAll" value="&lt;&lt;" /></div>
						</td>
						<td>
							<select id="selUserSelected" class="w180" multiple="multiple" size="15"></select>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>日期</td>
			<td><input type="text" id="txtDate" class="textbox" maxlength="10" /></td>
		</tr>
		<tr>
			<td>工长</td>
			<td>
				<select id="selDuration" class="default">
					<option value="0">0</option>
					<option value="0.5" selected="selected">0.5</option>
					<option value="1.0">1.0</option>
					<option value="1.5">1.5</option>
				</select>
				<label style="display:inline-block; margin-left:40px;">夜班类型</label>
				<select id="selType" class="default">
					<option value="0">无</option>
					<option value="1">前夜</option>
					<option value="2">后夜</option>
					<option value="3">前&后夜</option>
				</select>
			</td>
		</tr>
		<tr>
			<td>&nbsp;</td>
			<td><label><input type="checkbox" id="chkInWell" checked="checked" class="p" style="margin: 10px 5px 0px 0px;" />是否入井</label></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 200px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" style="margin-left: 60px;" />
	</div>
</asp:Content>

