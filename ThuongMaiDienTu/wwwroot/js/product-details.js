/**
 * Product Details Page Javascript
 */

// Toggle Card Body Display for Product Description and Reviews
document.addEventListener('DOMContentLoaded', function() {
    // Handle description and review toggles
    document.querySelectorAll('.toggle-arrow').forEach(arrow => {
        arrow.addEventListener('click', function(e) {
            e.preventDefault();
            e.stopPropagation();
            
            const subCard = this.closest('.sub-card');
            const cardBody = subCard.querySelector('.card-body');
            
            if (cardBody.classList.contains('d-none')) {
                cardBody.classList.remove('d-none');
                this.classList.add('rotated');
            } else {
                cardBody.classList.add('d-none');
                this.classList.remove('rotated');
            }
        });
    });

    // Initialize countdown on page load
    updateCountdown();
    
    // Update countdown every second
    setInterval(updateCountdown, 1000);
    
    // Initialize mini-cards highlighting
    initMiniCards();
});

/**
 * Updates the countdown display with appropriate styling
 */
function updateCountdown() {
    const countdownEl = document.getElementById('countdown--1');
    if (!countdownEl) return;
    
    const endTime = new Date(countdownEl.getAttribute('data-end')).getTime();
    const isEnded = countdownEl.getAttribute('data-ended') === 'true';
    const isFull = countdownEl.getAttribute('data-full') === 'true';
    const now = new Date().getTime();
    const distance = endTime - now;    // Clear previous classes
    countdownEl.classList.remove('countdown-ended', 'countdown-full');
    
    if (isEnded) {
        countdownEl.textContent = "Đã kết thúc";
        countdownEl.classList.add('countdown-ended');
    } else if (isFull) {
        countdownEl.textContent = "Đã full";
        countdownEl.classList.add('countdown-full');
    } else if (distance <= 0) {
        countdownEl.textContent = "Đã kết thúc";
        countdownEl.classList.add('countdown-ended');
    } else {
        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);
        
        countdownEl.textContent = `${days}d ${hours}h ${minutes}m ${seconds}s`;
    }
}

/**
 * Initializes mini-cards with hover effects
 */
function initMiniCards() {
    document.querySelectorAll('.mini-card').forEach(card => {
        card.addEventListener('mouseenter', function() {
            if (!this.classList.contains('active')) {
                this.style.transform = 'scale(1.03)';
                this.style.boxShadow = '0 4px 8px rgba(0,0,0,0.1)';
            }
        });
        
        card.addEventListener('mouseleave', function() {
            if (!this.classList.contains('active')) {
                this.style.transform = 'scale(1)';
                this.style.boxShadow = '';
            }
        });
    });
}

/**
 * Updates the total quantity count in the shopping cart and progress bar
 */
function updateTotal() {
    let total = 0;
    let totalValue = 0;
    const currentRegistration = parseInt(document.getElementById('CurrentRegistration').value);
    const priceData = JSON.parse(document.getElementById('PriceData').value);
    const maxPrice = parseInt(document.getElementById('GiaTien').value);
    
    // Calculate total quantity and individual product values
    document.querySelectorAll('.quantity-input input').forEach(input => {
        const value = parseInt(input.value) || 0;
        total += value;
        
        // Calculate value for each product using max price (for deposit calculation)
        totalValue += value * maxPrice;
    });
    
    // Calculate current price level based on total quantity + current registration
    const totalQuantity = currentRegistration + total;
    let currentPrice = maxPrice; // Default to max price
    
    // Find the lowest price tier achieved (best price)
    const achievedPrices = priceData.filter(p => p.Number <= totalQuantity);
    if (achievedPrices.length > 0) {
        currentPrice = Math.min(...achievedPrices.map(p => p.Price));
    }
    
    // Calculate final value using current price level
    const finalValue = total * currentPrice;
    
    // Calculate deposit (30% of max price value for risk coverage)
    const depositAmount = totalValue * 0.3;
    
    // Update display
    document.getElementById('Total-product').textContent = total;
    document.getElementById('current-price-level').textContent = currentPrice.toLocaleString() + ' VND';
    document.getElementById('total-value').textContent = finalValue.toLocaleString() + ' VND';
    document.getElementById('deposit-amount').textContent = depositAmount.toLocaleString() + ' VND';
      // Update progress bar
    updateProgressBar(total);
    
    // Update mini-cards highlighting
    updateMiniCards(totalQuantity);
}

