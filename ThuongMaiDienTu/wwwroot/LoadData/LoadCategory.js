$(document).ready(function () {
    LoadCategory();
})

function LoadCategory(){
    $.ajax({
        url: '/Category/List',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const list = $('#list-categories');
            list.empty();
            response.data.forEach(function (item) {
                list.append(`
                    <div class="card my_categoriescard">
                        <div class="my_cardimage">
                            <img src="${item.imageUrl}" class="card-img-top" alt="...">
                        </div>
                        <div class="card-body text-center">
                            <strong>${item.name}</strong>
                        </div>
                    </div>
                `);
            });

            // Khởi tạo hoặc làm mới Owl Carousel
            if (list.hasClass('owl-carousel')) {
                list.trigger('destroy.owl.carousel'); // Hủy carousel hiện tại
            }

            // Kiểm tra số lượng sản phẩm để cấu hình loop
            const loop = response.data.length > 1; // Chỉ bật loop nếu có nhiều hơn 1 sản phẩm

            list.owlCarousel({
                loop: true,
                nav: false,
                dots: false,
                autoplay: true,
                autoplayTimeout: 3000,
                responsive: {
                    0: { items: 3 },
                    600: { items: 4 },
                    750: { items: 4 },
                    1200: { items: 4 },
                    1300: { items: 5 }
                }
            });
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });

   
}