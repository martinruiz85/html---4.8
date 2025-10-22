<%@ Page Title="" Language="C#" MasterPageFile="~/MM.Master" AutoEventWireup="true"
    CodeBehind="Producto.aspx.cs" Inherits="MM.Site.Producto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ul class="list-group">
        <li ng-repeat="row in ProductosList.Table1" class="list-group-item" class="list-group-item">
            <div class="row">
                <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                    <h6>
                        {{row.NombreProducto}}<h6>
                            <span>{{row.DescripcionProducto}}</span>
                </div>
                <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                    {{row.Precio}}
                </div>
                <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4">
                    <div class="dropdown drop">
                        <button type="button" class="btn btn-primary dropdown-toggle" style="background: #FF0080;
                            border-color: #FF0080; font-size: 12px;" data-toggle="dropdown">
                            Opciones
                        </button>
                        <div class="dropdown-menu">
                            <a class="dropdown-item" href="/DetalleProducto.aspx?ProductoID={{row.ProductoID}}">
                                Editar</a> <a class="dropdown-item" href="#">Borrar</a>
                        </div>
                    </div>
                </div>
            </div>
        </li>
    </ul>

    <script type="text/javascript">
        var app = angular.module('myApp', ['ui.bootstrap']);
        app.controller('myCtrl', function($scope) {
            $scope.ProductosList = {};

            $scope.currentPage = 1;
            $scope.maxSize = 5;

            $scope.setPage = function(pageNo) {
                $scope.currentPage = pageNo;
                $scope.DataList($scope.currentPage);
            };

            $scope.pageChanged = function() {
                $scope.DataList($scope.currentPage);
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

                    }
                })
                .done(function(result) {
                    $scope.ProductosList = JSON.parse(result.Datos);
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
                });

            };

            $scope.DataList(1);

        });
    </script>

</asp:Content>