/**
 * Updates the progress bar for price levels
 */
function updateProgressBar(selectedQuantity) {
    const currentRegistration = parseInt(document.getElementById('CurrentRegistration').value);
    const maxQuantity = parseInt(document.getElementById('MaxQuantity').value);
    const priceData = JSON.parse(document.getElementById('PriceData').value);
    
    const totalQuantity = currentRegistration + selectedQuantity;
    
    // Find next price level
    const nextPriceLevel = priceData.find(p => p.Number > totalQuantity);
    const currentPriceLevel = priceData.filter(p => p.Number <= totalQuantity).pop();
    
    let progressPercent = 0;
    let needed = 0;
    let nextPriceText = "";
    let targetQuantity = maxQuantity;
    
    if (nextPriceLevel) {
        needed = nextPriceLevel.Number - totalQuantity;
        progressPercent = (totalQuantity / nextPriceLevel.Number) * 100;
        nextPriceText = nextPriceLevel.Price.toLocaleString() + " VND";
        targetQuantity = nextPriceLevel.Number;
    } else if (totalQuantity < maxQuantity) {
        needed = maxQuantity - totalQuantity;
        progressPercent = (totalQuantity / maxQuantity) * 100;
        nextPriceText = "Đã đạt giá tốt nhất";
    } else {
        progressPercent = 100;
        nextPriceText = "Đã đạt tối đa";
    }
    
    // Update progress bar
    const progressBar = document.querySelector('.price-progress .progress-bar');
    if (progressBar) {
        progressBar.style.width = progressPercent + '%';
        progressBar.setAttribute('aria-valuenow', progressPercent);
    }
    
    // Update text
    const quantityNeededElement = document.getElementById('quantity-needed');
    if (quantityNeededElement) {
        if (needed > 0 && nextPriceLevel) {
            quantityNeededElement.textContent = `Cần thêm ${needed} để đạt ${nextPriceText}`;
            quantityNeededElement.className = 'text-primary fw-bold';
        } else {
            quantityNeededElement.textContent = nextPriceText;
            quantityNeededElement.className = 'text-success fw-bold';
        }
    }
    
    // Update current quantity badge
    const currentBadge = document.querySelector('.price-progress .badge.bg-info');
    if (currentBadge) {
        currentBadge.textContent = totalQuantity;
    }
    
    // Update target quantity badge
    const targetBadge = document.querySelector('.price-progress .badge.bg-success');
    if (targetBadge && nextPriceLevel) {
        targetBadge.textContent = targetQuantity;
    }
}

/**
 * Initializes quantity input controls
 */
