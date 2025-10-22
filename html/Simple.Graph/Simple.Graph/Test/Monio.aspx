<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monio.aspx.cs" Inherits="Simple.Graph.Test.Monio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<script
		src="https://code.jquery.com/jquery-3.6.0.js"
		integrity="sha256-H+K7U5CnXl1h5ywQfKtSj8PCmoN9aaq30gDh27Xc0jk="
		crossorigin="anonymous"></script>
</head>
<body>
	<form id="form1" runat="server">
		<div>
		</div>
	</form>
	<select id="Select1">
		<option value="img/k1.jpg">1</option>
		<option value="img/k2.jpg">2</option>
		<option value="img/k3.jpg">3</option>
		<option value="img/k4.jpg">4</option>
		<option value="img/k5.jpg">5</option>
		<option value="img/k6.jpg">6</option>
		<option value="img/k7.jpg">7</option>
		<option value="img/k8.jpg">8</option>
		<option value="img/k9.jpg">9</option>
		<option value="img/k10.jpg">10</option>
	</select>
	<input id="Button1" type="button" value="button" />

	
	<a href="https://api.whatsapp.com/send?phone=348180819124">....</a>



	<a id="" href="whatsapp://send?text=URL" data-action="share/whatsapp/share">Enviar por WhatsApp</a>


	<br />
	<canvas id="canvas" height="500" width="1000" style="border: 1px solid #000;"></canvas>
	<script type="text/javascript">

		var c = document.getElementById("canvas");
		var ctx = c.getContext("2d");

		var car = new Image();
		var door = new Image();
		var door1 = new Image();
		var door2 = new Image();
		var door3 = new Image();
		var door4 = new Image();

		var texture1 = new Image();

		var center = new Image();

		var c2 = document.createElement('canvas');
		var ctx2 = c2.getContext('2d');



		var imgCount = 7;

		window.onload = function () {

			ctx.scale(2, 2);

			car.onload = start;
			car.src = 'img/moño.png';

			door.onload = start;
			door.src = 'img/door.png';

			door1.onload = start;
			door1.src = 'img/door2.1.PNG';

			door2.onload = start;
			door2.src = 'img/door2.2.PNG';

			door3.onload = start;
			door3.src = 'img/moño3.1.png';

			door4.onload = start;
			door4.src = 'img/moño3.2.png';

			texture1.onload = start;
			//texture1.src = 'img/Capture.PNG';
			texture1.src = 'img/k1.jpg';

			center.onload = start;
			center.src = 'img/kawaii.png';

		};

		function start() {

			//if (--imgCount > 0) { return; }

			c.width = car.width;
			c.height = car.height;

			c2.width = door.width;
			c2.height = door.height;


			// Shadow
			ctx.shadowColor = '#000000';
			ctx.shadowBlur = 15;

			ctx.fillStyle = "#EAEAEA";
			ctx.fillRect(-10, -10, c.width + 10, c.height + 10);

			ctx.drawImage(car, 0, 0);


			texturize(door2, texture1, 0.50, Math.PI / 4, 0.10, c2.width, c2.height);
			ctx.drawImage(c2, 0, 0);

			texturize(door1, texture1, 0.50, Math.PI / 4, 0.30, c2.width, c2.height);
			ctx.drawImage(c2, 0, 0);

			texturize(door, texture1, 0.50, Math.PI / 4, 0.60, c2.width, c2.height);
			ctx.drawImage(c2, 0, 0);

			texturize(door3, texture1, 0.50, Math.PI / 4, 0.99, c2.width, c2.height);
			ctx.drawImage(c2, 0, 0);

			texturize(door4, texture1, 0.50, Math.PI / 4, 0.50, c2.width, c2.height);
			ctx.drawImage(c2, 0, 0);

			//ctx.drawImage(car, 0, 0);

			ctx.drawImage(center, 210, 100, 200, 200);

		}

		function texturize(carpartImage, texture, scale, rotation, opacity, w, h) {
			ctx2.clearRect(0, 0, w, h);
			ctx2.drawImage(carpartImage, 0, 0);
			ctx2.save();
			ctx2.translate(0, 0);

			//ctx2.rotate(rotation);
			ctx2.globalAlpha = opacity;

			ctx2.globalCompositeOperation = 'source-atop';
			//ctx2.drawImage(texture, 0, 0, w, h);

			var bg = ctx2.createPattern(texture1, "repeat");
			ctx2.rect(0, 0, w, h);
			ctx2.fillStyle = bg;
			ctx2.fill();

			ctx2.restore();
		}

		$("#Select1").on("change", function () {

			texture1.src = $(this).val();
			/*
			window.open(
				"https://web.whatsapp.com/" +
				'send?text=' +
				encodeURIComponent("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png"),
				'_blank');
				*/

		});

		$("#Button1").on("click", function () {

			var dato = c.toDataURL("image/jpeg");
			dato = dato.replace("image/jpeg", "image/octet-stream");
			document.location.href = dato;

		});


	</script>

</body>
</html>
