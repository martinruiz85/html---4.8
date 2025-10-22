<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Porcent.aspx.cs" Inherits="Web.Canvas.Porcent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript">

        window.onload = function() {
            var numerador = document.getElementById("Text1");
            var denominador = document.getElementById("Text2");
            var resultado = document.getElementById("resultado");

            var canvas = document.getElementById("myCanvas");
            var c = document.getElementById("myCanvas");
            var ctx = c.getContext("2d", { alpha: false });

            var width = 500;

            window.setInterval(function() {

                var porcent = parseFloat(numerador.value) / parseFloat(denominador.value);

                ctx.clearRect(0, 0, canvas.width, canvas.height);

                ctx.beginPath();
                ctx.rect(0, 0, width, 100);
                ctx.stroke();


                //ctx.font = "20px Georgia";
                ctx.textAlign = "center";
                ctx.textBaseline = "middle";
                ctx.fillStyle = "#000000";

                for (var i = 0; i < denominador.value; i++) {

                    ctx.beginPath();
                    ctx.rect((width / denominador.value * i), 0, width / denominador.value, 100);
                    ctx.fillText(i + 1, (width / denominador.value * i) + (width / denominador.value) / 2, 0 + (100 / 2));
                    ctx.stroke();
                }

                ctx.fillStyle = "#000000";
                ctx.fillRect(0, 0, width * porcent, 100);
                ctx.fillStyle = "#ffffff";
                ctx.fillText(" ", 0 + (width * porcent / 2), 0 + (100 / 2));

                resultado.innerHTML = porcent.toString();

            }, 10);
        };
    
    </script>

</head>
<body>
    <table>
        <tr>
            <td>
                <canvas id="myCanvas" style="border: 1px solid #fff;" width="1020" height="120"></canvas>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td align="center">
                            <input id="Text1" type="text" style="width: 40px;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <hr style="border: 4px solid #000;" />
                        </td>
                        <td align="center">
                            <div id="resultado"><div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <input id="Text2" type="text" style="width: 40px;" />
                        </td>
                    </tr>                    
                </table>
            </td>
        </tr>
    </table>
</body>
</html>
