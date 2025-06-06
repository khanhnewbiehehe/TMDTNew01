$(document).ready(function () {    // Get search parameters from URL
    const urlParams = new URLSearchParams(window.location.search);
    const searchTerm = urlParams.get('searchTerm') || '';
    const categoryId = parseInt(urlParams.get('categoryId')) || 0;
    const minPrice = parseFloat(urlParams.get('minPrice')) || 0;
    // Handle empty maxPrice or 0 value as no upper limit
    const maxPrice = urlParams.get('maxPrice') ? (parseFloat(urlParams.get('maxPrice')) || 0) : '';
    const page = parseInt(urlParams.get('page')) || 1;
    const pageSize = parseInt(urlParams.get('pageSize')) || 12;
    // Get status filter values, default to true if not specified
    const launchGoing = urlParams.get('LaunchGoing') !== 'false';
    const launchFull = urlParams.get('LaunchFull') !== 'false';
    const launchEnded = urlParams.get('LaunchEnded') !== 'false';

    // Call function to load search products
    LoadSearchProducts(searchTerm, categoryId, minPrice, maxPrice, launchGoing, launchFull, launchEnded, page, pageSize);

    // We'll remove the click handler for price-range as we're using the slider now
    // Handle click on active filters "x" icon to remove that filter
    $(document).on('click', '.badge .bi-x', function (e) {
        e.stopPropagation(); // Stop event propagation
        const badge = $(this).closest('.badge');
        const filterType = badge.data('filter-type') || '';
        console.log('Badge X clicked:', badge.text().trim(), 'Filter type:', filterType); // Debug info

        if (filterType === 'search' || badge.text().includes('Search:')) {
            console.log('Resetting search term');
            $('#searchInput, #searchInputMobile, input[name="searchTerm"]').val('');
        } else if (filterType === 'category' || badge.text().includes('Category:')) {
            console.log('Resetting category');
            $('input[id="category-0"]').prop('checked', true);
        } else if (filterType === 'price' || badge.text().includes('Giá:')) {
            console.log('Resetting price range');
            // Reset price range filter
            $('input[name="minPrice"]').val(0);
            $('input[name="maxPrice"]').val('');
            $('#priceOption-all').prop('checked', true);
            $('#priceOption-custom').prop('checked', false);

            // If noUiSlider is initialized, reset it
            const slider = document.getElementById('price-range-slider');
            if (slider && slider.noUiSlider) {
                slider.noUiSlider.set([0, 20000000]);
            }
        } else if (filterType === 'status' || badge.text().includes('Trạng thái:')) {
            console.log('Resetting status filters');
            // Reset status filters - check all status checkboxes
            $('#status-all').prop('checked', true);
            $('#status-ongoing').prop('checked', true);
            $('#status-full').prop('checked', true);
            $('#status-ended').prop('checked', true);
            // Add hidden inputs for true values
            $('input[name="LaunchGoing"]').val(true);
            $('input[name="LaunchFull"]').val(true);
            $('input[name="LaunchEnded"]').val(true);
        }
        // Submit the form after a slight delay to ensure values are updated
        setTimeout(() => {
            console.log('Submitting search form after filter badge click');
            $('#searchForm').submit();
        }, 100);
    });
});

