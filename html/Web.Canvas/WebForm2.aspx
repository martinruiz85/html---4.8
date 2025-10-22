<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Web.Canvas.WebForm2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" language="javascript">

        var Maquina = function() {
            var $this = this;
            var estado = null;
            var ciclo_actual = null;

            var inicio = new cliclo_Inicio();
            var jugando = new cliclo_Jugando();
            var termino = new cliclo_Termino();

            this.GetEstado = function() {
                return estado;
            };
            this.SetEstado = function(value) {
                estado = value;
            };

            this.Inica = function() {
                $this.CambiarEstado(1);
            };
            this.CambiarEstado = function(estado) {
                $this.SetEstado(estado);

                if (ciclo_actual != null) 
                    ciclo_actual.Terminar();

                if ($this.GetEstado() == 1) {
                    inicio.Iniciar();
                    ciclo_actual = inicio;
                }
                else if ($this.GetEstado() == 2) {
                    jugando.Iniciar();
                    ciclo_actual = jugando;
                }
                else if ($this.GetEstado() == 3) {
                    termino.Iniciar();
                    ciclo_actual = termino;
                }
            };
            this.Inica();
        };
        Maquina.Satus = {};
        Maquina.Satus.Inicio = 1;
        Maquina.Satus.Jugando = 2;
        Maquina.Satus.Termino = 3;

        var cliclo_Inicio = function() {

            var timer;
            this.Iniciar = function() {
                var count = 0;
                timer = window.setInterval(function() {
                    count += 100;
                    var c = document.getElementById("myCanvas");
                    var ctx = c.getContext("2d");
                    ctx.beginPath();
                    ctx.clearRect(0, 0, c.width, c.height);
                    ctx.moveTo(0, 0);
                    ctx.lineTo(200, 100);

                    ctx.font = "30px Arial";
                    ctx.fillText("" + count / 1000, 10, 50);

                    ctx.stroke();

                }, 100);
            };
            this.Terminar = function() {

                clearInterval(timer);

            };
        };



        var cliclo_Jugando = function() {

            var timer;
            this.Iniciar = function() {
                var count = 0;
                timer = window.setInterval(function() {
                    count += 100;
                    var c = document.getElementById("myCanvas");
                    var ctx = c.getContext("2d");
                    ctx.beginPath();
                    ctx.clearRect(0, 0, c.width, c.height);
                    ctx.beginPath();
                    ctx.arc(95, 50, 40, 0, 2 * Math.PI);

                    ctx.font = "30px Arial";
                    ctx.fillText("" + count / 1000, 10, 50);

                    ctx.stroke();

                }, 100);
            };
            this.Terminar = function() {

                clearInterval(timer);

            };
        };

        var cliclo_Termino = function() {


            var timer;
            this.Iniciar = function() {
            var count = 0;
                timer = window.setInterval(function() {
                    count += 100;
                    var c = document.getElementById("myCanvas");
                    var ctx = c.getContext("2d");
                    ctx.beginPath();
                    ctx.clearRect(0, 0, c.width, c.height);
                    ctx.font = "30px Arial";
                    ctx.fillText("Hello World" + count / 1000, 10, 50);
                    ctx.stroke();


                }, 100);
            };
            this.Terminar = function() {

                clearInterval(timer);

            };
        };
        

        window.onload = function() {

            
            recursive(_array.shift());

            
            var m = new Maquina();

            var iniciar = document.getElementById("iniciar");
            iniciar.onclick = function() {
                m.CambiarEstado(1);
            };

            var jugando = document.getElementById("jugando");
            jugando.onclick = function() {
                m.CambiarEstado(2);
            };

            var terminar = document.getElementById("terminar");
            terminar.onclick = function() {
                m.CambiarEstado(3);
            };

        };
    
    </script>

</head>
<body>
    <button id="iniciar">
        iniciar</button>
    <button id="jugando">
        jugando</button>
    <button id="terminar">
        terminar</button>
    <canvas id="myCanvas" width="200" height="100" style="border: 2px solid #000;"></canvas>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
