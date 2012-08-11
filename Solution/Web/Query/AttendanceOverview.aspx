<%@ Page Title="考勤概况" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AttendanceOverview.aspx.cs" Inherits="Query_AttendanceOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
<link type="text/css" rel="stylesheet" href="../Style/jquery/ui.css" />
<style type="text/css">
	#MainContent_deptTree a {
		cursor: pointer;
	}
</style>
<script type="text/javascript" src="../Script/jquery-ui.js"></script>
<script type="text/javascript" src="../Script/jquery-ui.datepicker-zh-CN.js"></script>
<script type="text/javascript">
	var m_originalMargin;
	var m_originalWidth = 980 + 10; /* Reserve a margin of 10px at right */
	var m_lastWidth = m_originalWidth;

	$(function () {
		formatTree();

		$(window).resize(function () {
			var windownWidth = getWindowWidth();
			var offset = Math.max(windownWidth, m_originalWidth) - m_lastWidth;
			increaseContentWidth(offset);
			increaseLocalContent(offset);
			m_lastWidth += offset;
		}).resize();

		//启用菜单折叠功能
		//bindMenuCollapseEvents(resizeContentForCollapse);
		//collapseMenu();
	});

	function formatTree() {
		$(".MainContent_deptTree_0").each(function () {
			var arrText = $(this).html().split("|");
			if (arrText.length == 2) {
				$(this).replace("<a href='javascript:void(0);' onclick='dept_onclick(" + arrText[1] + ", \"" + arrText[0] + "\")'>" + arrText[0] + "</a>");
			}
		});
	}

	function resizeContentForCollapse(enlarge) {
		var changeAmount = 170;
		if (enlarge) {
			m_originalMargin = $("#right").css("margin-left");
			$("#right").css("margin-left", "20px");
			increaseContentWidth(changeAmount);
			increaseLocalContent(changeAmount);
		}
		else {
			$("#right").css("margin-left", m_originalMargin);
			increaseContentWidth(-1 * changeAmount);
			increaseLocalContent(-1 * changeAmount);
		}
	}

	function increaseLocalContent(amount) {
		if (!isNumeric(amount) || amount == 0) {
			return;
		}
		var frameWidth = parseInt($("#ifrContent").parent().css("width"));
		$("#ifrContent").parent().css("width", (frameWidth + amount).toString() + "px");
	}

	function dept_onclick(deptId, deptName) {
		document.getElementById("ifrContent").src = "AttendanceOverviewContent.aspx?deptId=" + deptId + "&deptName=" + deptName;
	}
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="title"><%=this.Title %></div>
	<div style="height:460px;">
		<div style="float:left; width:180px; height:100%; overflow:scroll;">
			<asp:TreeView ID="deptTree" LineImagesFolder="~/Image/TreeView" CssClass="treeCss" ShowLines="true" ShowExpandCollapse="true" EnableViewState="false" runat="server">
			</asp:TreeView>
		</div>
		<div style="float:left; width:560px; height:100%;">
			<iframe id="ifrContent" name="ifrContent" src="AttendanceOverviewContent.aspx" width="100%" height="100%" frameborder="1"></iframe>
		</div>
		<div style="clear:both;"></div>
	</div>
	<script type="text/javascript">
		//启用菜单折叠功能
		bindMenuCollapseEvents(resizeContentForCollapse);
		collapseMenu();
	</script>
</asp:Content>