document.addEventListener('DOMContentLoaded', function() {
    document.querySelectorAll('.quantity-input').forEach(container => {
        const input = container.querySelector('input');
        const btnDecrease = container.querySelector('.decrease');
        const btnIncrease = container.querySelector('.increase');

        btnDecrease.addEventListener('click', () => {
            let value = parseInt(input.value) || 0;
            input.value = value > 0 ? value - 1 : 0;
            updateTotal();
        });        btnIncrease.addEventListener('click', () => {
            let value = parseInt(input.value) || 0;
            const maxStock = parseInt(input.getAttribute('data-stock')) || 0;
            const newValue = value + 1;
            
            if (newValue <= maxStock) {
                input.value = newValue;
                updateTotal();
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Vượt quá số lượng',
                    text: `Chỉ còn ${maxStock} sản phẩm trong kho!`,
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        });        input.addEventListener('input', () => {
            let value = parseInt(input.value);
            const maxStock = parseInt(input.getAttribute('data-stock')) || 0;
            
            if (isNaN(value) || value < 0) {
                input.value = 0;
            } else if (value > maxStock) {
                input.value = maxStock;
                Swal.fire({
                    icon: 'warning',
                    title: 'Vượt quá số lượng',
                    text: `Chỉ còn ${maxStock} sản phẩm trong kho!`,
                    showConfirmButton: false,
                    timer: 2000
                });
            }
            updateTotal();
        });

        input.addEventListener('blur', () => {
            // Clear leading zeros
            let value = parseInt(input.value) || 0;
            input.value = value;
        });
    });    // Calculate total on page load
    updateTotal();
    
    // Initialize progress bar on page load
    if (document.getElementById('PriceData')) {
        updateProgressBar(0);
        // Initialize mini-cards highlighting
        const currentRegistration = parseInt(document.getElementById('CurrentRegistration').value);
        updateMiniCards(currentRegistration);
    }
});

/**
 * Adds product to favorites
 */
function CreateFavourite(id) {
    $.ajax({
        url: `/Customer/Favourite/Create/${id}`,
        type: 'POST',
        success: function(data) {
            if (data) {
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Sản phẩm đã được thêm vào yêu thích.',
                    icon: 'success',
                    confirmButtonText: 'OK'
                });
            } else {
                Swal.fire({
                    title: 'Thất bại!',
                    text: 'Không thể thêm vào yêu thích.',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        },
        error: function() {
            Swal.fire({
                title: 'Lỗi!',
                text: 'Đã xảy ra lỗi khi gửi yêu cầu.',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    });
}

/**
 * Starts the ordering process
 */
function StartOrder() {
    const total = Array.from(document.querySelectorAll(".quantity"))
        .reduce((sum, input) => sum + parseInt(input.value || 0), 0);

    const soLuongKhaDung = parseInt(document.getElementById("SoLuongKhaDung").value);

    if (total === 0) {
        Swal.fire({
            icon: 'warning',
            title: 'Chưa chọn sản phẩm',
            text: 'Bạn phải chọn ít nhất 1 sản phẩm để đặt hàng!',
        });
        return;
    }

    if (total > soLuongKhaDung) {
        Swal.fire({
            icon: 'error',
            title: 'Không thể đặt hàng',
            text: 'Số lượng đặt vượt quá số lượng khả dụng!',
        });
        return;
    }

    // Validate individual product stock limits
    let stockExceeded = false;
    let stockErrorMessage = '';
    
    document.querySelectorAll(".quantity").forEach(input => {
        const quantity = parseInt(input.value || 0);
        const maxStock = parseInt(input.getAttribute('data-stock')) || 0;
        const productName = input.getAttribute('data-name');
        
        if (quantity > maxStock) {
            stockExceeded = true;
            stockErrorMessage = `Sản phẩm "${productName}" chỉ còn ${maxStock} trong kho, bạn đã chọn ${quantity}!`;
        }
    });
    
    if (stockExceeded) {
        Swal.fire({
            icon: 'error',
            title: 'Vượt quá tồn kho',
            text: stockErrorMessage,
        });
        return;
    }

    const items = [];
    document.querySelectorAll(".quantity").forEach(input => {
        const quantity = parseInt(input.value || 0);
        if (quantity > 0) {
            items.push({
                productTypeId: parseInt(input.dataset.typeid),
                quantity: quantity,
                amount: quantity * $('#GiaTien').val()
            });
        }
    });

    $.ajax({
        url: '/Customer/Invoice/PreOrder',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(items),
        success: function() {
            // Redirect after successful POST
            window.location.href = '/Customer/Invoice/Create';
        },
        error: function(err) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi gửi đơn hàng',
                text: 'Vui lòng thử lại hoặc kiểm tra kết nối.'
            });
        }
    });
}

/**
 * Updates mini-cards highlighting based on current registration + selected quantity
 */
function updateMiniCards(totalQuantity) {
    const priceData = JSON.parse(document.getElementById('PriceData').value);
    
    // Find the highest price level achieved
    const currentActivePriceLevel = priceData.filter(p => totalQuantity >= p.Number)
                                            .sort((a, b) => b.Number - a.Number)[0];
    
    // Remove active class from all mini-cards
    document.querySelectorAll('.mini-card').forEach(card => {
        card.classList.remove('active');
    });
    
    // Add active class to the appropriate card
    if (currentActivePriceLevel) {
        // Find the card with matching number
        document.querySelectorAll('.mini-card').forEach(card => {
            const cardNumber = parseInt(card.querySelector('.card-header').textContent);
            if (cardNumber === currentActivePriceLevel.Number) {
                card.classList.add('active');
            }
        });
    } else {
        // No price level achieved, activate the default card (0)
        document.querySelectorAll('.mini-card').forEach(card => {
            const cardNumber = parseInt(card.querySelector('.card-header').textContent);
            if (cardNumber === 0) {
                card.classList.add('active');
            }
        });
    }
}
