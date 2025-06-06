
document.addEventListener("DOMContentLoaded", function () {

    // tải lại trang, lưu trạng thái button theme
    let savedTheme = localStorage.getItem("theme");
    let html = document.documentElement;
    let themeToggle = document.getElementById("theme"); // Lấy phần tử nút toggle
    if (savedTheme) {
        document.documentElement.setAttribute("data-bs-theme", savedTheme);
        themeToggle.checked = savedTheme === "dark"; // Cập nhật trạng thái checked
    }

    // kích hoạt trạng thái nav khi ở 1 trang tương tự
    let currentPath = window.location.pathname; // Lấy đường dẫn trang hiện tại
    let navLinks = document.querySelectorAll(".navbar-nav .nav-link");
    navLinks.forEach(link => {
        if (link.getAttribute("href") === currentPath) {
            link.classList.add("active");
        }
    });

    if (localStorage.getItem('leftNavHidden') === 'true') {
        ishidden(true, true)
    } else {
        ishidden(false, true)
    }
    
});

// thu phóng thanh nav khi nhấn nút
document.getElementById("closeMenu").addEventListener("click", function () {
    let icon = document.getElementById("icon");
    let leftPane = document.getElementById('leftNav');
    if (icon.classList.contains("on")) {
        ishidden(true)
    } else {
        ishidden(false)
    }
});
function ishidden(a,b) {
    let icon = document.getElementById("icon");
    let leftPane = document.getElementById('leftNav');
    leftPane.classList.add('trantision-x');
    if (a) {
        leftPane.classList.remove('hidden');
        icon.classList.remove("on");
        localStorage.setItem('leftNavHidden', 'true');
    } else {
        leftPane.classList.add('hidden');
        icon.classList.add("on");
        localStorage.setItem('leftNavHidden', 'false');
    }
    if (b) {
        leftPane.classList.remove('trantision-x');
    }
}

window.addEventListener('resize', function () {
    const element = document.getElementById('leftNav');
    let icon = document.getElementById("icon");
    if (window.innerWidth < 576) {// Thêm class 'hidden' khi <576px
        ishidden(false)
    }else
    if (element.classList.contains('hidden')) {
        ishidden(false)
        
    } else {
        ishidden(true)
    }
});


// đổi trạng thái theme khi nhấn nút
function toggleTheme() {
    let html = document.documentElement;
    let themeToggle = document.getElementById("theme");

    if (html.getAttribute("data-bs-theme") === "light") {
        html.setAttribute("data-bs-theme", "dark");
        localStorage.setItem("theme", "dark");
        themeToggle.checked = true;
    } else {
        html.setAttribute("data-bs-theme", "light");
        localStorage.setItem("theme", "light");
        themeToggle.checked = false;
    }
}

document.getElementById("cover-sub-info").addEventListener("click", function () {
    document.getElementById("cover-sub-info").style.display = "none";
    document.getElementById("sub-info").style.display = "none";
});

document.getElementById("nameUser").addEventListener("click", function () {
    document.getElementById("cover-sub-info").style.display = "block";
    document.getElementById("sub-info").style.display = "block";
});