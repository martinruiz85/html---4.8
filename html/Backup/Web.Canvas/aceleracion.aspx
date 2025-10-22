<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="aceleracion.aspx.cs" Inherits="Web.Canvas.aceleracion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        @font-face
        {
            font-family: "your_custom_font";
            src: url(Font/Roboto-Black.ttf);
        }
        @font-face
        {
            font-family: "your_custom_font2";
            src: url(Font/Roboto-Light.ttf);
        }
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

    <script type="text/javascript" language="javascript">

        function getRndInteger(min, max) {
            return Math.floor(Math.random() * (max - min)) + min;
        };


        var Rect = function(prms) {
            var $this = this;
            this.x = prms.x | 0;
            this.y = prms.y | 0;

            this.speed = 0.05;
            this.speedX = 0;
            this.speedY = 0;
            this.gravity = 0.00; //0.05;
            this.gravitySpeed = 0;

            this.acelerate = function() {
                this.speed = 0.05;
                this.speedX = Math.max(0, this.speedX + this.speed);
            };
            this.desacelerate = function() {

                var time = window.setInterval(function() {
                    $this.speed = -0.05;
                    $this.speedX = Math.max(0, $this.speedX + $this.speed);
                    if ($this.speedX == 0)
                        clearInterval(time);
                }, 1000 / 60);

            };


            this.update = function() {

                this.gravitySpeed += this.gravity;
                this.x += this.speedX;
                this.y += this.speedY + this.gravitySpeed;
            };

            this.draw = function(ctx) {
                ctx.beginPath();
                ctx.rect($this.x, $this.y, 10, 10);
                ctx.stroke();
            };
        };

        var r = new Rect({
            "x": 0,
            "y": 100
        });

        document.addEventListener('keypress', function(event) {
            var keyName = event.key;
            //alert('keydown event\n\n' + 'key: ' + keyName);

            //if (keyName == "ArrowRight")
            r.acelerate();
            //else if (keyName == "ArrowLeft")
            //    r.desacelerate();
        });

        document.addEventListener('keyup', function(event) {
            var keyName = event.key;
            //alert('keydown event\n\n' + 'key: ' + keyName);

            //if (keyName == "ArrowRight")
            //r.acelerate();
            //else if (keyName == "ArrowLeft")
            r.desacelerate();
        });


        document.addEventListener("DOMContentLoaded", function(event) {

            var c = document.getElementById("myCanvas");
            var ctx = c.getContext("2d", { alpha: false });

            ctx.imageSmoothingEnabled = false;
            //ctx.translate(.5, .5);
            c.setAttribute("width", document.documentElement.offsetWidth);
            c.setAttribute("height", document.documentElement.offsetHeight);

            canvas_width = document.documentElement.offsetWidth;
            canvas_height = document.documentElement.offsetHeight;
            square_width = document.documentElement.offsetWidth / 12;
            square_height = square_width;

            //ctx.scale(dpr, dpr);
            fps = 1000 / 60;
            var timer = window.setInterval(function() {

                ctx.clearRect(0, 0, canvas_width, canvas_height);
                ctx.fillStyle = "#ffffff";
                ctx.fillRect(0, 0, canvas_width, canvas_height)
                r.update();
                r.draw(ctx);

            }, fps);

        });
        
    </script>

</head>
<body>
    <canvas id="myCanvas" style="border: 0px solid #000;"></canvas>
</body>
</html>
