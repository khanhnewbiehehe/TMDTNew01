document.addEventListener('DOMContentLoaded', function () {
    const dropdownToggles = document.querySelectorAll('.my_dropdowntoggle');
  
    dropdownToggles.forEach(toggle => {
      const parent = toggle.closest('.my_dropdown');
      const dropdownMenu = toggle.nextElementSibling;
  
      toggle.addEventListener('click', function (e) {
        const isOnly = parent.classList.contains('dropdown-only');
        const isLargeScreen = window.innerWidth >= 990.98;
  
        // Nếu là dropdown-only và chưa đủ lớn thì không mở
        if (isOnly && !isLargeScreen) return;
  
        e.stopPropagation();
  
        const isOpen = dropdownMenu.style.display === 'block';
  
        // Đóng tất cả dropdown khác
        document.querySelectorAll('.my_dropdownlist').forEach(menu => {
          menu.style.display = 'none';
        });
  
        // Toggle menu hiện tại
        if (!isOpen) {
          dropdownMenu.style.display = 'block';
        }
      });
    });
  
    // Đóng dropdown khi click ra ngoài
    document.addEventListener('click', function () {
      document.querySelectorAll('.my_dropdownlist').forEach(menu => {
        menu.style.display = 'none';
      });
    });
  });
$(document).ready(function () {
    $(".carousel-1").owlCarousel({
        loop: true,
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
    $(".carousel-2").owlCarousel({
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
});