
const Utils = {
    numbers: ({ count, min, max }) => Array.from({ length: count }, () => Math.floor(Math.random() * (max - min + 1)) + min),
    random: (min, max) => Math.floor(Math.random() * (max - min + 1)) + min,
    randomIncrement: ({ count, min, max }) => Array.from({ length: count }, (_,i) => Math.floor(Math.random() * (max - min + 1)) + min + i*1000000),
    months: ({ count }) => ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'].slice(0, count),
    product: ({ count }) => ['Bàn gỗ', 'Ghế sofa', 'Ghế đẩu', 'Tủ quần áo', 'Tủ sách', 'Giường ngủ', 'Kệ tivi', 'Bàn ăn', 'Ghế ăn', 'Tủ giày', 'Rèm cửa', 'Bàn trà', 'Ghế bành', 'Khung ảnh', 'Đệm ngồi', 'Bàn làm việc', 'Ghế làm việc', 'Thảm trải sàn', 'Gương','Bình hoa'].slice(0, count),
    namedColor: (index) => ['#FCB454', '#00CCC0', '#E83F25', '#FF5733', '#C70039', '#4635B1', '#3A7D44', '#B771E5', '#2DAA9E', '#FFB200'][index % 9],
    color: (count) => ['#FCB454', '#00CCC0', '#E83F25', '#FF5733', '#C70039', '#4635B1', '#3A7D44', '#B771E5', '#2DAA9E', '#FFB200'].slice(0, count),
    transparentize: (color, opacity) => color + opacity,
    subtraction: (arr1, arr2) => arr1.map((num, index) => num - arr2[index]),
    getDates: ({start, end}) => Array.from({ length: (new Date(end) - new Date(start)) / 86400000 + 1 }, (_, i) => new Date(new Date(start).setDate(new Date(start).getDate() + i)).toLocaleDateString('vi-VN'))

};

(function () {
    const ctx = document.getElementById('numberOfAuctions');
    const randomdata = Utils.numbers({ count: 12, min: 10, max: 80 });
    const randomdataerror = Utils.numbers({ count: 12, min: 0, max: 10 });
    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: Utils.months({ count: 12 }),
            datasets: [{
                label: 'Số buổi tổ chức',
                data: randomdata,
                backgroundColor: '#FCB454',
                borderRadius: 4,
                stack: 'Stack 0',
            },
            {
                label: 'Số buổi hoàn thành',
                data: Utils.subtraction(randomdata, randomdataerror),
                backgroundColor: '#00CCC0',
                borderRadius: 4,
                stack: 'Stack 1',
            },
            {
                label: 'Số buổi bị hủy',
                data: randomdataerror,
                backgroundColor: '#E83F25',
                borderRadius: 4,
                stack: 'Stack 1',
            }]
        },
        options: {
            scales: {
                y: {
                    max: 90,
                    beginAtZero: true,
                    display: true,
                    
                    title: {
                        display: true,
                        text: 'Số lượng đợt mở bán',
                        color: '#191',
                        font: {
                            family: 'Times',
                            size: 20,
                            style: 'normal',
                            lineHeight: 1.2
                        },
                        padding: { top: 10, left: 0, right: 0, bottom: 0 }
                    }
                }
            },
            plugins: {
                title: {
                    display: true,
                    align: 'start',
                    //color: '#000000',
                    text: 'Số lượng đợt mở bán theo tháng',
                    font: {
                        size: 20,
                    }
                }
            },
            responsive: true,
            interaction: {
                intersect: false,
            },
        }
    });
    const actions = [
        {
            name: 'Randomize',
            handler(chart) {
                const randomdata = Utils.numbers({ count: 12, min: 10, max: 80 });
                const randomdataerror = Utils.numbers({ count: 12, min: 0, max: 10 });
                chart.data.datasets[0].data = randomdata;
                chart.data.datasets[1].data = Utils.subtraction(randomdata, randomdataerror);
                chart.data.datasets[2].data = randomdataerror;
                chart.update();
            }
        },
    ]
    document.getElementById('btn1_2021').addEventListener('click', () => actions[0].handler(myChart));
    document.getElementById('btn1_2022').addEventListener('click', () => actions[0].handler(myChart));
    document.getElementById('btn1_2023').addEventListener('click', () => actions[0].handler(myChart));
    document.getElementById('btn1_2024').addEventListener('click', () => actions[0].handler(myChart));
    

})();

