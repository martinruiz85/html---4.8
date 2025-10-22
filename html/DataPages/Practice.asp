<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"> 
<style>
	body
	{
		font-family:tahoma;
		font-size:12px;
		color:#212121;
	}
	
	span
	{
		font-size:12px;
		
	}
	
	table
	{		
		/*table-layout: fixed;*/
		border-collapse: collapse;
		width: 100%;
		border: 0px solid black;
	}
	
	th
	{
		/*width: 100px;*/
		border-bottom:5px solid #2e8ece;
		/*background: #eee;*/
		background:#3498DB;
		padding:10px;
		color:#FFF;
		font-family:tahoma;
		font-size:12px;
		font-weight: normal;
	}
	
	th div
	{		
		/*width: 100px;*/
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;
        /*background: #2980B9;		*/
	}
	
	td
	{
		/*width: 100px;*/
		/*border:1px solid #eee;*/
		background: #f9fafb; 
		padding:5px;
		border-bottom:1px solid #eee;
	}
		
	
	tr:nth-of-type(odd) td {
		background-color: #fff;
    }
	
	/*td:nth-of-type(odd) {
		background: #f9fafb;
	}*/
	
	td div
	{		
		/*width: 100px;*/
		white-space: nowrap;
		overflow: hidden;
		text-overflow: ellipsis;		
	}
		
  .ui-resizable-se {    
	background:#3498DB;	
	background-image:url('resize.png');
	padding:3px;	
  }
  
  .ui-resizable-s
  {
	background:#3498DB;	
	//background-image:url('resize.png');
	padding:2px;	
  }
  
  textarea
  {
	width:98%; 
	height: 100px;
	background:#f5f5f5;
	font-family:tahoma;
	color:#6E6E6E; 
	resize: both;
	overflow: visible;
	border: 1px solid #eee;
  }
  
  select
  {
    width:100px;
	font-family:tahoma;
	font-size:12px;
	font-weight: normal;
	margin-top:3px;
	margin-bottom:3px;
	background:#fff; 
	border:1px Solid #BDBDBD; 
	padding:8px;
	Color:#6E6E6E;
	border-radius: 4px; 
	cursor:pointer;  
	/*border-bottom:5px solid #2e8ece;*/
  }
  
  option
  {
    width:100px;
	font-family:tahoma;
	font-size:12px;
	font-weight: normal;
	margin-top:3px;
	margin-bottom:3px;
	background:#fff; 
	padding:10px;
	Color:#6E6E6E;
	border-radius: 4px; 
	cursor:pointer;  
  }
  
  
  
  input[type=button]
  {
    width:100px;
	font-family:tahoma;
	font-size:12px;
	font-weight: normal;
	margin-top:3px;
	margin-bottom:3px;
	background:#3498DB; 
	border:0px; 
	padding:10px;
	Color:#fff;
	border-radius: 4px; 
	cursor:pointer;  
	/*border-bottom:5px solid #2e8ece;*/
  }

  input[type=button]:disabled {
    background: #dddddd;
	cursor:wait;
 }
</style>
<style type="text/css">
        .loader
        {
            position: fixed;
            left: 0px;
            top: 0px;
            width: 100%;
            height: 100%;
            z-index: 9999;
            background: 50% 50% no-repeat rgb(249,249,249); 
        }
</style>
<style>
.sk-fading-circle {
  margin: 100px auto;
  width: 40px;
  height: 40px;
  position: relative;
}

.sk-fading-circle .sk-circle {
  width: 100%;
  height: 100%;
  position: absolute;
  left: 0;
  top: 0;
}

