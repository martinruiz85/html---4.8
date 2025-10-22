<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/Chart.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-3.4.1.min.js" type="text/javascript"></script>

    <script src="js/Chart.js" type="text/javascript"></script>

    <script src="js/Chart.bundle.js" type="text/javascript"></script>

    <script type="text/javascript">

        window.onload = function() {

            /*
            if (new Date("2018", "10", "08") > new Date("2018", "09", "01"))
            alert("es mayor");
            else
            alert("es menor");
            */

        };
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 200px; height: 200px;">
        <canvas id="myChart" width="0" height="0"></canvas>
    </div>
    </form>
</body>

<script type="text/javascript">

    ///https: //www.chartjs.org/docs/latest/

    /*
    var ctx = document.getElementById('myChart').getContext('2d');
    var myChart = new Chart(ctx, {
    type: 'bar',
    data: {
    labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
    datasets: [{
    label: '# of Votes',
    data: [12, 19, 3, 5, 2, 3],
    backgroundColor: [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
    ],
    borderColor: [
    'rgba(255, 99, 132, 1)',
    'rgba(54, 162, 235, 1)',
    'rgba(255, 206, 86, 1)',
    'rgba(75, 192, 192, 1)',
    'rgba(153, 102, 255, 1)',
    'rgba(255, 159, 64, 1)'
    ],
    borderWidth: 1
    }]
    },
    options: {
    scales: {
    yAxes: [{
    ticks: {
    beginAtZero: true
    }
    }]
    }
    }
    });
    */

    var ctx = document.getElementById("myChart").getContext("2d");

    var data = {
        labels: ["Chocolate", "Vanilla", "Strawberry"],
        datasets: [{
            label: "Blue",
            backgroundColor: "rgba(255, 99, 132, 1)",
            data: [3, 7, 4]
        }, {
            label: "Red",
            backgroundColor: "rgba(54, 162, 235, 1)",
            data: [4, 3, 5]
        }, {
            label: "Green",
            backgroundColor: "rgba(255, 206, 86, 1)",
            data: [7, 2, 6]
}]
        };

        var myBarChart = new Chart(ctx, {
            type: 'bar',
            data: data,
            options: {
                barValueSpacing: 20,
                scales: {
                    yAxes: [{
                        ticks: {
                            min: 0
                        }
}]
                    }
                }
            });

            window.setTimeout(function() {

                myBarChart.data.datasets = [{
                    label: "Blue",
                    backgroundColor: "rgba(255, 99, 132, 1)",
                    data: [4, 3, 1]
                }, {
                    label: "Red",
                    backgroundColor: "rgba(54, 162, 235, 1)",
                    data: [5, 4, 4]
                }, {
                    label: "Green",
                    backgroundColor: "rgba(255, 206, 86, 1)",
                    data: [8, 5, 9]}];
                    
                    myBarChart.update();


                }, 1000);

            
</script>

</html>
