<%@ Page Title="" Language="C#" MasterPageFile="~/MM.Master" AutoEventWireup="true"
    CodeBehind="DetalleProducto.aspx.cs" Inherits="MM.Site.DetalleProducto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*size image: 781 x 521*/
        /*https://mdbootstrap.com/plugins/jquery/sortable/#!*/
        body
        {
            color: #34495e;
            font-size: 12px;
        }
        .btn
        {
            font-size: 12px;
        }
        .your-custom-class
        {
            width: 200px;
            height: 200px;
            background: #ffffff url(../images/testMaster.gif) no-repeat center center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uib-tabset active="activeJustified" justified="true">
    <uib-tab index="0" heading="Detalle">
        <div class="card " style="border: 0px; margin-top: 10px;">
                <div class="card-header" style="background: #FF0080; color: #ffffff;">
                    Detalle del Producto {{ProductoID}}
                </div>
                <div class="card-body">
                    <input type="hidden" ng-model="ItemProducto.ProductoID" />
                    <div class="form-group">
                        <label for="cod">
                            Codigo:</label>
                        <input type="text" class="form-control" id="cod" ng-model="ItemProducto.Codigo">
                    </div>
                    <div class="form-group">
                        <label for="usr">
                            Nombre:</label>
                        <input type="text" class="form-control" id="Text2" ng-model="ItemProducto.NombreProducto">
                    </div>
                    <div class="form-group">
                        <label for="text">
                            Descripcion:</label>
                        <input type="text" class="form-control" id="pwd" ng-model="ItemProducto.DescripcionProducto">
                    </div>
                    <div class="form-group">
                        <label for="price">
                            Precio:</label>
                        <input type="text" class="form-control" id="price" ng-model="ItemProducto.Precio">
                    </div>
                    <div class="form-group">
                        <label for="price">
                            No Disponibles:</label>
                        <input type="text" class="form-control" id="Text1" ng-model="ItemProducto.NoDisponibles">
                    </div>
                    <div class="form-group">
                        <label for="Select1">
                            Categoria:</label>
                        <select class="form-control" id="Select1" ng-model="ItemProducto.CategoriaID">
                            <option ng-value="-1">--sin especificar--</option>
                            <option ng-repeat="Categoria in ListCategoria" ng-value="Categoria.CategoriaID">{{Categoria.NombreCategoria}}</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label for="Select2">
                            Activo:</label>
                        <select class="form-control" id="Select2" ng-model="ItemProducto.EsActivo">
                            <option ng-value="true">Activo</option>
                            <option ng-value="false">Inactivo</option>
                        </select>
                    </div>
                    <div class="form-group text-right">
                        <button type="button" class="btn btn-primary" style="background: #FF0080; border-color: #FF0080;"
                            ng-click="DataSave(ItemProducto)">
                            Guardar</button>
                    </div>
                </div>
        </div>
    </uib-tab>
    <uib-tab index="1" heading="Imagenes" ng-click="ImagenesProductoList()">
        <div class="card " style="border: 0px; margin-top: 10px;">
                <div class="card-header" style="background: #FF0080; color: #ffffff;">
                    Imagenes del Producto {{ProductoID}}
                </div>
                <div class="card-body">                   
                <ul class="list-group">
                        <li class="list-group-item" class="list-group-item">
                            <div class="row">
                                <div ng-repeat="row in ImagenProductosList.Table" class="col-xs-4 col-sm-4 col-md-4 col-lg-3">                                                        
                                    <div class="card" style="margin-bottom: 10px; border: 0px">
                                        <div class="text-center" style=" min-height:200px; background: #ffffff url(../images/MMProcess.gif) no-repeat center center;">
                                            <img class="card-img-top" sb-load="onImgLoad($event)"  src="handdlers/hdlImage.ashx?ProductoID={{row.ProductoID}}" alt="Cinque Terre">                                    
                                        </div>
                                        <div class="card-body" style="font-size: 12px;">
                                            <div class="row">
                                                <div class="col-md-6 text-center align-middle">
                                                    <div class="form-check">
                                                      <input type="radio" name="Portada" ng-model="row.Portada" ng-value="row.ImagenProductoID">                                                        
                                                      <label class="form-check-label" for="Portada">
                                                        Predeterminada
                                                      </label>
                                                    </div>
                                                     
                                                </div>
                                                <div class="col-md-6">                                                        
                                                    <input type="text" class="form-control" id="Text3" ng-model="row.Orden">
                                                </div>
                                            </div>    
                                        </div>
                                    </div>
                                </div>
                            </div>                            
                        </li>
                    </ul>
                </div>
                <div class="form-group text-right" style="margin-right:20px;">
                        <button type="button" class="btn btn-primary" style="background: #FF0080; border-color: #FF0080;"
                            ng-click="DataSave(ItemProducto)">
                            Guardar</button>
                </div>
        </div>
    </uib-tab>
    <uib-tab index="2" heading="Subir Imagenes">
        <div class="card " style="border: 0px; margin-top: 10px;">                
        </div>
    </uib-tab>
    </uib-tabset>

    <script type="text/javascript">

        // Custom
        //var customElement = $("<div>", { "class": "your-custom-class" });

        //$.LoadingOverlaySetup({
        //background: "rgba(255, 255, 255, 0.5)",

        //image: '<svg width="255" height="169" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink"><image href="../images/monosmarie.jpg" /></svg>',
        //imageAnimation: "1s fadein",

        //imageColor: "#ffffff"
        //size : 50,
        //maxSize: 255,
        //minSize: 20,
        //imageAutoResize:true,
        //imageResizeFactor:1,
        //direction:"column"
        //image: "",
        //custom: customElement
        //});

        function getUrlVars() {
            var vars = {};
            var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value) {
                vars[key] = value;
            });
            return vars;
        }
        function getUrlParam(parameter, defaultvalue) {
            var urlparameter = defaultvalue;
            if (window.location.href.indexOf(parameter) > -1) {
                urlparameter = getUrlVars()[parameter];
            }
            return urlparameter;
        }



        var app = angular.module('myApp', ['ui.bootstrap']);


        app.directive('sbLoad', ['$parse', function($parse) {
            return {
                restrict: 'A',
                link: function(scope, elem, attrs) {
                    var fn = $parse(attrs.sbLoad);
                    elem.on('load', function(event) {
                        scope.$apply(function() {
                            fn(scope, { $event: event });
                        });
                    });
                }
            };
        } ]);


        app.controller('myCtrl', function($scope) {
            $scope.ProductoID = getUrlParam('ProductoID', '-1');
            $scope.ItemProducto = {};
            $scope.ListCategoria = [];

            $scope.DataGet = function(ProductoID) {

                $.ajax({
                    url: 'services/ProductoService.svc/Get',
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify({ "ProductoID": ProductoID }),
                    contentType: 'application/json; charset=utf-8',
                    global: true,
                    async: true,
                    beforeSend: function() {

                    }
                })
                .done(function(result) {
                    var jsonResult = JSON.parse(result.Datos);
                    $scope.ItemProducto = jsonResult.Table[0];
                    $scope.ListCategoria = jsonResult.Table1;
                    $scope.$apply();
                })
                .fail(function(jqXHR, statusText) {
                    if (jqXHR.status) {
                        //params.fail && params.fail(jqXHR, statusText)
                    }
                }).always(function() {
                });

            };


            $scope.DataSave = function(ItemProducto) {

                $.ajax({
                    url: 'services/ProductoService.svc/Save',
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify
                    ({
                        "ProductoID": ItemProducto.ProductoID,
                        "Codigo": ItemProducto.Codigo,
                        "NombreProducto": ItemProducto.NombreProducto,
                        "CategoriaID": ItemProducto.CategoriaID,
                        "DescripcionProducto": ItemProducto.DescripcionProducto,
                        "Precio": ItemProducto.Precio,
                        "NoDisponibles": ItemProducto.NoDisponibles,
                        "EsActivo": ItemProducto.EsActivo
                    }),
                    contentType: 'application/json; charset=utf-8',
                    global: true,
                    async: true,
                    beforeSend: function() {
                        // Show full page LoadingOverlay
                        $.LoadingOverlay("show");
                    }
                })
                .done(function(result) {
                    $scope.DataGet($scope.ProductoID);

                })
                .fail(function(jqXHR, statusText) {
                    if (jqXHR.status) {
                        //params.fail && params.fail(jqXHR, statusText)
                    }
                }).always(function() {
                    $.LoadingOverlay("hide");
                });

            };


            $scope.ImagenProductosList = {};
            $scope.ImagenesProductoList = function() {

                $.ajax({
                    url: 'services/ImagenProductoService.svc/List',
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify
                    ({
                        "ProductoID": $scope.ProductoID
                    }),
                    contentType: 'application/json; charset=utf-8',
                    global: true,
                    async: true,
                    beforeSend: function() {
                    }
                })
                .done(function(result) {
                    $scope.ImagenProductosList = JSON.parse(result.Datos);
                    $scope.$apply();
                })
                .fail(function(jqXHR, statusText) {
                    if (jqXHR.status) {
                        //params.fail && params.fail(jqXHR, statusText)
                    }
                }).always(function() {
                });

            };

            $scope.onImgLoad = function(event) {
                // ...
                $(event.currentTarget).fadeIn(1000);
            };



            $scope.DataGet($scope.ProductoID);

        });
    </script>

</asp:Content>