function LoadSearchProducts(searchTerm, categoryId, minPrice, maxPrice, launchGoing, launchFull, launchEnded, page, pageSize) {
    $.ajax({
        url: '/Customer/Search/GetFilteredProducts',
        type: 'GET',
        data: {
            searchTerm: searchTerm,
            categoryId: categoryId,
            minPrice: minPrice,
            maxPrice: maxPrice,
            LaunchGoing: launchGoing,
            LaunchFull: launchFull,
            LaunchEnded: launchEnded,
            page: page,
            pageSize: pageSize
        },
        dataType: 'json',
        success: function (response) {
            const list = $('#list_search_product');
            list.empty();

            // Create row container for products
            let currentRow;            // Display products in a grid layout (4 products per row)
            // Apply client-side status filtering for better UX
            const filteredData = response.data.filter(item => {
                const now = new Date().getTime();
                const endTime = new Date(item.end).getTime();
                const isEnded = endTime <= now;
                const isFull = item.registration >= item.quantity;

                // Only show products that match the selected status filters
                return (isEnded && launchEnded) ||
                    (isFull && !isEnded && launchFull) ||
                    (!isFull && !isEnded && launchGoing);
            });

            // Update results count
            $('#results-count').text(`Đã tìm thấy ${filteredData.length} kết quả`);
            filteredData.forEach(function (item, index) {
                // Create a new row for every 3 products (larger cards)
                if (index % 3 === 0) {
                    currentRow = $('<div class="row g-3 mb-4"></div>');
                    list.append(currentRow);
                }

                // Create column for the product card
                const column = $('<div class="col-md-4 mb-3"></div>');
                // Determine badge status based on time and registration
                let badgeHtml = '';
                const now = new Date().getTime();
                const endTime = new Date(item.end).getTime();
                const isEnded = endTime <= now;
                const isFull = item.registration >= item.quantity;

                if (isEnded) {
                    badgeHtml = '<div class="position-absolute top-0 start-0 badge-ended">KẾT THÚC</div>';
                } else if (isFull) {
                    badgeHtml = '<div class="position-absolute top-0 start-0 badge-full">ĐÃ ĐẦY</div>';
                } else {
                    // Create a unique countdown ID for each product
                    const countdownId = `search-countdown-${index}`;
                    badgeHtml = `
                        <div class="position-absolute top-0 start-0 badge-ongoing">
                            <small class="d-flex align-items-center gap-1">
                                <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" fill="currentColor" viewBox="0 0 512 512">
                                    <path d="M464 256A208 208 0 1 1 48 256a208 208 0 1 1 416 0zM0 256a256 256 0 1 0 512 0A256 256 0 1 0 0 256zM232 120l0 136c0 8 4 15.5 10.7 20l96 64c11 7.4 25.9 4.4 33.3-6.7s4.4-25.9-6.7-33.3L280 243.2 280 120c0-13.3-10.7-24-24-24s-24 10.7-24 24z" />
                                </svg>
                                <span id="${countdownId}" data-end="${item.end}"></span>
                            </small>
                        </div>
                    `;
                } const card = `
                    <div class="card product-card h-100 position-relative">
                        ${badgeHtml}
                        <div class="product-image-container">
                            <img src="${item.imageUrl}" alt="${item.name}" class="product-image">
                            <div class="product-overlay">
                                <a onClick="CreateFavourite(${item.id})" class="btn btn-sm btn-light rounded-circle me-2">
                                    <i class="bi bi-heart"></i>
                                </a>
                                <a href="/Product/Details/${item.id}" class="btn btn-sm btn-light rounded-circle">
                                    <i class="bi bi-eye"></i>
                                </a>
                            </div>
                        </div>
                        <div class="card-body p-3">
                            <h6 class="product-title">${item.name}</h6>
                            <p class="product-price mb-2">${item.min.toLocaleString('vi-VN')}₫ - ${item.max.toLocaleString('vi-VN')}₫</p>
                            <div class="progress mb-2">
                                <div class="progress-bar" 
                                     role="progressbar" 
                                     title="${item.registration}/${item.quantity}"
                                     style="width: ${Math.min((item.registration * 100 / item.quantity), 100)}%; background-color: #2484C2;" 
                                     aria-valuenow="${item.registration}" 
                                     aria-valuemin="0" 
                                     aria-valuemax="${item.quantity}">
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                column.html(card);
                currentRow.append(column);
            });            // Add empty message if no products found
            if (filteredData.length === 0) {
                list.html(`
                    <div class="text-center py-5 bg-white border">
                        <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" fill="#ccc" class="bi bi-search mb-3" viewBox="0 0 16 16">
                            <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
                        </svg>
                        <h5>Không tìm thấy sản phẩm nào</h5>
                        <p class="text-muted">Vui lòng thử lại với các tiêu chí tìm kiếm khác</p>
                    </div>
                `);
            }

            // Add hover effect to product cards
            $('.card').hover(
                function () {
                    $(this).find('.bg-dark').removeClass('opacity-0');
                },
                function () {
                    $(this).find('.bg-dark').addClass('opacity-0');
                }
            );            // Update countdowns every second
            updateCountdowns();            // Add pagination if needed
            if (filteredData.length > 0 && response.total > pageSize) {
                const totalPages = Math.ceil(response.total / pageSize);
                const paginationContainer = $('<div class="pagination-container"><nav aria-label="Page navigation"><ul class="pagination justify-content-center mb-0"></ul></nav></div>');
                const paginationList = paginationContainer.find('ul');
                // Previous button
                paginationList.append(`
                    <li class="page-item ${page <= 1 ? 'disabled' : ''}">
                        <a class="page-link" href="#" data-page="${parseInt(page) - 1}" aria-label="Previous">
                            <span aria-hidden="true">
                                <i class="bi bi-chevron-left"></i>
                            </span>
                        </a>
                    </li>
                `);

                // Page numbers - limit shown pages for better UX
                const maxVisiblePages = 5;
                let startPage = Math.max(1, page - Math.floor(maxVisiblePages / 2));
                let endPage = Math.min(totalPages, startPage + maxVisiblePages - 1);

                if (endPage - startPage + 1 < maxVisiblePages) {
                    startPage = Math.max(1, endPage - maxVisiblePages + 1);
                }

                // Always show first page
                if (startPage > 1) {
                    paginationList.append(`
                        <li class="page-item">
                            <a class="page-link" href="#" data-page="1">1</a>
                        </li>
                    `);
                    if (startPage > 2) {
                        paginationList.append(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
                    }
                }

                // Page numbers
                for (let i = startPage; i <= endPage; i++) {
                    paginationList.append(`
                        <li class="page-item ${i == page ? 'active' : ''}">
                            <a class="page-link" href="#" data-page="${i}">${i}</a>
                        </li>
                    `);
                }

                // Always show last page
                if (endPage < totalPages) {
                    if (endPage < totalPages - 1) {
                        paginationList.append(`<li class="page-item disabled"><span class="page-link">...</span></li>`);
                    }
                    paginationList.append(`
                        <li class="page-item">
                            <a class="page-link" href="#" data-page="${totalPages}">${totalPages}</a>
                        </li>
                    `);
                }
                // Next button
                paginationList.append(`
                    <li class="page-item ${page >= totalPages ? 'disabled' : ''}">
                        <a class="page-link" href="#" data-page="${parseInt(page) + 1}" aria-label="Next">
                            <span aria-hidden="true">
                                <i class="bi bi-chevron-right"></i>
                            </span>
                        </a>
                    </li>
                `);

                // Add pagination to the DOM
                list.append(paginationContainer);// Handle pagination clicks
                $('.pagination .page-link').on('click', function (e) {
                    e.preventDefault();
                    if (!$(this).parent().hasClass('disabled')) {
                        const newPage = $(this).data('page');
                        LoadSearchProducts(searchTerm, categoryId, minPrice, maxPrice, launchGoing, launchFull, launchEnded, newPage, pageSize);
                    }
                });
            }
        }, error: function (xhr, status, error) {
            console.error('Error loading search products:', error);
            console.error('Status:', status);
            console.error('Response:', xhr.responseText);

            let errorMessage = 'Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.';
            if (xhr.status === 404) {
                errorMessage = 'Không tìm thấy đường dẫn API. Vui lòng kiểm tra lại cấu hình.';
            } else if (xhr.status === 500) {
                errorMessage = 'Lỗi máy chủ. Vui lòng thử lại sau.';
            }

            $('#list_search_product').html(`<div class="alert alert-danger">${errorMessage} (Status: ${xhr.status})</div>`);
        }
    });
}

// Function to update all countdowns
function updateCountdowns() {
    const countdownTimer = setInterval(function () {
        $('[id^=search-countdown-]').each(function () {
            const endTime = new Date($(this).data('end')).getTime();
            const now = new Date().getTime();
            const distance = endTime - now;

            if (distance <= 0) {
                $(this).closest('.card').find('.position-absolute.top-0.start-0')
                    .removeClass('bg-light')
                    .addClass('bg-danger')
                    .css('background-color', '#EE5858 !important')
                    .html('KẾT THÚC');

                // If all countdowns are finished, clear the interval
                if ($('[id^=search-countdown-]').length === $('[id^=search-countdown-]:contains("Đã kết thúc")').length) {
                    clearInterval(countdownTimer);
                }
            } else {
                const days = Math.floor(distance / (1000 * 60 * 60 * 24));
                const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                const seconds = Math.floor((distance % (1000 * 60)) / 1000);

                $(this).text(`${days}d ${hours}h ${minutes}m ${seconds}s`);
            }
        });
    }, 1000);
}

// Reuse existing CreateFavourite function from LoadProduct.js
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
                                text: res.message || 'Đã thêm vào danh sách yêu thích.',
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
                            text: 'Đã xảy ra lỗi khi gửi yêu cầu.',
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