<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Web.Canvas.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        @font-face
        {
            font-family: 'MyRoboto-Regular';
            src: url('<%= ResolveUrl("~/Font/Roboto-Regular.eot") %>');
            src: url('<%= ResolveUrl("~/Font/Roboto-Regular.eot") %>?#iefix') format('embedded-opentype'), url('<%= ResolveUrl("~/Font/Roboto-Regular.woff") %>') format('woff'), url('<%= ResolveUrl("~/Font/Roboto-Regular.ttf") %>') format('truetype'), url(<%= ResolveUrl("~/Font/Roboto-Regular.svg") %>#Roboto-Regular') format('svg');
        }
    </style>

    <script type="text/javascript" language="javascript">

        /** 
        * Draws a rounded rectangle using the current state of the canvas.  
        * If you omit the last three params, it will draw a rectangle  
        * outline with a 5 pixel border radius  
        * @param {Number} x The top left x coordinate 
        * @param {Number} y The top left y coordinate  
        * @param {Number} width The width of the rectangle  
        * @param {Number} height The height of the rectangle 
        * @param {Object} radius All corner radii. Defaults to 0,0,0,0; 
        * @param {Boolean} fill Whether to fill the rectangle. Defaults to false. 
        * @param {Boolean} stroke Whether to stroke the rectangle. Defaults to true. 
        */
        CanvasRenderingContext2D.prototype.roundRect = function(x, y, width, height, radius, fill, stroke) {
            var cornerRadius = { upperLeft: 0, upperRight: 0, lowerLeft: 0, lowerRight: 0 };
            if (typeof stroke == "undefined") {
                stroke = true;
            }
            if (typeof radius === "object") {
                for (var side in radius) {
                    cornerRadius[side] = radius[side];
                }
            }

            this.beginPath();
            this.moveTo(x + cornerRadius.upperLeft, y);
            this.lineTo(x + width - cornerRadius.upperRight, y);
            this.quadraticCurveTo(x + width, y, x + width, y + cornerRadius.upperRight);
            this.lineTo(x + width, y + height - cornerRadius.lowerRight);
            this.quadraticCurveTo(x + width, y + height, x + width - cornerRadius.lowerRight, y + height);
            this.lineTo(x + cornerRadius.lowerLeft, y + height);
            this.quadraticCurveTo(x, y + height, x, y + height - cornerRadius.lowerLeft);
            this.lineTo(x, y + cornerRadius.upperLeft);
            this.quadraticCurveTo(x, y, x + cornerRadius.upperLeft, y);
            this.closePath();
            if (stroke) {
                this.stroke();
            }
            if (fill) {
                this.fill();
            }
        }

        String.prototype.isNullOrWhitespace = function(input) {

            if (typeof input === 'undefined' || input == null) return true;

            return input.replace(/\s/g, '').length < 1;
        }

        // First, checks if it isn't implemented yet.
        if (!String.prototype.format) {
            String.prototype.format = function() {
                var args = arguments;
                return this.replace(/{(\d+)}/g, function(match, number) {
                    return typeof args[number] != 'undefined' ? args[number] : match;
                });
            };
        }

        //https://stackoverflow.com/questions/3969475/javascript-pause-settimeout
        var Timer = function(callback, delay) {
            var timerId, start, remaining = delay;

            this.pause = function() {
                window.clearTimeout(timerId);
                remaining -= Date.now() - start;
            };

            this.resume = function() {
                start = Date.now();
                window.clearTimeout(timerId);
                timerId = window.setTimeout(callback, remaining);
            };

            this.resume();
        };


        var Data = {};
        Data.users = { id: 1, user: "mruiz", points: 0, level: 0 };


        var Direction = {};
        Direction.UP = 1;
        Direction.RIGHT = 2;
        Direction.DOWN = 3;
        Direction.LEFT = 4;

        var Draw = function(prms) {

            var x = prms.x || 0;
            var y = prms.y || 0;
            var w = prms.w || 0;
            var h = prms.h || 0;

            this.GetX = function() {
                return x;
            };
            this.GetY = function() {
                return y;
            };
            this.GetW = function() {
                return w;
            };
            this.GetH = function() {
                return h;
            };
            this.SetX = function(value) {
                x = value;
            };
            this.SetY = function(value) {
                y = value;
            };
            this.SetW = function(value) {
                w = value;
            };
            this.SetH = function(value) {
                h = value;
            };
            this.isPointInside = function(x, y) {
                return (x >= this.GetX()
            && x <= this.GetX() + this.GetW()
            && y >= this.GetY()
            && y <= this.GetY() + this.GetH());
            }
        };

        var Letter = function(prms) {

            var $this = this;

            Draw.call(this, prms);

            var charater = prms.character;
            var timer = null;
            var porcent = 1;

            this.GetCharacter = function() {
                return charater;
            };
            this.SetCharacter = function(value) {
                charater = value;
            };
            this.GetTimer = function() {
                return timer;
            };
            this.SetTimer = function(value) {
                timer = value;
            };
            this.GetPorcent = function() {
                return porcent;
            };
            this.SetPorcent = function(value) {
                porcent = value;
            };

            this.onDraw = function(ctx) {

                ctx.beginPath();
                if ($this.GetCharacter() == wordcopy[currentletter])
                //ctx.strokeStyle = "rgba(231,76,60," + $this.GetPorcent() + ")";
                    ctx.strokeStyle = "#e74c3c";
                else
                    ctx.strokeStyle = "#cbcbcb";

                if ($this.GetCharacter() == word[currentletter])
                    ctx.lineWidth = 2 * this.GetPorcent();
                //ctx.lineWidth = "1";
                else
                    ctx.lineWidth = "1";



                ctx.fillStyle = '#ffffff';

                //ctx.rect(this.GetX(), this.GetY(), this.GetW(), this.GetH());
                if ($this.GetCharacter() == wordcopy[currentletter])
                    ctx.roundRect(
                        this.GetX() - 2 * this.GetPorcent(),
                        this.GetY() - 2 * this.GetPorcent(),
                        this.GetW() + 4 * this.GetPorcent(),
                        this.GetH() + 4 * this.GetPorcent(),
                        {
                            upperLeft: 2 + 2 * this.GetPorcent(),
                            upperRight: 2 + 2 * this.GetPorcent(),
                            lowerLeft: 2 + 2 * this.GetPorcent(),
                            lowerRight: 2 + 2 * this.GetPorcent()
                        }, true, true);
                else
                    ctx.roundRect(this.GetX(), this.GetY(), this.GetW(), this.GetH(), { upperLeft: 2, upperRight: 2, lowerLeft: 2, lowerRight: 2 }, true, true);

                ctx.stroke();

                ctx.fillStyle = "rgba(0, 0, 0, 0.87)";



                ctx.textAlign = "center";
                ctx.textBaseline = "middle";

                if ($this.GetCharacter() == wordcopy[currentletter])
                    ctx.font = "{0}px Verdana".format(12 + 2 * $this.GetPorcent());
                else
                    ctx.font = "12px Verdana";

                ctx.fillText(this.GetCharacter(), this.GetX() + this.GetW() / 2, this.GetY() + this.GetH() / 2);

            };

            this.MoveRight = function(prms) {
                var $this = this;


                //window.setTimeout(function() {
                $this.SetTimer(new Timer(function() {

                    var porcentaje = 1 - prms.cicle / 10;
                    var distancia = 20;

                    $this.SetPorcent(porcentaje);

                    if (prms.direction == Direction.RIGHT) {

                        if (prms.ox + distancia < Map[0].length * 20)
                            $this.SetX(prms.ox + distancia * porcentaje);
                    }
                    else if (prms.direction == Direction.UP) {

                        if (prms.oy - distancia >= 0)
                            $this.SetY(prms.oy - distancia * porcentaje);
                    }
                    else if (prms.direction == Direction.DOWN) {
                        if (prms.oy + distancia < Map.length * 20)
                            $this.SetY(prms.oy + distancia * porcentaje);
                    }
                    else if (prms.direction == Direction.LEFT) {
                        if (prms.ox - distancia >= 0)
                            $this.SetX(prms.ox - distancia * porcentaje);
                    }

                    prms.cicle = prms.cicle - 1;

                    if (prms.cicle > -1)
                        $this.MoveRight({ "cicle": prms.cicle, "ox": prms.ox, "oy": prms.oy, "direction": prms.direction });
                    else
                        $this.MoveRight({ "cicle": 10, "ox": $this.GetX(), "oy": $this.GetY(), "direction": Math.floor((Math.random() * 4) + 1) });


                }, 24));
            };

            window.setTimeout(function() {
                //$this.MoveRight({ "cicle": 10, "ox": $this.GetX(), "oy": $this.GetY(), "direction": Direction.RIGHT });
                $this.MoveRight({ "cicle": 10, "ox": $this.GetX(), "oy": $this.GetY(), "direction": Math.floor((Math.random() * 4) + 1) });
            }, 1000);


        };
        Letter.prototype = new Draw({});


        DrawObjects = [];

        Map = [
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", "H", "E", "L", "L", "O", "!", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
                [" ", " ", " ", " ", " ", " ", " ", " ", " ", "2"]
              ];

        //        Map = [
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", "*", "G", "A", "B", "R", "I", "E", "L", "A"],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", "2"]
        //              ];

        //        Map = [
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", "J", "K", "A", "B", "C", "B", "C", "A", " "],
        //                [" ", "B", "C", "A", "B", "C", "B", "C", "A", " "],
        //                [" ", "*", "G", "A", "B", "R", "I", "E", "L", " "],
        //                [" ", "J", "K", "A", "B", "C", "B", "C", "A", " "],
        //                [" ", "B", "C", "A", "B", "C", "B", "C", "A", " "],
        //                [" ", "E", "F", "A", "B", "C", "B", "C", "A", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "],
        //                [" ", " ", " ", " ", " ", " ", " ", " ", " ", " "]
        //              ];
        //Map = [["A"]];

        var pause = false;
        var currentletter = 0;
        //var word = ["*", "G", "A", "B", "R", "I", "E", "L", " ", "2"];
        var word = ["H", "E", "L", "L", "O", " ", "!"];
        var wordcopy = [];

        var transX;
        var transY;

        window.onload = function() {

            //guardar copia de la palabra
            wordcopy = word.slice(0, word.length);

            var btnStar = document.getElementById("btnStart");
            btnStar.onclick += function() {

            };

            var btn = document.getElementById("btn");
            btn.onclick = function() {

                for (var i = 0; i < DrawObjects.length; i++) {
                    if (!pause)
                        DrawObjects[i].GetTimer().pause();
                    else
                        DrawObjects[i].GetTimer().resume();
                }

                pause = !pause;

            };

            var c = document.getElementById("myCanvas");

            var ctx = c.getContext("2d", { alpha: false });
            ctx.imageSmoothingEnabled = false;
            //ctx.translate(0.5, 0.5);

            transX = (c.width - 200) * 0.5;
            transY = (c.height - 200) * 0.5;

            ctx.translate(transX, transY);

            ctx.restore();
            ctx.translate(.5, .5);

            //var l = new Letter({ "x": 0, "y": 0, "w": 20, "h": 20, "character": "A" });
            //l.onDraw(ctx);

            ArrangeLetters();


            //ctx.clearRect(-transX, -transY, c.width, c.height);

            window.setInterval(function() {


                ctx.clearRect(-transX, -transY, c.width, c.height);
                //currentletter = ReturnIndex();



                for (var i = 0; i < 10; i++) {
                    for (var j = 0; j < 10; j++) {

                        var _upperLeft = 0;
                        var _upperRight = 0;
                        var _lowerLeft = 0;
                        var _lowerRight = 0;

                        if (i == 0 && j == 0) _upperLeft = 2;
                        if (i == 9 && j == 0) _upperRight = 2;
                        if (i == 0 && j == 9) _lowerLeft = 2;
                        if (i == 9 && j == 9) _lowerRight = 2;

                        ctx.beginPath();
                        ctx.strokeStyle = "#cbcbcb";
                        ctx.lineWidth = "1";
                        ctx.fillStyle = '#ffffff';
                        //ctx.rect(this.GetX(), this.GetY(), this.GetW(), this.GetH());
                        ctx.roundRect(i * 20, j * 20, 20, 20, { upperLeft: _upperLeft, upperRight: _upperRight, lowerLeft: _lowerLeft, lowerRight: _lowerRight }, true, true);

                        ctx.stroke();

                    }
                }




                for (var i = 0; i < DrawObjects.length; i++) {
                    DrawObjects[i].onDraw(ctx);
                }

            }, 40);

            //            ctx.restore();
            //            ctx.translate(.5, .5);

            c.onmousedown = function(e) { handleMouseDown(e); };


        };

        // calc the mouseclick position and test if it's inside the rect
        function handleMouseDown(e) {

            // calculate the mouse click position
            var c = document.getElementById("myCanvas");
            mouseX = parseInt(e.clientX - c.offsetLeft - transX);
            mouseY = parseInt(e.clientY - c.offsetTop - transY);

            for (var i = 0; i < DrawObjects.length; i++) {
                if (DrawObjects[i].isPointInside(mouseX, mouseY)) {
                    // remover el caracter de la sopa de letras
                    var l = DrawObjects.splice(i, 1);

                    //remover la letra del la palabra copia, que no era la siguiente x
                    var index = wordcopy.join("").indexOf(l[0].GetCharacter());
                    if (index > 0) {
                        wordcopy.splice(index, 1);
                        return;
                    }

                    //remover la letra de la palabra copia, que si era la siguiente ok
                    wordcopy.splice(currentletter, 1);

                    // siguiente letra
                    //currentletter += 1;


                    // mientras existan letras en palabra, ignorar espacios u cadenas vacias
                    while (currentletter < wordcopy.length) {
                        // si es espacio o cadena vacia pasar a la siguiente letra
                        if (wordcopy[currentletter] == "" || wordcopy[currentletter] == " ")
                        //currentletter += 1;
                            wordcopy.splice(currentletter, 1);
                        //salir del ciclo                            
                        else
                            break;
                    }

                    // si ya no hay mas letras de la palabra borrar el exceso
                    if (wordcopy.length == 0) {
                        window.setTimeout(function() {
                            DrawObjects.splice(0, DrawObjects.length);
                        }, 500);

                    }

                    //salir de la funcion para solo remover una sola letra
                    return;
                }
            }
        };

        function ReturnIndex() {
            for (var i = 0; i < DrawObjects.length; i++) {
                if (DrawObjects[i].GetCharacter() == word[currentletter]) {
                    return currentletter;
                }
            }
            //return currentletter + 1;
            return currentletter
        }

        function ArrangeLetters() {

            for (var j = 0; j < Map.length; j++) {
                for (var i = 0; i < Map[0].length; i++) {
                    if (Map[j][i] != " ")
                        DrawObjects.push(
                    new Letter(
                    {
                        "x": i * 20,
                        "y": j * 20,
                        "w": 20,
                        "h": 20,
                        "character": Map[j][i]
                    }));
                }
            }
        };        
                  
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="txtWord" type="text" />
        <input id="btnStart" type="button" value="button" />
        <input id="btn" type="button" value="button" style="display: none;" />
        <canvas id="myCanvas" width="240" height="240" style="border: 1px solid #efefef;
            cursor: pointer; display: block;"></canvas>
    </div>
    </form>
</body>
</html>
