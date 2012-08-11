<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RosteringForm.aspx.cs" Inherits="Attendance_RosteringForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style type="text/css">
	input[type=text].time {
		width: 60px;
	}
	
	input[type=text].buffer {
		width: 30px;
	}
	
	select.roster_select {
		width: 100px;
	}
</style>

<script type="text/javascript">
	var rosteringId = "<%=GetQSInteger("id") %>";
	var rosteringType = "<%=GetQSInteger("type") %>";
	$.ajaxSettings.async = false;

	$(function () {
		if (rosteringType == 1) {
			$("#divTwo").remove();
		}
		else if (rosteringType == 2) {
			$("#divOne").remove();
		}
		else {
			$("#divOne,#divTwo").remove();
		}

		$("#btnSave").click(function () {
			if (isEmpty($("#txtName").val())) {
				alert("请输入班次名称");
				$("#txtName").focus();
				return;
			}
			//上班时间
			if (isEmpty($("#txtStart:eq(0)").val())) {
				alert("请输入上班时间");
				$("#txtStart:eq(0)").focus();
				return;
			}
			else if (!isTime($("#txtStart:eq(0)").val())) {
				alert("上班时间无效");
				$("#txtStart:eq(0)").focus();
				return;
			}
			if ($("#divTwo #txtStart:eq(1)").length>0) {
				if (isEmpty($("#divTwo #txtStart:eq(1)").val())) {
					alert("请输入上班时间");
					$("#divTwo #txtStart:eq(1)").focus();
					return;
				}
				else if (!isTime($("#divTwo #txtStart:eq(1)").val())) {
					alert("上班时间无效");
					$("#divTwo #txtStart:eq(1)").focus();
					return;
				}
			}
			if (isEmpty($("#txtBeforeStart:eq(0)").val())) {
				$("#txtBeforeStart:eq(0)").val("0");
			}
			if ($("#divTwo #txtBeforeStart:eq(1)").length>0) {
				if (isEmpty($("#divTwo #txtBeforeStart:eq(1)").val())) {
					$("#divTwo #txtBeforeStart:eq(1)").val("0");
				}
			}
			//下班时间
			if (isEmpty($("#txtEnd:eq(0)").val())) {
				alert("请输入下班时间");
				$("#txtEnd:eq(0)").focus();
				return;
			}
			else if (!isTime($("#txtEnd:eq(0)").val())) {
				alert("下班时间无效");
				$("#txtEnd:eq(0)").focus();
				return;
			}
			if ($("#divTwo #txtEnd:eq(1)").length>0) {
				if (isEmpty($("#divTwo #txtEnd:eq(1)").val())) {
					alert("请输入下班时间");
					$("#divTwo #txtEnd:eq(1)").focus();
					return;
				}
				else if (!isTime($("#divTwo #txtEnd:eq(1)").val())) {
					alert("下班时间无效");
					$("#divTwo #txtEnd:eq(1)").focus();
					return;
				}
			}
			if (isEmpty($("#txtAfterEnd:eq(0)").val())) {
				$("#txtAfterEnd:eq(0)").val("0");
			}
			if ($("#divTwo #txtAfterEnd:eq(1)").length>0) {
				if (isEmpty($("#divTwo #txtAfterEnd:eq(1)").val())) {
					$("#divTwo #txtAfterEnd:eq(1)").val("0");
				}
			}

			$.post("RosteringAjax.aspx?type=save"
				, { id: rosteringId
					, name: $("#txtName").val()
					, startTime: $("#txtStart").val()
					, earlyRange: $("#txtBeforeStart").val()
					, endTime: $("#txtEnd").val()
					, lateRange: $("#txtAfterEnd").val()
					, nightWork: $("#selType").val()
					, duration: $("#selDuration").val()
					, active: $("#chkActive").attr("checked")

					, startTime2: $("#divTwo #txtStart:eq(1)").val()
					, earlyRange2: $("#divTwo #txtBeforeStart:eq(1)").val()
					, endTime2: $("#divTwo #txtEnd:eq(1)").val()
					, lateRange2: $("#divTwo #txtAfterEnd:eq(1)").val()
					, duration2: $("#divTwo #selDuration:eq(1)").val()
				}
				, function (response) {
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "RosteringList.aspx" + qs;
		});

		loadRostering(rosteringId);
	});

	function loadRostering(id) {
		if (id == null || id == 0) {
			return;
		}

		$.get("RosteringAjax.aspx?type=get", {id: id}, function(response) {
			eval("var rostering = " + response);
			$("#txtName").val(rostering.name);
			$("#txtStart:eq(0)").val(rostering.startTime);
			$("#txtBeforeStart:eq(0)").val(rostering.earlyRange);
			$("#txtEnd:eq(0)").val(rostering.endTime);
			$("#txtAfterEnd:eq(0)").val(rostering.lateRange);
			$("#selType:eq(0)").val(rostering.nightWork);
			$("#selDuration:eq(0)").val(rostering.duration);
			$("#chkActive").attr("checked", rostering.active);
			if (rostering.nextID>0) {
				$.get("RosteringAjax.aspx?type=get", {id: rostering.nextID}, function(response) {
					eval("var rostering = " + response);
					$("#divTwo #txtStart:eq(1)").val(rostering.startTime);
					$("#divTwo #txtBeforeStart:eq(1)").val(rostering.earlyRange);
					$("#divTwo #txtEnd:eq(1)").val(rostering.endTime);
					$("#divTwo #txtAfterEnd:eq(1)").val(rostering.lateRange);
					$("#divTwo #selDuration:eq(1)").val(rostering.duration);
				});
			}
		});
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<div style="margin-left: 40px;">
		<div style="padding:3px; margin-bottom:10px;">
			<label style="width:60px; display:inline-block;">班次名称</label>
			<input type="text" id="txtName" style="width:160px;" maxlength="10" />
		</div>
		<div id="divOne">
			<table cellpadding="3" cellspacing="0">
				<tr>
					<td style="width:60px;">上班时间</td>
					<td style="width:200px;"><input type="text" id="txtStart" class="time" maxlength="5" /></td>
					<td style="width:60px;">可提前</td>
					<td><input type="text" id="txtBeforeStart" class="buffer" maxlength="4" value="0" /> 分钟签到</td>
				</tr>
				<tr>
					<td>下班时间</td>
					<td><input type="text" id="txtEnd" class="time" maxlength="5" /></td>
					<td>可延点</td>
					<td><input type="text" id="txtAfterEnd" class="buffer" maxlength="4" value="0" /> 分钟签退</td>
				</tr>
				<tr>
					<td>夜班类型</td>
					<td><select id="selType" class="roster_select">
						<option value="0">无</option>
						<option value="1">前夜</option>
						<option value="2">后夜</option>
						<option value="3">前&后夜</option>
					</select></td>
					<td>该段计时</td>
					<td><select id="selDuration" class="roster_select">
						<option value="0">0</option>
						<option value="0.5">0.5</option>
						<option value="1.0" selected="selected">1.0</option>
						<option value="1.5">1.5</option>
					</select></td>
				</tr>
			</table>
		</div>
		<div id="divTwo">
			<fieldset>
				<legend>上午</legend>
				<table cellpadding="3" cellspacing="0">
					<tr>
						<td style="width:60px;">上班时间</td>
						<td style="width:200px;"><input type="text" id="txtStart" class="time" maxlength="5" /></td>
						<td style="width:60px;">可提前</td>
						<td><input type="text" id="txtBeforeStart" class="buffer" maxlength="4" value="0" /> 分钟签到</td>
					</tr>
					<tr>
						<td>下班时间</td>
						<td><input type="text" id="txtEnd" class="time" maxlength="5" /></td>
						<td>可延点</td>
						<td><input type="text" id="txtAfterEnd" class="buffer" maxlength="4" value="0" /> 分钟签退</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>该段计时</td>
						<td><select id="selDuration" class="roster_select">
							<option value="0">0</option>
							<option value="0.5" selected="selected">0.5</option>
							<option value="1.0">1.0</option>
							<option value="1.5">1.5</option>
						</select></td>
					</tr>
				</table>
			</fieldset>
			<br />
			<fieldset>
				<legend>下午</legend>
				<table cellpadding="3" cellspacing="0">
					<tr>
						<td style="width:60px;">上班时间</td>
						<td style="width:200px;"><input type="text" id="txtStart" class="time" maxlength="5" /></td>
						<td style="width:60px;">可提前</td>
						<td><input type="text" id="txtBeforeStart" class="buffer" maxlength="4" value="0" /> 分钟签到</td>
					</tr>
					<tr>
						<td>下班时间</td>
						<td><input type="text" id="txtEnd" class="time" maxlength="5" /></td>
						<td>可延点</td>
						<td><input type="text" id="txtAfterEnd" class="buffer" maxlength="4" value="0" /> 分钟签退</td>
					</tr>
					<tr>
						<td>&nbsp;</td>
						<td>&nbsp;</td>
						<td>该段计时</td>
						<td><select id="selDuration" class="roster_select">
							<option value="0">0</option>
							<option value="0.5" selected="selected">0.5</option>
							<option value="1.0">1.0</option>
							<option value="1.5">1.5</option>
						</select></td>
					</tr>
				</table>
			</fieldset>
		</div>
		<div style="padding:3px; margin-top:5px;">
			<label><input type="checkbox" id="chkActive" checked="checked" class="p" style="margin: 10px 5px 0px 0px;" />是否有效</label>
		</div>

		<div style="margin:10px 0px 0px 180px;">
			<input type="button" class="button2" id="btnSave" value="保存" />
			<input type="button" class="button2" id="btnBack" value="返回" style="margin-left:100px;" />
		</div>
	</div>
</asp:Content>
