<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Simple.Graph._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <img alt="porcent" src="../Handlers/GeneratorHandler.ashx?width=100&height=100&porcent=.5" />
        <img alt="Tick240" src="../Handlers/GeneratorTickHandler.ashx?width=250&height=250&porcent=0.21463414&ticks=240&color=8e44ad" />
        <img alt="LineTick240" src="../Handlers/GeneratorLineTickHandler.ashx?width=75&height=20&porcent=0.21463414&ticks=15&color=8e44ad" />
        
        </br>
        <img alt="Tick240" src="../Handlers/GeneratorTickHandler.ashx?width=250&height=250&porcent=0.3621&ticks=174&color=8e44ad" />
        <img alt="Tick240" src="../Handlers/GeneratorTickHandler.ashx?width=250&height=250&porcent=0&ticks=111&color=8e44ad" />
        <img alt="Tick240" src="../Handlers/GeneratorTickHandler.ashx?width=250&height=250&porcent=0&ticks=108&color=8e44ad" />
        <img alt="Tick240" src="../Handlers/GeneratorTickHandler.ashx?width=250&height=250&porcent=0&ticks=100&color=8e44ad" />
    </div>
    </form>
</body>
</html>
