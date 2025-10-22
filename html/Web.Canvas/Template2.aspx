<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template2.aspx.cs" Inherits="Web.Canvas.Template2" %>

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

        //global variables
        var fps = 60;
        var canvas_width, canvas_height, square_width, square_height;

        var Drawing = function(prms) {
            var $this = this;
            $this.x = prms.x || 0;
            $this.y = prms.y || 0;
            $this.w = prms.h || 0;
            $this.h = prms.h || 0;
            $this.draw = function(ctx) {
                ctx.fillStyle = "green";
                ctx.fillRect($this.x, $this.y, $this.w, $this.h);
            };
        };


        var EnemyStatus = {};
        EnemyStatus.Init = 0;

        var Enemy = function(prms) {
            var $this = this;
            Drawing.call(this, prms);
            $this.status = 0;

            $this.count_init = 0;
            $this.init = function() {

                $this.x += 1;
                $this.count_init++;
                if ($this.count_init > 59)
                    $this.count_init = 0;
            };

            $this.update = function() {
                if ($this.status == EnemyStatus.Init)
                    $this.init();               
            };
            $this.draw = function(ctx) {
                ctx.fillStyle = "red";
                ctx.fillRect($this.x, $this.y, $this.w, $this.h);
            };

            $this.update();
        };
        Enemy.prototype = new Drawing({});
        Enemy.prototype.constructor = Enemy;
        Enemy.Status = {};
        Enemy.Status.Init = 0;


        var SinglentonInicio = (function() {
            var singlenton;
            var Inicio = function() {
                var $this = this;
                var drawingObjects = [];
                $this.drawObjects = function(ctx) {
                    for (var i = 0; i < drawingObjects.length; i++) {
                        drawingObjects[i].update(ctx);
                        drawingObjects[i].draw(ctx);
                    }
                };
                $this.load = function() {

                    var rect = new Enemy({
                        "x": 0,
                        "y": 0,
                        "w": 10,
                        "h": 10
                    });
                    drawingObjects.push(rect);

                };
                $this.load();
            };

            return {
                GetInstance: function() {
                    if (!singlenton) singlenton = new Inicio();
                    return singlenton;
                }
            };

        })();

        window.addEventListener("resize", function() {

            canvas_width = document.documentElement.offsetWidth;
            canvas_height = document.documentElement.offsetHeight;
            square_width = document.documentElement.offsetWidth / 12;
            square_height = square_width;

        });


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

                // limpiar pantalla
                //ctx.fillStyle = "#ffffff";
                ctx.fillStyle = "#34495e";
                ctx.clearRect(0, 0, canvas_width, canvas_height);
                ctx.fillRect(0, 0, canvas_width, canvas_height);

                SinglentonInicio.GetInstance().drawObjects(ctx);

            }, fps);

        });
        
        
    
    </script>

</head>
<body>
    <canvas id="myCanvas" style="border: 0px solid #000;" />
</body>
</html>
