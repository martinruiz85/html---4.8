<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChartAP2.aspx.cs" Inherits="WebApplication1.ChartAP2" uiCulture="es-MX" Culture="es-MX" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        body
        {
            font-family: "Trebuchet MS" , Tahoma, Arial, Verdana, Helvetica;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width: 700px;">
            <tr>
                <td align="right" style="width: 250px; font-weight: bold;">
                    Periodo planif.:
                </td>
                <td>
                    <asp:Label ID="lblPeriodCode" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 250px; font-weight: bold;">
                    Programa de Evaluaci&#243;n:
                </td>
                <td class="style1">
                    <asp:Label ID="lblProgramTitle" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table> 
        <%
            Response.Write(System.Threading.Thread.CurrentThread.CurrentCulture);
         %>       
        <asp:Chart ID="chart1" runat="server">
        </asp:Chart>
    </div>
    </form>
</body>
</html>
