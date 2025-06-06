// Invoice Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    initializeInvoicePage();
});

function initializeInvoicePage() {
    setupFormSubmission();
    setupAnimations();
    setupInteractiveElements();
    validateFormData();
}

// Form submission with loading state
function setupFormSubmission() {
    const form = document.getElementById('invoiceForm');
    const submitBtn = form.querySelector('button[type="submit"]');
    const spinner = document.getElementById('paymentSpinner');
    
    form.addEventListener('submit', function(e) {
        // Validate before submit
        if (!validateInvoiceData()) {
            e.preventDefault();
            return;
        }
        
        // Show loading state
        showLoadingState(submitBtn, spinner);
        
        // Add loading overlay
        createLoadingOverlay();
        
        // Auto-hide loading after timeout (safety measure)
        setTimeout(() => {
            hideLoadingState(submitBtn, spinner);
        }, 30000); // 30 seconds timeout
    });
}

// Show loading state
function showLoadingState(submitBtn, spinner) {
    submitBtn.disabled = true;
    spinner.classList.remove('d-none');
    
    const buttonText = submitBtn.querySelector('span');
    buttonText.setAttribute('data-original-text', buttonText.textContent);
    buttonText.textContent = 'Đang xử lý thanh toán...';
    
    submitBtn.classList.add('loading');
}

// Hide loading state
function hideLoadingState(submitBtn, spinner) {
    submitBtn.disabled = false;
    spinner.classList.add('d-none');
    
    const buttonText = submitBtn.querySelector('span');
    const originalText = buttonText.getAttribute('data-original-text');
    if (originalText) {
        buttonText.textContent = originalText;
    }
    
    submitBtn.classList.remove('loading');
    
    // Remove loading overlay
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) {
        overlay.remove();
    }
}

// Create loading overlay
function createLoadingOverlay() {
    const loadingOverlay = document.createElement('div');
    loadingOverlay.className = 'loading-overlay';
    loadingOverlay.innerHTML = `
        <div class="text-center">
            <div class="loading-spinner mb-4"></div>
            <h4 class="text-primary mb-3">
                <i class="fas fa-credit-card me-2"></i>
                Đang chuyển hướng đến trang thanh toán
            </h4>
            <p class="text-muted mb-3">Vui lòng đợi trong giây lát, không đóng trình duyệt</p>
            <div class="progress" style="height: 4px; width: 200px; margin: 0 auto;">
                <div class="progress-bar progress-bar-striped progress-bar-animated bg-primary" 
                     style="width: 100%"></div>
            </div>
        </div>
    `;
    document.body.appendChild(loadingOverlay);
}

// Setup animations
function setupAnimations() {
    // Animate info sections
    const infoSections = document.querySelectorAll('.info-section');
    infoSections.forEach((section, index) => {
        section.style.animationDelay = `${index * 0.3}s`;
        section.classList.add('animate__animated', 'animate__fadeInUp');
    });
    
    // Animate product table rows
    const tableRows = document.querySelectorAll('tbody tr');
    tableRows.forEach((row, index) => {
        row.style.animationDelay = `${(index + 2) * 0.2}s`;
        row.classList.add('animate__animated', 'animate__fadeInLeft');
    });
    
    // Animate payment summary
    const paymentSummary = document.querySelector('.payment-summary');
    if (paymentSummary) {
        paymentSummary.style.animationDelay = '1s';
        paymentSummary.classList.add('animate__animated', 'animate__zoomIn');
    }
}

// Setup interactive elements
function setupInteractiveElements() {
    // Product image hover effects
    const productImages = document.querySelectorAll('.product-image');
    productImages.forEach(img => {
        img.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.1) rotate(2deg)';
            this.style.boxShadow = '0 8px 25px rgba(0,0,0,0.2)';
        });
        
        img.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1) rotate(0deg)';
            this.style.boxShadow = '0 3px 10px rgba(0,0,0,0.1)';
        });
    });
    
    // Submit button click animation
    const submitBtn = document.querySelector('button[type="submit"]');
    if (submitBtn) {
        submitBtn.addEventListener('mousedown', function() {
            this.style.transform = 'scale(0.98)';
        });
        
        submitBtn.addEventListener('mouseup', function() {
            this.style.transform = 'scale(1)';
        });
        
        submitBtn.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    }
    
    // Info section hover effects
    const infoCards = document.querySelectorAll('.info-section');
    infoCards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.boxShadow = '0 10px 30px rgba(0,0,0,0.15)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = 'none';
        });
    });
}

// Validate invoice data
function validateInvoiceData() {
    const form = document.getElementById('invoiceForm');
    const formData = new FormData(form);
    
    // Check if invoice items exist
    const hasItems = formData.getAll('InvoiceItems[0].ProductTypeId').length > 0;
    if (!hasItems) {
        showNotification('Không có sản phẩm nào trong đơn hàng!', 'error');
        return false;
    }
    
    // Check deposit amount
    const deposit = parseFloat(formData.get('Deposit'));
    if (!deposit || deposit <= 0) {
        showNotification('Số tiền cọc không hợp lệ!', 'error');
        return false;
    }
    
    return true;
}

// Validate form data on page load
function validateFormData() {
    const totalElements = document.querySelectorAll('[data-total-amount]');
    let totalAmount = 0;
    
    totalElements.forEach(element => {
        const amount = parseFloat(element.getAttribute('data-total-amount')) || 0;
        totalAmount += amount;
    });
    
    if (totalAmount <= 0) {
        showNotification('Tổng giá trị đơn hàng không hợp lệ!', 'warning');
    }
}

// Show notification
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `alert alert-${type} alert-dismissible fade show position-fixed`;
    notification.style.cssText = `
        top: 20px;
        right: 20px;
        z-index: 10000;
        max-width: 400px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15);
    `;
    
    notification.innerHTML = `
        <i class="fas fa-${getIconForType(type)} me-2"></i>
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(notification);
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        if (notification.parentNode) {
            notification.remove();
        }
    }, 5000);
}

// Get icon for notification type
function getIconForType(type) {
    const icons = {
        'success': 'check-circle',
        'error': 'exclamation-circle',
        'warning': 'exclamation-triangle',
        'info': 'info-circle'
    };
    return icons[type] || 'info-circle';
}

// Format currency (utility function)
function formatCurrency(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Copy invoice code to clipboard
function copyInvoiceCode() {
    const invoiceCode = document.querySelector('[data-invoice-code]');
    if (invoiceCode) {
        const text = invoiceCode.textContent;
        navigator.clipboard.writeText(text).then(() => {
            showNotification('Đã sao chép mã hóa đơn!', 'success');
        }).catch(() => {
            showNotification('Không thể sao chép mã hóa đơn!', 'error');
        });
    }
}

// Print invoice
function printInvoice() {
    window.print();
}

// Export functions for global access
window.InvoiceManager = {
    copyInvoiceCode,
    printInvoice,
    showNotification,
    formatCurrency
};
