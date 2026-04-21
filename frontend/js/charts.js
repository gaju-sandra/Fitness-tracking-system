// FITTRACK PREMIUM CHARTS CONFIGURATION

document.addEventListener('DOMContentLoaded', function() {
    // Activity Chart (Area)
    const activityOptions = {
        series: [{
            name: 'Calories Burned',
            data: [310, 420, 380, 520, 480, 610, 450]
        }, {
            name: 'Active Minutes',
            data: [45, 60, 55, 80, 70, 95, 65]
        }],
        chart: {
            height: 300,
            type: 'area',
            toolbar: { show: false },
            background: 'transparent',
            foreColor: '#94a3b8'
        },
        colors: ['#3b82f6', '#8b5cf6'],
        dataLabels: { enabled: false },
        stroke: { curve: 'smooth', width: 3 },
        fill: {
            type: 'gradient',
            gradient: {
                shadeIntensity: 1,
                opacityFrom: 0.45,
                opacityTo: 0.05,
                stops: [20, 100, 100, 100]
            }
        },
        grid: {
            borderColor: 'rgba(255, 255, 255, 0.05)',
            xaxis: { lines: { show: true } }
        },
        xaxis: {
            categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            axisBorder: { show: false },
            axisTicks: { show: false }
        },
        yaxis: {
            labels: {
                formatter: function (val) { return val; }
            }
        },
        tooltip: {
            theme: 'dark',
            x: { show: false },
            marker: { show: false }
        }
    };

    const activityChart = new ApexCharts(document.querySelector("#activityChart"), activityOptions);
    activityChart.render();
});
