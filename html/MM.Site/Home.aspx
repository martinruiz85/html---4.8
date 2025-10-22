<%@ Page Title="" Language="C#" MasterPageFile="~/MM.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="MM.Site.Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        body
        {
            color: #34495e;
            font-size: 12px;
        }
        summary
        {
            display: list-item;
            cursor: pointer;
        }
        .btn:not(:disabled):not(.disabled)
        {
            cursor: pointer;
        }
        .custom-range::-webkit-slider-runnable-track
        {
            width: 100%;
            height: 0.5rem;
            color: transparent;
            cursor: pointer;
            background-color: #dee2e6;
            border-color: transparent;
            border-radius: 1rem;
        }
        .custom-range::-moz-range-track
        {
            width: 100%;
            height: 0.5rem;
            color: transparent;
            cursor: pointer;
            background-color: #dee2e6;
            border-color: transparent;
            border-radius: 1rem;
        }
        .custom-range::-ms-track
        {
            width: 100%;
            height: 0.5rem;
            color: transparent;
            cursor: pointer;
            background-color: transparent;
            border-color: transparent;
            border-width: 0.5rem;
        }
        .navbar-toggler:not(:disabled):not(.disabled)
        {
            cursor: pointer;
        }
        .page-link:not(:disabled):not(.disabled)
        {
            cursor: pointer;
        }
        .close:not(:disabled):not(.disabled)
        {
            cursor: pointer;
        }
        .carousel-indicators li
        {
            box-sizing: content-box;
            -ms-flex: 0 1 auto;
            flex: 0 1 auto;
            width: 30px;
            height: 3px;
            margin-right: 3px;
            margin-left: 3px;
            text-indent: -999px;
            cursor: pointer;
            background-color: #fff;
            background-clip: padding-box;
            border-top: 10px solid transparent;
            border-bottom: 10px solid transparent;
            opacity: .5;
            transition: opacity 0.6s ease;
        }
        .pagination > li > a
        {
            background-color: white;
            color: #FF0080;
        }
        .pagination > li > a:focus, .pagination > li > a:hover, .pagination > li > span:focus, .pagination > li > span:hover
        {
            color: #5a5a5a;
            background-color: #eee;
            border-color: #ddd;
        }
        .pagination > .active > a
        {
            color: white;
            background-color: #FF0080 !important;
            border: solid 1px #FF0080 !important;
        }
        .pagination > .active > a:hover
        {
            background-color: #FF0080 !important;
            border: solid 1px #FF0080;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
            <div class="card" style="border: 0px">
                <div class="card-header" style="background: #FF0080; color: #ffffff;">
                    Filtrar Por
                </div>
                <div class="card-body">
                    <h5 class="font-weight-bold mb-3">
                        Caracteristicas</h5>
                    <p class="mb-0">
                        busca por lo que tu prefieras :D</p>
                </div>
                <ul class="list-group list-group-flush">
                    <li class="list-group-item">Edades</li>
                    <li class="list-group-item">Precios</li>
                    <li class="list-group-item">Descripcion</li>
                    <li class="list-group-item">Color</li>
                    <li class="list-group-item">Personaje</li>
                    <li class="list-group-item">Animales</li>
                    <li class="list-group-item">Frutas</li>
                    <li class="list-group-item">Tamaños</li>
                    <li class="list-group-item">Producto</li>
                </ul>
                <div class="card-body text-right">
                    <a href="#!" class=" btn btn-primary" style="background: #FF0080; border-color: #FF0080;">
                        Limpiar</a> <a href="#!" class=" btn btn-primary" style="background: #FF0080; border-color: #FF0080;">
                            Buscar</a>
                </div>
            </div>
        </div>
        <div class="col-xs-9 col-sm-9 col-md-9 col-lg-9">
            <div class="row">
                <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
                    <div class="dropdown drop">
                        <button type="button" class="btn btn-primary dropdown-toggle" style="width: 100%;
                            background: #FF0080; border-color: #FF0080; font-size: 12px;" data-toggle="dropdown">
                            Ordenar Por
                        </button>
                        <div class="dropdown-menu">
                            <a class="dropdown-item" href="#">Precio (Alto a Bajo)</a> <a class="dropdown-item"
                                href="#">Precio (Bajo a Alto)</a> <a class="dropdown-item" href="#">Fecha (Nuevos-Anteriores)</a>
                            <a class="dropdown-item" href="#">Fecha (Anteriores- Nuevos)</a> <a class="dropdown-item"
                                href="#">Descripcion (A-Z)</a> <a class="dropdown-item" href="#">Descripcion (Z-A)</a>
                        </div>
                    </div>
                </div>
                <div class="col-xs-7 col-sm-7 col-md-7 col-lg-7">
                    <ul uib-pagination total-items="totalItems" ng-model="currentPage" class=" pagination  justify-content-end"
                        boundary-link-numbers="true" rotate="false" max-size="maxSize" ng-change="pageChanged()"
                        items-per-page="24" num-pages="numPages" previous-text="&lsaquo;" next-text="&rsaquo;"
                        first-text="&laquo;" last-text="&raquo;">
                    </ul>
                </div>
                <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                    <pre>P&#225;gina: {{currentPage}} / {{numPages}}</pre>
                </div>
            </div>
            <div id="table" ng-repeat="row in ProductosList.Table" class="row">
                <div ng-repeat="(key, value) in row" class="col-xs-12 col-sm-6 col-md-4 col-lg-3">
                    <div ng-show="(value)" class="card" style="margin-bottom: 10px; border: 0px">
                        <div class="text-center" style="height: 200px; min-height: 200px; overflow: hidden;
                            background: #ffffff url(../images/MMProcess.gif) no-repeat center center;">
                            <img class="card-img-top" sb-load="onImgLoad($event)" style="display: none;" src="handdlers/hdlImage.ashx?ProductoID={{value.ProductoID}}"
                                alt="Card image" />
                        </div>
                        <div class="card-body" style="font-size: 12px;">
                            <h6 class="card-title" style="">
                                {{value.Codigo}}</h6>
                            <p class="card-text">
                                {{value.NombreProducto}}&nbsp;</p>
                            <p class="card-text">
                                {{value.Precio | currency:"MXN$":0}}</p>
                            <table width="100%">
                                <tr>
                                    <td>
                                        Disponibles
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span>{{value.NoDisponibles}}</span>
                                    </td>
                                    <td>
                                        <div style="float: right;">
                                            <a href="#" class="btn btn-primary" style="background-color: #FF0080; border-color: #FF0080;
                                                font-size: 12px;">Ver...</a>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-3 col-sm-3 col-md-3 col-lg-3">
                    <div class="dropdown drop">
                        <button type="button" class="btn btn-primary dropdown-toggle" style="width: 100%;
                            background: #FF0080; border-color: #FF0080; font-size: 12px;" data-toggle="dropdown">
                            Ordenar Por
                        </button>
                        <div class="dropdown-menu">
                            <a class="dropdown-item" href="#">Precio (Alto a Bajo)</a> <a class="dropdown-item"
                                href="#">Precio (Bajo a Alto)</a> <a class="dropdown-item" href="#">Fecha (Nuevos-Anteriores)</a>
                            <a class="dropdown-item" href="#">Fecha (Anteriores- Nuevos)</a> <a class="dropdown-item"
                                href="#">Descripcion (A-Z)</a> <a class="dropdown-item" href="#">Descripcion (Z-A)</a>
                        </div>
                    </div>
                </div>
                <div class="col-xs-7 col-sm-7 col-md-7 col-lg-7">
                    <ul uib-pagination total-items="totalItems" ng-model="currentPage" class=" pagination  justify-content-end"
                        boundary-link-numbers="true" rotate="false" max-size="maxSize" ng-change="pageChanged()"
                        items-per-page="24" num-pages="numPages" previous-text="&lsaquo;" next-text="&rsaquo;"
                        first-text="&laquo;" last-text="&raquo;">
                    </ul>
                </div>
                <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2">
                    <pre>P&#225;gina: {{currentPage}} / {{numPages}}</pre>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
    
        function RemplaceTableMatrix($tableMatrix, $tableMap, $parentPropertyName) {
            for (var i = 0; i < $tableMatrix.length; i++) {
                for (var prop in $tableMatrix[i]) {
                    var $colvalue = $tableMatrix[i][prop];
                    $tableMatrix[i][prop] = $tableMap.filter(function(obj) {
                        return obj[$parentPropertyName] == $colvalue;
                    })[0];
                }
            }
        };

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
            $scope.ProductosList = {};

            $scope.currentPage = 1;
            $scope.maxSize = 5;

            $scope.setPage = function(pageNo) {
                $scope.currentPage = pageNo;
            };

            $scope.pageChanged = function() {
                $scope.DataList($scope.currentPage);
            };

            $scope.onImgLoad = function(event) {
                // ...
                $(event.currentTarget).fadeIn(1000);
                /*
                $(event.currentTarget).animate({ width: "100%", height: "100%" }, 0);

                $(event.currentTarget).hover(
                function() {
                $(this).css("cursor", "pointer");
                $(this).animate({ width: "110%", height: "110%" }, 250);
                },
                function() {
                $(this).animate({ width: "100%", height: "100%" }, 250);
                });
                */
            };

            $scope.DataList = function(pageNumber) {

                $.ajax({
                    url: 'services/ProductoService.svc/List',
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify({ "pageNumber": pageNumber }),
                    contentType: 'application/json; charset=utf-8',
                    global: true,
                    async: true,
                    beforeSend: function() {
                        $.LoadingOverlay("show");
                        //$("#table").LoadingOverlay("show")
                    }
                })
                .done(function(result) {
                    $scope.ProductosList = JSON.parse(result.Datos);

                    RemplaceTableMatrix($scope.ProductosList.Table, $scope.ProductosList.Table1, "ProductoID");
                    $scope.totalItems = $scope.ProductosList.Table2[0].Total;

                    $scope.$apply();
                    $([document.documentElement, document.body]).animate({
                        scrollTop: $("#elementtoScrollToID").offset().top
                    }, 500);
                })
                .fail(function(jqXHR, statusText) {
                    if (jqXHR.status) {
                        //params.fail && params.fail(jqXHR, statusText)
                    }
                }).always(function() {
                    $.LoadingOverlay("hide");
                    //$("#table").LoadingOverlay("hide")
                });

            };

            window.setTimeout(function() {
                $scope.DataList(1);
            }, 100);

        });
    </script>

</asp:Content>
