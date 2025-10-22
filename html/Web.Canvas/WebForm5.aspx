<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm5.aspx.cs" Inherits="Web.Canvas.WebForm5" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">

        function setupCanvas(canvas) {
            // Get the device pixel ratio, falling back to 1.
            var dpr = window.devicePixelRatio || 1;
            // Get the size of the canvas in CSS pixels.
            var rect = canvas.getBoundingClientRect();
            // Give the canvas pixel dimensions of their CSS
            // size * the device pixel ratio.
            canvas.width = rect.width * dpr;
            canvas.height = rect.height * dpr;
            var ctx = canvas.getContext('2d');
            // Scale all drawing operations by the dpr, so you
            // don't have to worry about the difference.
            ctx.scale(dpr, dpr);
            return ctx;
        }


        //cargar primero
        var image = document.createElement("img");
        image.setAttribute("src", "Images/blood64Black.png");
        
        window.onload = function() {
            // Now this line will be the same size on the page
            // but will look sharper on high-DPI devices!
            var ctx = setupCanvas(document.getElementById('myCanvas'));
            ctx.lineWidth = 5;
            ctx.beginPath();
            ctx.moveTo(100, 100);
            ctx.lineTo(200, 200);
            ctx.stroke();

           
            //ctx.drawImage(image, 0, 0, 10, 10, 0, 0, 10, 10);
            ctx.drawImage(image, 0, 0, 64, 64, 0, 0, 64, 64);
            document.documentElement.appendChild(image);
        };
    
    </script>

</head>
<body>
    <canvas id="myCanvas" style="border: 1px solid #000;"></canvas>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
