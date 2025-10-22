<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartAP.aspx.cs" Inherits="WebApplication1.ChartAP" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 50%;">
            <tr>
                <td align="right">
                    Periodo planif.:
                </td>
                <td>
                    <asp:Label ID="lblPeriodCode" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Programa de Evaluaci&#243;&n:
                </td>
                <td class="style1">
                    <asp:Label ID="lblProgramTitle" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:Image ID="Image1" ImageUrl="~/ChartAP.ashx" runat="server" />
    </div>
    </form>
</body>
</html>
