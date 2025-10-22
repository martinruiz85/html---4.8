<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template1.aspx.cs" Inherits="Web.Canvas.Template1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @font-face
        {
            font-family: "Roboto-Black";
            src: url(Font/Roboto-Black.ttf);
        }
        @font-face
        {
            font-family: "Roboto-Light";
            src: url(Font/Roboto-Light.ttf);
        }
    </style>
    <style type="text/css">
        html, body
        {
            width: 100%;
            height: 100%;
            overflow: hidden;
            min-height: 100%;
            margin: 0px 0px;
            padding: 0px;
            border: 0px;
        }
        canvas
        {
            background: #ffffff;
            position: relative;
            z-index: 1;
        }
    </style>

    <script type="text/javascript">


        var fps = 60;
        document.addEventListener("DOMContentLoaded", function(event) {

            var c = document.getElementById("myCanvas");
            var ctx = c.getContext("2d", { alpha: false });

            ctx.imageSmoothingEnabled = false;
            c.setAttribute("width", document.documentElement.offsetWidth);
            c.setAttribute("height", document.documentElement.offsetHeight);

            canvas_width = document.documentElement.offsetWidth;
            canvas_height = document.documentElement.offsetHeight;
            square_width = document.documentElement.offsetWidth / 12;
            square_height = square_width;

            fps = 1000 / 60;
            var timer = window.setInterval(function() {

                ctx.clearRect(0, 0, canvas_width, canvas_height);

            }, fps);

        });
    
    </script>

</head>
<body>
    <canvas id="myCanvas" style="border: 0px solid #000;" />
</body>
</html>
