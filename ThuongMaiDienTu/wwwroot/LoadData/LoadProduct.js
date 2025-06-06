$(document).ready(function () {
    LoadProduct();
});

function LoadProduct() {
    $.ajax({
        url: '/Product/List',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const list = $('.list_product');
            list.empty();

            response.data.forEach(function (item, index) {
                // Tạo ID riêng cho mỗi countdown
                const countdownId = `countdown-${index}`;

                list.append(`
                    <div class="card my_card">
                        <div class="my_countdown d-flex justify-content-center align-items-center">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                                <path d="M464 256A208 208 0 1 1 48 256a208 208 0 1 1 416 0zM0 256a256 256 0 1 0 512 0A256 256 0 1 0 0 256zM232 120l0 136c0 8 4 15.5 10.7 20l96 64c11 7.4 25.9 4.4 33.3-6.7s4.4-25.9-6.7-33.3L280 243.2 280 120c0-13.3-10.7-24-24-24s-24 10.7-24 24z" />
                            </svg>
                            <span id="${countdownId}" data-end="${item.end}"></span>
                        </div>
                        <div class="my_cardimage d-flex justify-content-center align-items-center">
                            <div class="my_overlay d-flex justify-content-center align-items-center">
                                <a onClick="CreateFavourite(${item.id})" class="btn my_overlaybtn d-flex justify-content-center align-items-center">
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
                                        <path d="M225.8 468.2l-2.5-2.3L48.1 303.2C17.4 274.7 0 234.7 0 192.8l0-3.3c0-70.4 50-130.8 119.2-144C158.6 37.9 198.9 47 231 69.6c9 6.4 17.4 13.8 25 22.3c4.2-4.8 8.7-9.2 13.5-13.3c3.7-3.2 7.5-6.2 11.5-9c0 0 0 0 0 0C313.1 47 353.4 37.9 392.8 45.4C462 58.6 512 119.1 512 189.5l0 3.3c0 41.9-17.4 81.9-48.1 110.4L288.7 465.9l-2.5 2.3c-8.2 7.6-19 11.9-30.2 11.9s-22-4.2-30.2-11.9zM239.1 145c-.4-.3-.7-.7-1-1.1l-17.8-20-.1-.1s0 0 0 0c-23.1-25.9-58-37.7-92-31.2C81.6 101.5 48 142.1 48 189.5l0 3.3c0 28.5 11.9 55.8 32.8 75.2L256 430.7 431.2 268c20.9-19.4 32.8-46.7 32.8-75.2l0-3.3c0-47.3-33.6-88-80.1-96.9c-34-6.5-69 5.4-92 31.2c0 0 0 0-.1 .1s0 0-.1 .1l-17.8 20c-.3 .4-.7 .7-1 1.1c-4.5 4.5-10.6 7-16.9 7s-12.4-2.5-16.9-7z" />
                                    </svg>
                                </a>
                                <a href="/Product/Details/${item.id}" class="btn my_overlaybtn d-flex justify-content-center align-items-center">
                                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                                        <path d="M288 80c-65.2 0-118.8 29.6-159.9 67.7C89.6 183.5 63 226 49.4 256c13.6 30 40.2 72.5 78.6 108.3C169.2 402.4 222.8 432 288 432s118.8-29.6 159.9-67.7C486.4 328.5 513 286 526.6 256c-13.6-30-40.2-72.5-78.6-108.3C406.8 109.6 353.2 80 288 80zM95.4 112.6C142.5 68.8 207.2 32 288 32s145.5 36.8 192.6 80.6c46.8 43.5 78.1 95.4 93 131.1c3.3 7.9 3.3 16.7 0 24.6c-14.9 35.7-46.2 87.7-93 131.1C433.5 443.2 368.8 480 288 480s-145.5-36.8-192.6-80.6C48.6 356 17.3 304 2.5 268.3c-3.3-7.9-3.3-16.7 0-24.6C17.3 208 48.6 156 95.4 112.6zM288 336c44.2 0 80-35.8 80-80s-35.8-80-80-80c-.7 0-1.3 0-2 0c1.3 5.1 2 10.5 2 16c0 35.3-28.7 64-64 64c-5.5 0-10.9-.7-16-2c0 .7 0 1.3 0 2c0 44.2 35.8 80 80 80zm0-208a128 128 0 1 1 0 256 128 128 0 1 1 0-256z" />
                                    </svg>
                                </a>                               
                            </div>
                            <img src="${item.imageUrl}" alt="Ảnh sản phẩm">
                        </div>
                        <div class="card-body my_card_body">
                            <h5 class="card-title my_card_title">${item.name}</h5>
                            <p class="my_pricestyle mt-2">${item.min.toLocaleString('vi-VN')} - ${item.max.toLocaleString('vi-VN')}</p>
                            <div class="progress my_progress">
                                <div class="progress-bar progress-bar-striped" title="${item.registration}" role="progressbar"
                                     style="width: ${item.registration*100/item.quantity}%"
                                     aria-valuenow="${item.registration * 100 / item.quantity}" aria-valuemin="0" aria-valuemax="100">
                                </div>
                            </div>
                        </div>
                    </div>
                `);
            });

            $('.list_product.owl-carousel').each(function () {
                const $list = $(this);

                $list.trigger('destroy.owl.carousel');
                $list.owlCarousel({
                    loop: $list.children().length > 3,
                    nav: false,
                    dots: false,
                    autoplay: true,
                    autoplayTimeout: 3000,
                    responsive: {
                        0: { items: 2 },
                        600: { items: 3 },
                        750: { items: 3 },
                        1000: { items: 3 },
                        1300: { items: 4 }
                    }
                });
            });

            // Cập nhật countdown mỗi giây
            setInterval(function () {
                $('[id^=countdown-]').each(function () {
                    const endTime = new Date($(this).data('end')).getTime();
                    const now = new Date().getTime();
                    const distance = endTime - now;

                    if (distance <= 0) {
                        $(this).text("Đã kết thúc");
                    } else {
                        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
                        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                        const seconds = Math.floor((distance % (1000 * 60)) / 1000);

                        $(this).text(`${days}d ${hours}h ${minutes}m ${seconds}s`);
                    }
                });
            }, 1000);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}

function CreateFavourite(id) {
    $.ajax({
        url: `/Guest/Favourite/${id}`,
        type: 'POST',
        success: function (data) {
            if (data.redirect) {
                Swal.fire({
                    title: 'Yêu cầu đăng nhập',
                    text: data.message || 'Vui lòng đăng nhập trước khi thêm vào yêu thích.',
                    icon: 'warning',
                    confirmButtonText: 'Đăng nhập'
                }).then(() => {
                    window.location.href = data.redirect;
                });
            } else if (data.success && data.forwardTo) {
                // Nếu được xác nhận là customer, gọi tiếp API thực hiện tạo
                $.ajax({
                    url: data.forwardTo,
                    type: 'POST',
                    success: function (res) {
                        if (res.success) {
                            Swal.fire({
                                title: 'Thành công!',
                                text: 'Sản phẩm đã được thêm vào yêu thích.',
                                icon: 'success',
                                confirmButtonText: 'OK'
                            });
                        } else {
                            Swal.fire({
                                title: 'Thất bại!',
                                text: res.message || 'Không thể thêm vào yêu thích.',
                                icon: 'error',
                                confirmButtonText: 'OK'
                            });
                        }
                    },
                    error: function () {
                        Swal.fire({
                            title: 'Lỗi!',
                            text: 'Đã xảy ra lỗi khi thêm vào yêu thích.',
                            icon: 'error',
                            confirmButtonText: 'OK'
                        });
                    }
                });
            } else {
                Swal.fire({
                    title: 'Không thể thực hiện',
                    text: data.message || 'Hành động không hợp lệ.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        },
        error: function () {
            Swal.fire({
                title: 'Lỗi!',
                text: 'Đã xảy ra lỗi khi gửi yêu cầu.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    });
}
