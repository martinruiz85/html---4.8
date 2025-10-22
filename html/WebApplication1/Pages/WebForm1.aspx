<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.Pages.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../js/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function() {

            $.ajax({
                type: "POST",
                url: "../WebService1.asmx/DoWork2",
                data: "{prms:'input'}",
                contentType: "application/json",
                datatype: "json",
                success: function(response) {
                    alert(response.d)
                },
                error: function(jqXHR, textStatus, errorThrown) {
                }
            });

        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
