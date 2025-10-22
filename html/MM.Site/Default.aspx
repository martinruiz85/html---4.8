<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MM.Site._Default"
    MasterPageFile="~/MM.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <table class="table">
            <tr ng-repeat="x in Categoria">
                <td>
                    {{ x.CategoriaID }}
                </td>
                <td>
                    {{ x.NombreCategoria }}
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript">

        var app = angular.module('myApp', []);
        app.controller('myCtrl', function($scope) {

            $scope.Categoria = [];

            $scope.DataList = function() {
                
                $.ajax({
                    url: 'Service1.svc/DoWork',
                    type: 'POST',
                    dataType: 'json',
                    data: JSON.stringify({value:1}),
                    contentType: 'application/json; charset=utf-8',
                    global: true,
                    async: true,
                    beforeSend: function() {

                    }
                })
                .done(function(result) {
                    $scope.Categoria = JSON.parse(result.Datos);
                    $scope.$apply();
                })
                .fail(function(jqXHR, statusText) {                
                    if (jqXHR.status) {
                        //params.fail && params.fail(jqXHR, statusText)
                    }
                }).always(function() {
                });

            };

            window.setTimeout(function() {
                $scope.DataList();
            }, 10);

        });

        $(document).ready(function() {
        });
    
    </script>

</asp:Content>