(function () {
    const ctx = document.getElementById('warehouse')
    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: Utils.product({ count: 20 }),
            datasets: [{
                label: 'Số lượng sản phẩm tồn kho',
                data: Utils.numbers({ count: 20, min: 10, max: 900 }),
                backgroundColor: Utils.namedColor(Utils.random(0, 10)),
                borderRadius: 4,
            }]
        },
        options: {
            plugins: {
                scales: {
                    y: {
                        max: 900,
                    }
                },
                title: {
                    display: true,
                    align: 'start',
                    //color: '#000000',
                    text: 'Số lượng sản phẩm tồn kho',
                    font: {
                        size: 20,
                    }
                },
                padding: {
                    top: 10,  // Khoảng cách trên
                    bottom: 20 // Khoảng cách dưới (giữa title và biểu đồ)
                },
                legend: {
                    label: {
                        padding: 100,
                    },
                },
            },
            responsive: true,
            interaction: {
                intersect: false,
            },
        }
    });
    const actions = [
        {
            name: 'Randomize',
            handler(chart) {
                chart.data.datasets[0].data = Utils.numbers({ count: 20, min: 10, max: 900 });
                chart.data.datasets[0].backgroundColor = Utils.namedColor(Utils.random(0, 10));
                chart.update();
            }
        },
    ]
    document.getElementById('btn_Load_warehouse').addEventListener('click', () => actions[0].handler(myChart));
})();

(function () {
    function skipped(ctx, value) {
        return ctx.p0.skip || ctx.p1.skip ? value : undefined;
    }

    function down(ctx, value) {
        return ctx.p0.parsed.y > ctx.p1.parsed.y ? value : undefined;
    }
    const ctx = document.getElementById('linechart');

    const myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: Utils.getDates({ start: '2024/01/01', end: '2024/12/31' }),
            datasets: [{
                label: 'Tiền',
                data: Utils.randomIncrement({ count: 365, min: 0, max: 150000000 }),
                borderColor: '#06D001',
                backgroundColor: '#4379F2',
                segment: {
                    borderColor: ctx => skipped(ctx, 'rgb(0,0,0,0.2)') || down(ctx, '#E83F25'),
                    borderDash: ctx => skipped(ctx, [6, 6]),
                },
                
            }]
        },
        options: {
            scales: {
                y: {
                    max: 600000000,
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString('vi-VN') + ' VNĐ';
                        }
                    }
                }
            },
            responsive: true,
            plugins: {
                legend: {
                    display: false,
                },
                tooltip: {
                    callbacks: {
                        label: function (context) {
                            const label = context.dataset.label || '';
                            const value = context.raw.toLocaleString('vi-VN'); // Format số có dấu phẩy
                            return `${label}: ${value} VNĐ`;
                        }
                    }
                }
            },
            interaction: {
                intersect: false,
            },
        }
    });
    const actions = [
        {
            name: 'Randomize',
            handler(chart, year) {
                chart.data.labels = Utils.getDates({ start: `${year}/01/01`, end: `${year}/12/31` });
                chart.data.datasets[0].data = Utils.randomIncrement({ count: 365, min: 0, max: 150000000 }),
                chart.update();
            }
        },
    ]

    document.getElementById('btn2_2021').addEventListener('click', () => actions[0].handler(myChart, 2021));
    document.getElementById('btn2_2022').addEventListener('click', () => actions[0].handler(myChart, 2022));
    document.getElementById('btn2_2023').addEventListener('click', () => actions[0].handler(myChart, 2023));
    document.getElementById('btn2_2024').addEventListener('click', () => actions[0].handler(myChart, 2024));
})();
