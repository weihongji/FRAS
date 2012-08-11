<%@ Page Title="考勤报表" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttendanceReport.aspx.cs" Inherits="Query_AttendanceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	$(function () {
		$("#chkMonthly").click(function () {
			$("#trMonth,#trDate").toggle();
		});

		$("#btnSearch").click(function () {
			if (!checkCriteria()) {
				return;
			}

			var url = location.href;
			url = removeQSItem(url, "dept");
			url = removeQSItem(url, "device");
			url = removeQSItem(url, "usertype");
			url = removeQSItem(url, "start");
			url = removeQSItem(url, "end");
			if ($("#selDept").val() >= 0) {
				url = setQSItem(url, "dept", $("#selDept").val());
			}
			if ($("#selDevice").val() > 0) {
				url = setQSItem(url, "device", $("#selDevice").val());
			}
			if ($("#selUserType").val() >= 0) {
				url = setQSItem(url, "usertype", $("#selUserType").val());
			}
			if ($("#txtStart:visible").length > 0) {
				url = setQSItem(url, "start", $("#txtStart").val());
				url = setQSItem(url, "end", $("#txtEnd").val());
			}
			else {
				url = setQSItem(url, "year", $("#selYear").val());
				url = setQSItem(url, "month", $("#selMonth").val());
			}
			url = setQSItem(url, "show", "1");
			location.href = url;
		});

		$("#txtStart").datepicker($.datepicker.regional['zh-CN']);
		$("#txtEnd").datepicker($.datepicker.regional['zh-CN']);

		$("#btnExport").click(function () {
			if (!checkCriteria()) {
				return;
			}

			$.ajax({
				  async: true
				, type: 'GET'
				, url: 'AttendanceAjax.aspx?type=export'
				, data: {
					  dept: $("#selDept").val()
					, device: $("#selDevice").val()
					, usertype: $("#selUserType").val()
					, start: ($("#txtStart:visible").length > 0 ? $("#txtStart").val() : "")
					, end: ($("#txtEnd:visible").length > 0 ? $("#txtEnd").val() : "")
					, year: ($("#selYear:visible").length > 0 ? $("#selYear").val() : "")
					, month: ($("#selMonth:visible").length > 0 ? $("#selMonth").val() : "")
				}
				, beforeSend: function() {
					$("#lblDownload").html("正在生成Excel文件……");
					$("#imgDownload").show();
				}
				, success: function(response) {
					if (response && response.indexOf(".xls")>0) {
						$("#lblDownload").html("<a href='../Export/" + response + "'>" + response + "</a>");
					}
					else {
						if (response.length>100) {
							var msg = "生成Excel文件时报错！";
						}
						else {
							var msg = "生成Excel文件时报错：" + response;
						}
						alert(msg);
						$("#lblDownload").html("<span style='color:red; font-weight:bold;'>" + msg + "</span>");
					}
				}
				, complete: function() {
					$("#imgDownload").hide();
				}
			});
		});

		initValues();
		increaseContentWidth(300);
		setFrameWidth(document.width);
	});

	function checkCriteria() {
		if ($("#txtStart:visible").length > 0) {
			if (isEmpty($("#txtStart").val())) {
				alert("请输入起始日期");
				$("#txtStart").focus();
				return false;
			}
			else if (!isDate($("#txtStart").val())) {
				alert("起始日期无效");
				$("#txtStart").focus();
				return false;
			}
			if (isEmpty($("#txtEnd").val())) {
				alert("请输入结束日期");
				$("#txtEnd").focus();
				return false;
			}
			else if (!isDate($("#txtEnd").val())) {
				alert("结束日期无效");
				$("#txtEnd").focus();
				return false;
			}
			if (new Date($("#txtStart").val())>new Date($("#txtEnd").val())) {
				alert("起始时间不得小于终止时间");
				$("#txtEnd").focus();
				return false;
			}
		}
		return true;
	}

	function initValues() {
		if($("#selDept option").length == 2) {
			$("#selDept option:first").remove();
		}
		else {
			$("#selDept").val("<%=GetQSInteger("dept", -1) %>");
		}
		$("#selDevice").val("<%=GetQSInteger("device") %>");
		$("#selUserType").val("<%=GetQSInteger("usertype", -1) %>");
		var now = (new Date()).getFullYear() + "-" + ((new Date()).getMonth()+1) + "-" + (new Date()).getDate();
		$("#txtStart").val("<%=Request.QueryString["start"] %>");
		if (!isDate($("#txtStart").val())) {
			$("#txtStart").val(formatDate(now));
		}
		$("#txtEnd").val("<%=Request.QueryString["end"] %>");
		if (!isDate($("#txtEnd").val())) {
			$("#txtEnd").val(formatDate(now));
		}
		initYearMonth();
		var year = "<%=Request.QueryString["year"] %>";
		if (year.length>0 && year>2000) {
			$("#selYear").val(year);
			$("#selMonth").val("<%=Request.QueryString["month"] %>");
		}
		else {
			$("#selMonth").val((new Date()).getMonth()); // Select previous month by default.
		}
		if ("<%=Request.QueryString["start"] %>".length>0) {
			$("#chkMonthly").click();
		}
	}

	function initYearMonth() {
		var maxYear = (new Date()).getFullYear();
		for(var i=maxYear; i>maxYear-10; i--) {
			$("#selYear").addOption(i, i);
		}

		for(var i=1; i<=12; i++) {
			$("#selMonth").addOption(i, i.toString() + "月");
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<fieldset style="margin-bottom:10px; width:720px;">
		<legend>检索</legend>
		<table style="width:100%;" cellpadding="3" cellspacing="0">
			<tr>
				<td style="width:120px;">部门</td>
				<td style="width:160px;"><select id="selDept" class="default">
					<option value="-1">-全部-</option>
					<asp:Repeater ID="repeaterDept" runat="server">
					<ItemTemplate>
					<option value="<%#Eval("ID") %>"><%#Eval("Name") %></option>
					</ItemTemplate>
					</asp:Repeater>
				</select></td>
				<td style="width:120px;">&nbsp;</td>
				<td>&nbsp;</td>
			</tr>
			<tr>
				<td>考勤设备</td>
				<td><select id="selDevice" class="default">
					<option value="0">-全部-</option>
					<option value="1">井口3号通道</option>
					<option value="2">井口2号通道</option>
					<option value="3">井口4号通道</option>
					<option value="4">井口1号通道</option>
					<option value="5">联建楼</option>
					<option value="6">办公楼</option>
					<option value="7">生活区</option>
					<option value="8">井口5号通道</option>
				</select></td>
				<td>员工类别</td>
				<td><select id="selUserType" class="default">
					<option value="-1">-全部-</option>
					<option value="0">正式工</option>
					<option value="1">劳务工</option>
					<option value="2">中煤中宇</option>
				</select></td>
			</tr>
			<tr>
				<td colspan="4"><label><input type="checkbox" id="chkMonthly" checked="checked" />按月份生成报表</label></td>
			</tr>
			<tr id="trMonth">
				<td>报表月份</td>
				<td colspan="3">
					<select id="selYear"></select>
					<select id="selMonth"></select>
				</td>
			</tr>
			<tr id="trDate" style="display:none;">
				<td>起始日期</td>
				<td><input type="text" id="txtStart" class="textbox" maxlength="10" /></td>
				<td>结束日期</td>
				<td><input type="text" id="txtEnd" class="textbox" maxlength="10" /></td>
			</tr>
		</table>
	</fieldset>
	<div style="margin-bottom:10px;">
		<input type="button" class="button4" id="btnSearch" value="查询" />
		<input type="button" class="button4" id="btnExport" value="导出Excel" style="margin-left:20px;" />
		<span id="lblDownload" style="margin-left:20px;"></span>
		<img id="imgDownload" style="margin-left:10px; display:none;" src="../Image/ajax-loader.gif" alt="loading" />
	</div>
	<table class="list two_line_header">
		<thead>
			<tr>
				<th style="width: 100px;">部门</th>
				<th style="width: 60px;">工号</th>
				<th style="width: 70px;">姓名</th>
				<th style="width: 50px;">应出勤</th>
				<th style="width: 60px;">日常<br/>出勤</th>
				<th style="width: 40px;">迟到</th>
				<th style="width: 40px;">早退</th>
				<th style="width: 40px;">旷工</th>
				<th style="width: 60px;">日常<br/>加班</th>
				<th style="width: 50px;">节假日<br/>加班</th>
				<th style="width: 40px;">前夜</th>
				<th style="width: 40px;">后夜</th>
				<th style="width: 40px;">入井</th>
				<th style="width: 40px;">休假</th>
				<th style="width: 40px;">事假</th>
				<th style="width: 40px;">病假</th>
				<th style="width: 40px;">工伤</th>
				<th style="width: 40px;">年休</th>
				<th style="width: 40px;">婚假</th>
				<th style="width: 40px;">产假</th>
				<th style="width: 40px;">丧假</th>
				<th style="width: 50px;">探亲假</th>
				<th style="width: 40px;">出差</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterAttendance" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td class="c"><%#Eval("deptName")%></td>
				<td class="c"><%#Eval("userId")%></td>
				<td class="c"><%#Eval("userName")%></td>
				<td class="c"><%#Eval("must")%></td>
				<td class="c"><%#Eval("normal")%></td>
				<td class="c"><%#Eval("late")%></td>
				<td class="c"><%#Eval("quit")%></td>
				<td class="c"><%#Eval("kuang")%></td>
				<td class="c"><%#Eval("overtime_non_holiday")%></td>
				<td class="c"><%#Eval("overtime_holiday")%></td>
				<td class="c"><%#Eval("front")%></td>
				<td class="c"><%#Eval("back")%></td>
				<td class="c"><%#Eval("well")%></td>
				<td class="c"><%#Eval("leave1")%></td>
				<td class="c"><%#Eval("leave2")%></td>
				<td class="c"><%#Eval("leave3")%></td>
				<td class="c"><%#Eval("leave4")%></td>
				<td class="c"><%#Eval("leave5")%></td>
				<td class="c"><%#Eval("leave6")%></td>
				<td class="c"><%#Eval("leave7")%></td>
				<td class="c"><%#Eval("leave8")%></td>
				<td class="c"><%#Eval("leave9")%></td>
				<td class="c"><%#Eval("leave10")%></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
	<div style="margin-top:10px;">
		<asp:Literal ID="litRowCount" runat="server"></asp:Literal>
	</div>
</asp:Content>
