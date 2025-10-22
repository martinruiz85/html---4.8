<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tuto1.aspx.cs" Inherits="Web.Canvas.tuto1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body
        {
            margin: 0;
            padding: 0;
            height: 100%;
            width: 100%;
        }
        canvas
        {
            background: #fff;
            height: 100%;
            width: 100%;
            display: block;
        }
    </style>

    <script type="text/javascript" language="javascript">
        //  https://www.sitepoint.com/quick-tip-game-loop-in-javascript/

        function update(progress) {
            // Update the state of the world for the elapsed time since last render
            s.update();
        }

        function draw() {
            // Draw the state of the world

            var c = document.getElementById("canvas");
            var ctx = c.getContext("2d", { alpha: false });
            ctx.imageSmoothingEnabled = false;
            c.setAttribute("width", document.documentElement.offsetWidth);
            c.setAttribute("height", document.documentElement.offsetHeight);

            canvas_width = document.documentElement.offsetWidth;
            canvas_height = document.documentElement.offsetHeight;
            square_width = document.documentElement.offsetWidth / 12;

            ctx.clearRect(0, 0, canvas_width, canvas_height);
            ctx.fillStyle = "#ffffff";
            ctx.fillRect(0, 0, canvas_width, canvas_height)

            s.draw(ctx);

        }

        var lastCalledTime;
        var counter = 0;
        var fpsArray = [];
        var fps;

        var process = function(prms) {
            var $this = this;
            this.frame = 0;
            this.fx = prms.fx || function() { };
            this.end = prms.end || function() { };
            this.secunds = prms.secunds || 0; // un segundo
            this.task = function(prms) {
                prms.porcent = $this.frame / (fps * $this.secunds);
                $this.fx(prms);
                $this.frame++;
                if ($this.frame > fps * $this.secunds) {
                    $this.frame = 0;
                    $this.end();
                }
            };
        };


        var square = function() {
            var $this = this;
            $this.status = square.edo.right;
            $this.x = 0;
            $this.y = 0;
            $this.update = function() {
                switch ($this.status) {
                    case square.edo.begin:
                        break;
                    case square.edo.right:
                        $this.right.task();
                        break;
                }
            };

            this.draw = function(ctx) {
                ctx.beginPath();
                ctx.rect($this.x, $this.y, 10, 10);
                ctx.stroke();
            };

            this.right = new process({
                "_x": $this.x,
                "_y": $this.y,
                "secunds": 1,
                "fx": function(prms) {
                    $this.x = this._x + 100 * prms.porcent;
                },
                "end": function() {
                    this._x = $this.x;
                    $this.status = 0
                }
            });
        };
        square.edo = {};
        square.edo.begin = 0;
        square.edo.right = 1;

        var s = new square();


        function loop(timestamp) {

            var progress = timestamp - lastRender

            update(progress)
            draw()

            lastRender = timestamp
            window.requestAnimationFrame(loop)
        }

        var lastRender = 0
        var fps = 60;

        window.onload = function() {

            window.requestAnimationFrame(loop)

        };

        
        
    </script>

</head>
<body>
    <canvas id="canvas" />
</body>
</html>