.sk-fading-circle .sk-circle:before {
  content: '';
  display: block;
  margin: 0 auto;
  width: 15%;
  height: 15%;
  background-color: #333;
  border-radius: 100%;
  -webkit-animation: sk-circleFadeDelay 1.2s infinite ease-in-out both;
          animation: sk-circleFadeDelay 1.2s infinite ease-in-out both;
}
.sk-fading-circle .sk-circle2 {
  -webkit-transform: rotate(30deg);
      -ms-transform: rotate(30deg);
          transform: rotate(30deg);
}
.sk-fading-circle .sk-circle3 {
  -webkit-transform: rotate(60deg);
      -ms-transform: rotate(60deg);
          transform: rotate(60deg);
}
.sk-fading-circle .sk-circle4 {
  -webkit-transform: rotate(90deg);
      -ms-transform: rotate(90deg);
          transform: rotate(90deg);
}
.sk-fading-circle .sk-circle5 {
  -webkit-transform: rotate(120deg);
      -ms-transform: rotate(120deg);
          transform: rotate(120deg);
}
.sk-fading-circle .sk-circle6 {
  -webkit-transform: rotate(150deg);
      -ms-transform: rotate(150deg);
          transform: rotate(150deg);
}
.sk-fading-circle .sk-circle7 {
  -webkit-transform: rotate(180deg);
      -ms-transform: rotate(180deg);
          transform: rotate(180deg);
}
.sk-fading-circle .sk-circle8 {
  -webkit-transform: rotate(210deg);
      -ms-transform: rotate(210deg);
          transform: rotate(210deg);
}
.sk-fading-circle .sk-circle9 {
  -webkit-transform: rotate(240deg);
      -ms-transform: rotate(240deg);
          transform: rotate(240deg);
}
.sk-fading-circle .sk-circle10 {
  -webkit-transform: rotate(270deg);
      -ms-transform: rotate(270deg);
          transform: rotate(270deg);
}
.sk-fading-circle .sk-circle11 {
  -webkit-transform: rotate(300deg);
      -ms-transform: rotate(300deg);
          transform: rotate(300deg); 
}
.sk-fading-circle .sk-circle12 {
  -webkit-transform: rotate(330deg);
      -ms-transform: rotate(330deg);
          transform: rotate(330deg); 
}
.sk-fading-circle .sk-circle2:before {
  -webkit-animation-delay: -1.1s;
          animation-delay: -1.1s; 
}
.sk-fading-circle .sk-circle3:before {
  -webkit-animation-delay: -1s;
          animation-delay: -1s; 
}
.sk-fading-circle .sk-circle4:before {
  -webkit-animation-delay: -0.9s;
          animation-delay: -0.9s; 
}
.sk-fading-circle .sk-circle5:before {
  -webkit-animation-delay: -0.8s;
          animation-delay: -0.8s; 
}
.sk-fading-circle .sk-circle6:before {
  -webkit-animation-delay: -0.7s;
          animation-delay: -0.7s; 
}
.sk-fading-circle .sk-circle7:before {
  -webkit-animation-delay: -0.6s;
          animation-delay: -0.6s; 
}
.sk-fading-circle .sk-circle8:before {
  -webkit-animation-delay: -0.5s;
          animation-delay: -0.5s; 
}
.sk-fading-circle .sk-circle9:before {
  -webkit-animation-delay: -0.4s;
          animation-delay: -0.4s;
}
.sk-fading-circle .sk-circle10:before {
  -webkit-animation-delay: -0.3s;
          animation-delay: -0.3s;
}
.sk-fading-circle .sk-circle11:before {
  -webkit-animation-delay: -0.2s;
          animation-delay: -0.2s;
}
.sk-fading-circle .sk-circle12:before {
  -webkit-animation-delay: -0.1s;
          animation-delay: -0.1s;
}

@-webkit-keyframes sk-circleFadeDelay {
  0%, 39%, 100% { opacity: 0; }
  40% { opacity: 1; }
}

@keyframes sk-circleFadeDelay {
  0%, 39%, 100% { opacity: 0; }
  40% { opacity: 1; } 
}
</style>
<link rel="stylesheet" href="../ConfigXLP/Style/jquery-ui.css">
<script src="../../Scripts/jquery.js"></script>
<script src="../../Scripts/jquery.ui.js"></script>	
<script>
// A $( document ).ready() block.
$( document ).ready(function() {    
	
	//$(".loader").fadeOut(800);
	
	$( "#text" ).slideDown(800,function() {
		// Animation complete.
		$( "#text" ).resizable({
		  handles: "s"
		}); 
	});
	
	$('#spiner').hide();	
	
	$("#Clear").click(function(){
		$( "#container" ).empty();
	});
	
	$("#Submit").click(function(){
		var _data = {};
		_data.mode = $("#mode").val();
		_data.text = $("#text").val();
		
		$.ajax({		
			url: "PracticeReload.asp", 
			type: 'POST',
			data: _data,
			beforeSend: function(){
				// Handle the beforeSend event
				$( "#container" ).empty();
				$('#Submit').prop('disabled', true);
				$('#spiner').show();
		    },
		    complete: function(){
				// Handle the complete event
				$('#Submit').prop('disabled', false);
				$('#spiner').hide(200);
		    },
			success: function(result){
				$("#container").html(result);
			},
			error: function(xhr, status, error) {
				// handle error
				alert(status);
			}
		});
	});
	
});
</script>
</head>
<body>
<!--<div class="loader">
</div>-->
<form method="post" accept-charset="utf-8">
  <textarea id="text" name="text" style="display:none;"><%= REQUEST.FORM("text")%></textarea>
  <div>
  <select name="mode" id="mode">
      <option value="1"selected="selected">Query</option>
	  <option value="2">Several Querys</option>
      <option value="3">NonQuery</option>
  </select>
  <input id="Submit" type="button" value="Submit">
  <input id="Clear" type="button" value="Clear">
  <div>
  <div id="spiner">
  <div class="sk-fading-circle">
  <div class="sk-circle1 sk-circle"></div>
  <div class="sk-circle2 sk-circle"></div>
  <div class="sk-circle3 sk-circle"></div>
  <div class="sk-circle4 sk-circle"></div>
  <div class="sk-circle5 sk-circle"></div>
  <div class="sk-circle6 sk-circle"></div>
  <div class="sk-circle7 sk-circle"></div>
  <div class="sk-circle8 sk-circle"></div>
  <div class="sk-circle9 sk-circle"></div>
  <div class="sk-circle10 sk-circle"></div>
  <div class="sk-circle11 sk-circle"></div>
  <div class="sk-circle12 sk-circle"></div>
  </div>
  </div>
</form>
<div id="container"></div>
</body>
</html>