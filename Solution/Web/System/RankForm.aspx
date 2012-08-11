<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RankForm.aspx.cs" Inherits="System_RankForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<script type="text/javascript">
	var rankId = "<%=RankID %>";
	$.ajaxSettings.async = false;

	$(function () {
		$("#btnSave").click(function () {
			if (isEmpty($("#txtName").val())) {
				alert("请输入职位名称");
				$("#txtName").focus();
				return;
			}

			var pass = true;
			$.get("RankAjax.aspx?type=exist"
				, { id: $("#txtId").val(), name: $("#txtName").val() }
				, function (response) {
					if (response == "true") {
						alert("名称为\"" + $("#txtName").val() + "\"的职位已经存在！ 不能再使用这个名称。");
						$("#txtName").focus();
						pass = false;
					}
				}
			);
			if (!pass) {
				return;
			}

			$.post("RankAjax.aspx?type=save"
				, { id: $("#txtId").val(), name: $("#txtName").val() }
				, function (response) {
					$("#btnBack").click();
				}
			);
		});

		$("#btnBack").click(function () {
			location.href = "RankList.aspx";
		});

		setIdState();
		loadRank(rankId);
	});

	function loadRank(id) {
		if (id == null || id <= 0) {
			return;
		}

		$.get("RankAjax.aspx?type=get", {id: id}, function(response) {
			eval("var rank = " + response);
			$("#txtId").val(rank.id);
			$("#txtName").val(rank.name);
		});
	}

	function setIdState() {
		if (rankId <= 0) {
			$("#txtId").parent().parent().hide();
		}
		else {
			$("#txtId").attr("disabled", true);
		}
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=PageTitle%></div>
	<table style="margin-left: 40px;">
		<tr>
			<td style="width:80px;">职位编号</td>
			<td><input type="text" id="txtId" class="textbox" /></td>
		</tr>
		<tr>
			<td>职位名称</td>
			<td><input type="text" id="txtName" class="textbox" maxlength="50" /></td>
		</tr>
	</table>
	<div style="margin:10px 0px 0px 80px;">
		<input type="button" class="button2" id="btnSave" value="保存" />
		<input type="button" class="button2" id="btnBack" value="返回" />
	</div>
</asp:Content>
