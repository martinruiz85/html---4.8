<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormOneMain.aspx.cs"
    Inherits="Web.Canvas.WebFormOneMain" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta charset="utf-8" />
    <link rel="preload" as="font" href="Font/Roboto-Black.ttf" type="font/ttf" crossorigin="anonymous">
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

    <script type="text/javascript">


        // First, checks if it isn't implemented yet.
        if (!String.prototype.format) {
            String.prototype.format = function() {
                var args = arguments;
                return this.replace(/{(\d+)}/g, function(match, number) {
                    return typeof args[number] != 'undefined' ? args[number] : match;
                });
            };
        }

        var square = function(prms) {
            var $this = this;
            $this.point = {};
            $this.point.x = prms.x;
            $this.point.y = prms.y;
            $this.velocity = 0;
            $this.frame = 0;

            $this.update = function() {
                $this.velocity += 0.5;
                $this.point.x += $this.velocity;
            };

            $this.draw = function(ctx) {
                ctx.beginPath();
                ctx.lineWidth = "0";
                ctx.arc($this.point.x, $this.point.y, 10, 0, 2 * Math.PI, false);
                ctx.fillStyle = "rgb(231, 76, 60, 1)";
                ctx.fill();

                /*
                ctx.beginPath();
                ctx.fillStyle = "rgb(255, 255, 255, 1)";
                ctx.textAlign = "center";
                ctx.textBaseline = "middle";
                ctx.fillText("{0}".format(frame), 10, 20);
                ctx.fill();
                */
            }


        };

        var enemy = function(prms) {
            var $this = this;
            $this.point = {};
            $this.point.x = prms.x;
            $this.point.y = prms.y;
            $this.point.oldx = prms.x;
            $this.point.oldy = prms.y;

            $this.velocity = prms.velocity; //100px x secouds
            $this.seconds = prms.seconds; //100px x secouds

            $this.frame = 0;
            $this.status = 0;
            $this.update = function() {

                if ($this.status == 0) {
                    $this.waiting(3);
                }
                else if ($this.status == 1) {
                    $this.update_left();
                }
                else if ($this.status == 2) {
                    $this.update_bottom();
                }
                else if ($this.status == 3) {
                    $this.update_top();
                }
                else if ($this.status == 4) {
                    $this.update_right();
                }
                else if ($this.status == 5) {
                    $this.del();
                }

            };
            $this.del = function() {
            };
            $this.waiting = function(secunds) {
                var porcent = $this.frame / (fps * secunds)
                $this.frame++;
                if ($this.frame > (fps * secunds)) {
                    $this.frame = 0;
                    $this.status = 1
                }
            };
            $this.update_left = function() {
                $this.point.x = Math.max(0, $this.point.oldx + ($this.velocity * $this.frame) / (fps * $this.seconds));
                $this.frame++;
                if ($this.frame > (fps * $this.seconds)) {
                    $this.frame = 0;
                    $this.status = Math.floor(Math.random() * 4) + 1;
                    $this.point.oldx = $this.point.x;
                }
            };
            $this.update_right = function() {
                $this.point.x = Math.min(canvas_width, $this.point.oldx - ($this.velocity * $this.frame) / (fps * $this.seconds));
                $this.frame++;
                if ($this.frame > (fps * $this.seconds)) {
                    $this.frame = 0;
                    $this.status = Math.floor(Math.random() * 4) + 1;
                    $this.point.oldx = $this.point.x;
                }
            };
            $this.update_bottom = function() {
                $this.point.y = Math.min(canvas_height, $this.point.oldy + ($this.velocity * $this.frame) / (fps * $this.seconds));
                $this.frame++;
                if ($this.frame > (fps * $this.seconds)) {
                    $this.frame = 0;
                    $this.status = Math.floor(Math.random() * 4) + 1;
                    $this.point.oldy = $this.point.y;
                }
            };
            $this.update_top = function() {
                $this.point.y = Math.max(0, $this.point.oldy - ($this.velocity * $this.frame) / (fps * $this.seconds));
                $this.frame++;
                if ($this.frame > (fps * $this.seconds)) {
                    $this.frame = 0;
                    $this.status = Math.floor(Math.random() * 4) + 1;
                    $this.point.oldy = $this.point.y;
                }
            };

            $this.count = 0;
            $this.draw = function(ctx) {
                ctx.beginPath();
                ctx.lineWidth = "0";
                ctx.arc($this.point.x, $this.point.y, 10, 0, 2 * Math.PI, false);
                ctx.fillStyle = "rgb(231, 76, 60, 1)";
                ctx.fill();

                ctx.beginPath();
                ctx.fillStyle = "rgb(255, 255, 255, 1)";
                ctx.textAlign = "center";
                ctx.textBaseline = "middle";
                ctx.fillText("{0}".format(frame), 10, 20);
                ctx.fill();
            }
        }
        enemy.status = {};
        enemy.status.left = 1;
        enemy.status.top = 2;
        enemy.status.right = 3;
        enemy.status.bottom = 4;


        var DrawObjects = [];

        var e1 = new enemy({ "x": 100, "y": 100, "velocity": 10, "seconds": .10 });
        var e2 = new enemy({ "x": 120, "y": 100, "velocity": 10, "seconds": .10 });
        var e3 = new enemy({ "x": 140, "y": 100, "velocity": 10, "seconds": .10 });
        var e4 = new enemy({ "x": 160, "y": 100, "velocity": 10, "seconds": .10 });
        var e5 = new enemy({ "x": 180, "y": 100, "velocity": 10, "seconds": .10 });
        var e6 = new enemy({ "x": 200, "y": 100, "velocity": 10, "seconds": .10 });
        var e7 = new enemy({ "x": 220, "y": 100, "velocity": 10, "seconds": .10 });
        var e8 = new enemy({ "x": 240, "y": 100, "velocity": 10, "seconds": .10 });

        DrawObjects.push(e1);
        DrawObjects.push(e2);
        DrawObjects.push(e3);
        DrawObjects.push(e4);
        DrawObjects.push(e5);
        DrawObjects.push(e6);
        DrawObjects.push(e7);
        DrawObjects.push(e8);

        var s = new square({ "x": 100, "y": 200 });
        //var e2 = new enemy({ "x": 10, "y": 40, "velocity": 200, "seconds": 1 });
        //var e3 = new enemy({ "x": 10, "y": 60, "velocity": 100, "seconds": .5 });


        var fps = 60;
        var frame = 0;

        document.addEventListener("DOMContentLoaded", function(event) {

            var c = document.getElementById("myCanvas");
            var ctx = c.getContext("2d", { alpha: false });

            document.addEventListener("mousedown", function(event) {
                for (var i = 0; i < DrawObjects.length; i++) {
                    var c = DrawObjects[i];
                    var circle = new Path2D();
                    circle.arc(c.point.x, c.point.y, 10, 0, 2 * Math.PI, false);
                    if (ctx.isPointInPath(circle, event.offsetX, event.offsetY)) {
                        DrawObjects.splice(i, 1);
                    }
                }
            });

            ctx.imageSmoothingEnabled = false;
            //ctx.translate(.5, .5);
            c.setAttribute("width", document.documentElement.offsetWidth);
            c.setAttribute("height", document.documentElement.offsetHeight);

            canvas_width = document.documentElement.offsetWidth;
            canvas_height = document.documentElement.offsetHeight;
            square_width = document.documentElement.offsetWidth / 12;

            var segundos = 0;
            var timer2 = window.setInterval(function() {
                segundos++;
            }, 1000);


            var timer = window.setInterval(function() {
                ctx.clearRect(0, 0, canvas_width, canvas_height);

                for (var i = 0; i < DrawObjects.length; i++) {
                    DrawObjects[i].update();
                    DrawObjects[i].draw(ctx);

                    ctx.beginPath();
                    ctx.moveTo(DrawObjects[i].point.x, DrawObjects[i].point.y);
                    if (i + 1 < DrawObjects.length)
                        ctx.lineTo(DrawObjects[i + 1].point.x, DrawObjects[i + 1].point.y);
                    ctx.strokeStyle = "red";
                    ctx.stroke();
                }

                //e1.update();
                //e1.draw(ctx);

                //s.update();
                //s.draw(ctx);

                //e2.update();
                //e2.draw(ctx);

                //                e3.update();
                //                e3.draw(ctx);


                /*
                ctx.beginPath();
                ctx.fillStyle = "rgb(255, 255, 255, 1)";
                ctx.textAlign = "center";
                ctx.textBaseline = "middle";
                ctx.fillText("{0} segundos".format(segundos), 400, 40);
                ctx.fill();
                */

                frame++;
                if (frame > 59)
                    frame = 0;

            }, 1000 / fps);

        });
    
    </script>

</head>
<body>
    <canvas id="myCanvas" style="border: 0px solid #000;"></canvas>
</body>
</html>
