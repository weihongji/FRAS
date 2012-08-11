<%@ Page Title="手工报工记录导入" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttendanceImport.aspx.cs" Inherits="Attendance_AttendanceImport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<style type="text/css">
	#divMyContent {
		margin-left: 40px;
	}
	
	#divMyContent div {
		margin-top: 10px;
	}
</style>
<script type="text/javascript">
	$(function () {
		$("#btnBack").click(function () {
			var qs = removeQSItem(location.search, "id");
			location.href = "AttendanceList.aspx" + qs;
		});

		$("#btnImport").click(function () {
			var fileName = $("#<%=lblFileName.ClientID %>").html();
			if (!checkFileType(fileName)) {
				return;
			}
			$.post("AttendanceAjax.aspx?type=import", { name: fileName }, function (response) {
				if (isNumeric(response)) {
					alert("完毕，共导入" + response + "条记录。");
				}
				else if (response && response.indexOf("EXIST|") == 0) {
					alert("用户" + response.substring(6) + "有重复报工。\n请核对报工信息后，重新报工。");
				}
				else {
					alert("导入失败！");
				}
			});
		});

		setButtonState();
	});

	function setButtonState() {
		$("#btnImport").attr("disabled", $("#<%=lblFileName.ClientID %>").html() == "无");
	}

	function btnUpload_onclick() {
		var fileName = $("#<%=fileUpload1.ClientID %>").val();
		return checkFileType(fileName);
	}

	function checkFileType(fileName) {
		var ext = getFileExtension(fileName);
		if (ext != "xls") {
			var message = "无效的文件类型！";
			if (ext == "xlsx") {
				message += "请使用与Excel 97-2003相兼容的文件格式。";
			}
			alert(message);
			return false;
		}
		return true;
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<div id="divMyContent">
		<div>1. 上传报工记录 (Excel文件)</div>
		<div style="padding-left:20px;">
			<asp:FileUpload ID="fileUpload1" runat="server" />
			<asp:Button ID="btnUpload" Text="上传" CssClass="button4" OnClientClick="return btnUpload_onclick();" OnClick="btnUpload_Click" runat="server" />
		</div>
		<br />
		<div>2. 导入数据库</div>
		<div style="padding-left:20px;" id="divUploadedFile">
			已上传文件：<asp:Label ID="lblFileName" Text="无" runat="server" />
		</div>
		<br />
		<div>
			<input type="button" style="margin-left:80px;" class="button4" id="btnImport" value="导入" />
			<input type="button" style="margin-left:80px;" class="button4" id="btnBack" value="返回" />
		</div>
		<div style="margin-top:100px; padding-left:20px;">注：手工报工记录模板 <a href="../Import/Template/报工导入模板.xls">下载</a></div>
	</div>
</asp:Content>
