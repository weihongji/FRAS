<%@ Page Title="考勤汇总" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DailyAttendance.aspx.cs" Inherits="Query_DailyAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	$(function () {
		$("#btnSearch").click(function () {
			if (!checkCriteria()) {
				return;
			}

			var url = location.href;
			url = removeQSItem(url, "start");
			url = setQSItem(url, "start", $("#txtStart").val());
			url = setQSItem(url, "show", "1");
			location.href = url;
		});

		$("#txtStart").datepicker($.datepicker.regional['zh-CN']);

		$("#btnExport").click(function () {
			if (!checkCriteria()) {
				return;
			}

			$.ajax({
				  async: true
				, type: 'GET'
				, url: 'DurationAjax.aspx?type=export_daily'
				, data: { start: $("#txtStart").val() }
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
	});

	function checkCriteria() {
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
		return true;
	}

	function initValues() {
		var now = (new Date()).getFullYear() + "-" + ((new Date()).getMonth()+1) + "-" + (new Date()).getDate();
		$("#txtStart").val("<%=Request.QueryString["start"] %>");
		if (!isDate($("#txtStart").val())) {
			$("#txtStart").val(formatDate(now));
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<fieldset style="margin-bottom:10px;">
		<legend>检索</legend>
		<table style="width:100%;" cellpadding="3" cellspacing="0">
			<tr>
				<td style="width:80px;">考勤日期</td>
				<td><input type="text" id="txtStart" class="textbox" maxlength="10" /></td>
			</tr>
		</table>
	</fieldset>
	<div style="margin-bottom:10px;">
		<input type="button" class="button4" id="btnSearch" value="查询" />
		<input type="button" class="button4" id="btnExport" value="导出Excel" style="margin-left:20px;" />
		<span id="lblDownload" style="margin-left:20px;"></span>
		<img id="imgDownload" style="margin-left:10px; display:none;" src="../Image/ajax-loader.gif" alt="loading" />
	</div>
	<table class="list">
		<thead>
			<tr>
				<th style="width: 100px; background-image: url(../Image/table_bg03.gif);" rowspan="2">部门</th>
				<th style="width: 60px; background-image: url(../Image/table_bg03.gif);"  rowspan="2">在册<br />人数</th>
				<th colspan="2">8点班</th>
				<th colspan="2">4点班</th>
				<th colspan="2">0点班</th>
				<th colspan="2">白班</th>
				<th style="width: 60px; background-image: url(../Image/table_bg03.gif);"  rowspan="2">休假</th>
				<th style="width: 60px; background-image: url(../Image/table_bg03.gif);"  rowspan="2">请假</th>
				<th style="width: 60px; background-image: url(../Image/table_bg03.gif);"  rowspan="2">缺勤</th>
			</tr>
			<tr>
				<th style="width: 60px;">井下</th>
				<th style="width: 60px;">地面</th>
				<th style="width: 60px;">井下</th>
				<th style="width: 60px;">地面</th>
				<th style="width: 60px;">井下</th>
				<th style="width: 60px;">地面</th>
				<th style="width: 60px;">井下</th>
				<th style="width: 60px;">地面</th>
			</tr>
		</thead>
		<tbody>
			<asp:Repeater ID="repeaterDuration" EnableViewState="false" runat="server">
			<ItemTemplate>
			<tr>
				<td><%#Eval("DeptName")%></td>
				<td class="c"><%#Eval("UserCount")%></td>
				<td class="c"><%#Eval("C_8_W")%></td>
				<td class="c"><%#Eval("C_8_G")%></td>
				<td class="c"><%#Eval("C_4_W")%></td>
				<td class="c"><%#Eval("C_4_G")%></td>
				<td class="c"><%#Eval("C_0_W")%></td>
				<td class="c"><%#Eval("C_0_G")%></td>
				<td class="c"><%#Eval("C_D_W")%></td>
				<td class="c"><%#Eval("C_D_G")%></td>
				<td class="c"><%#Eval("XiuJia")%></td>
				<td class="c"><%#Eval("QingJia")%></td>
				<td class="c"><%#Eval("QueQin")%></td>
			</tr>
			</ItemTemplate>
			</asp:Repeater>
		</tbody>
	</table>
	<div style="margin-top:10px;">
		<asp:Literal ID="litRowCount" runat="server"></asp:Literal>
	</div>
</asp:Content>
