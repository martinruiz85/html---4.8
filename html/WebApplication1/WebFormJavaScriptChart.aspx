<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormJavaScriptChart.aspx.cs"
    Inherits="WebApplication1.WebFormJavaScriptChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="js/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script type="text/ecmascript" language="javascript">

        var timer;
        window.onload = function() {

            //json tree
            //            var tree = {
            //                "text": "node parent",
            //                "nodes": [
            //                    { "text": "node 11" },
            //                    { "text": "node 12" },
            //                    { "text": "node 13",
            //                        "nodes": [{ "text": "node 131"}]}]
            //                    };

            //            var tree = {
            //                "text": "PRESIDENTE GLOBAL DE VENTAS",
            //                "nodes": [
            //                    { "text": "DIRECTOR DE VENTAS ESTADOS UNIDOS",
            //                        "nodes": [
            //                        { "text": "GERENTE DE VENTAS SUR" },
            //                        { "text": "GERENTE DE VENTAS CANADÁ" },
            //                        { "text": "GERENTE DE VENTAS NORTE"}]}]
            //            };

            //arrange tree
            posorden(tree);

            // draw tree
            var c = document.getElementById("myCanvas");
            var ctx = c.getContext("2d");
            ctx.imageSmoothingQuality = "high";
            ctx.translate(0.5, 0.5);
            ctx.imageSmoothingEnabled = true;

            //DCDCDC
            ctx.clearRect(0, 0, c.width, c.height);

            ctx.beginPath();
            ctx.fillStyle = '#f5f5f5';
            ctx.fillRect(0, 0, c.width, c.height);
            ctx.stroke();

            draw(tree, ctx);
            

            //var img    = c.toDataURL("image/png");
            //document.write('<img src="'+img+'"/>');

            $("#txtsearch").keyup(function() {


                var prms = {};
                prms.PosID = this.value || "-1";

                if (timer) window.clearTimeout(timer);

                timer = window.setTimeout(function() {

                    ///////////////////////
                    $("#myImage")[0].src = "OrgChartImgExporter.ashx?PosID=" + prms.PosID;

                    $.ajax({
                        url: "WebFormJavaScriptChart.aspx/BuildTree",
                        data: JSON.stringify(prms),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function(msg) {
                            //alert(msg.d);

                            if (!msg.d) return;

                            tree = msg.d;



                            x = 10;
                            y = 10;
                            level = 0;

                            ///arrange tree
                            posorden(tree);

                            ctx.canvas.height = msg.d.height;
                            ctx.imageSmoothingQuality = "high";
                            ctx.translate(0.5, 0.5);
                            ctx.imageSmoothingEnabled = true;

                            ctx.clearRect(0, 0, c.width, c.height);

                            ctx.beginPath();
                            ctx.fillStyle = '#f5f5f5';
                            ctx.fillRect(0, 0, c.width, c.height);
                            ctx.stroke();

                            draw(tree, ctx);



                        },
                        error: function(result) {
                            alert("ERROR " + result.status + ' ' + result.statusText);
                        }
                    });
                    //////////////////////


                }, 0);

            });

            //repeat();
        };


        var index = 0;
        var range = [55, 101, 113, 115, 92, 122, 58, 69, 22, 32, 9, 38, 13, 88, 135, 66, 10, 71, 33, 7, 100, 45, 60, 42, 80, 108, 146, 144, 5, 105, 141, 48, 74, 131, 121, 127, 132, 47, 1, 8];
        function repeat() {

            window.setTimeout(function() {

                if (index < range.length) {
                    $("#txtsearch").val(range[index]);
                    $("#txtsearch").keyup();
                    index += 1;
                    repeat();
                }

            }, 1000);
        }


        function draw_arrow(ctx, fx, fy, tx, ty) { //ctx is the context
            var angle = Math.atan2(ty - fy, tx - fx);
            ctx.moveTo(fx, fy); ctx.lineTo(tx, ty);
            var w = 1; //width of arrow to one side. 7 pixels wide arrow is pretty
            ctx.strokeStyle = "#4d4d4d"; ctx.fillStyle = "#4d4d4d";
            angle = angle + Math.PI / 2; tx = tx + w * Math.cos(angle); ty = ty + w * Math.sin(angle);
            ctx.lineTo(tx, ty);
            //Drawing an isosceles triangle of sides proportional to 2:7:2
            angle = angle - 1.849096; tx = tx + w * 3.5 * Math.cos(angle); ty = ty + w * 3.5 * Math.sin(angle);
            ctx.lineTo(tx, ty);
            angle = angle - 2.584993; tx = tx + w * 3.5 * Math.cos(angle); ty = ty + w * 3.5 * Math.sin(angle);
            ctx.lineTo(tx, ty);
            angle = angle - 1.849096; tx = tx + w * Math.cos(angle); ty = ty + w * Math.sin(angle);
            ctx.lineTo(tx, ty);
            ctx.stroke(); ctx.fill();
        }

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

        var x = 10;
        var y = 10;
        var level = 0;
        var NodeWidth = 175;
        var NodeHorizontalSpacing = 40;
        var NodeHeight = 20;
        var NodeVerticalSpacing = 10;

        function posorden(tree) {

            if ((tree.nodes || []).length > 0)
                level += 1;

            for (var e in (tree.nodes || [])) {
                posorden(tree.nodes[e]);
            }

            //document.writeln(tree.text);

            if ((tree.nodes || []).length > 0)
                level -= 1;

            var newx = x + level * (NodeWidth + NodeHorizontalSpacing);

            if ((tree.nodes || []).length > 0) {
                var newy = tree.nodes[0].y + ((tree.nodes[tree.nodes.length - 1].y - tree.nodes[0].y) / 2.00);
                tree.x = newx;
                tree.y = newy;
                tree.level = level;
            }
            else {
                tree.x = newx;
                tree.y = y;
                tree.level = level;
            }

            y += NodeHeight + NodeVerticalSpacing;

        }

        function draw(tree, ctx) {

            var segments_dash = (tree.text || "").length > 0 ? [] : [2, 2];
            var fill_color_node = tree.level != 1 ? '#ffffff' : '#ffffb2';
            var border_color_node = (tree.text || "").length > 0 ? '#cbcbcb' : '#000000';
            var margin_node = 10;

            //DRAW NODE SHADOW
            ctx.beginPath();
            ctx.strokeStyle = "#cbcbcb";
            ctx.lineWidth = "1";
            ctx.fillStyle = '#A9A9A9';
            ctx.setLineDash(segments_dash);
            ctx.roundRect(tree.x + 3, tree.y + 4, NodeWidth, NodeHeight, { upperLeft: 5, upperRight: 5, lowerLeft: 5, lowerRight: 5 }, true, true);
            ctx.stroke();
            ctx.closePath();

            //DRAW NODE 
            ctx.beginPath();
            ctx.strokeStyle = border_color_node;
            ctx.lineWidth = "1";
            ctx.fillStyle = '#a9a9a9';


            ctx.setLineDash(segments_dash);
            ctx.fillStyle = fill_color_node;
            //ctx.rect(tree.x, tree.y, NodeWidth, NodeHeight);            
            ctx.roundRect(tree.x, tree.y, NodeWidth, NodeHeight, { upperLeft: 5, upperRight: 5, lowerLeft: 5, lowerRight: 5 }, true, true);
            ctx.stroke();
            ctx.closePath();

            //DRAW TEXT
            ctx.beginPath();
            ctx.font = "8px Arial";
            ctx.fillStyle = '#000000';
            ctx.textAlign = "center";
            ctx.textBaseline = "middle";
            //var w = ctx.measureText((tree.text || "").trim()).width;
            //center text
            ctx.fillText((tree.text || "").trim(), tree.x + (NodeWidth / 2), tree.y + (NodeHeight / 2), NodeWidth - margin_node);
            ctx.stroke();
            ctx.closePath();

            //DRAW LINES
            for (var e in (tree.nodes || [])) {

                //SIMPLE LINE
                //ctx.beginPath();
                //ctx.moveTo(tree.x + NodeWidth, tree.y + NodeHeight / 2);
                //ctx.lineTo(tree.nodes[e].x, tree.nodes[e].y + NodeHeight / 2);
                //ctx.stroke();
                //ctx.closePath();

                //draw circle
                ctx.beginPath();
                ctx.arc(tree.nodes[e].x - NodeHorizontalSpacing / 2, tree.nodes[e].y + NodeHeight / 2, 2, 0, Math.PI * 2, true);
                ctx.fill();
                ctx.closePath();

                ctx.beginPath();
                ctx.fillStyle = '#0e0e0e';
                ctx.strokeStyle = "#0e0e0e";
                ctx.lineWidth = "1px";
                ctx.lineCap = "butt";
                ctx.setLineDash([]);


                draw_arrow(ctx, tree.x + NodeWidth + NodeHorizontalSpacing / 2, tree.y + NodeHeight / 2, tree.x + NodeWidth + 5, tree.y + NodeHeight / 2);
                //parent to midle
                ctx.moveTo(tree.x + NodeWidth, tree.y + NodeHeight / 2);
                ctx.lineTo(tree.x + NodeWidth + NodeHorizontalSpacing / 2, tree.y + NodeHeight / 2);

                //parent to midle and child to midle
                ctx.moveTo(tree.x + NodeWidth + NodeHorizontalSpacing / 2, tree.y + NodeHeight / 2);
                ctx.lineTo(tree.nodes[e].x - NodeHorizontalSpacing / 2, tree.nodes[e].y + NodeHeight / 2);                
                
                //child to midle
                ctx.moveTo(tree.nodes[e].x, tree.nodes[e].y + NodeHeight / 2);
                ctx.lineTo(tree.nodes[e].x - NodeHorizontalSpacing / 2, tree.nodes[e].y + NodeHeight / 2);

                ctx.stroke();
                ctx.closePath();

                draw(tree.nodes[e], ctx);
            }
        }
            
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <input type="text" id="txtsearch" class="allownumericwithoutdecimal" maxlength="9"
        autocomplete="off" />
    <table>
        <tr>
            <td valign="top">
                <canvas runat="server" id="myCanvas" width="650" height="200" style="border: 0px solid #000;
                    display: block;">
        </canvas>
            </td>
            <td  valign="top">
                <img id="myImage" alt="" src="OrgChartImgExporter.ashx?PosID=" style="display: inline;" />
            </td>
        </tr>
    </table>
    </form>
</body>

<script type="text/javascript">
    $(".allownumericwithdecimal").on("keypress keyup blur", function(event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

    $(".allownumericwithoutdecimal").on("keypress keyup blur", function(event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
</script>

</html>
