<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="WebAngular._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>

    <script src="//ajax.googleapis.com/ajax/libs/angularjs/1.2.19/angular.min.js"></script>

</head>
<body ng-app="app" ng-controller="PruebaController">
    <form id="form1" runat="server">
    <div>
        <h1>
            {{mensaje}}</h1>
    </div>
    </form>

    <script type="text/javascript">
    
    var app = angular.module("app",[]);
  
    function PruebaController($scope) {
        $scope.mensaje="Hola Mundo";
    }
    </script>

</body>
</html>
