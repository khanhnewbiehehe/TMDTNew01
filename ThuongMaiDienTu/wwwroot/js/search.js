$(document).ready(function () {
    console.log("Search.js loaded");

    // Ensure search form is properly submitted when search button is clicked
    $('.search-btn, button[form="searchForm"]').on('click', function (e) {
        e.preventDefault();
        console.log("Search button clicked");
        $('#searchForm').submit();
    });

    // Make sure the input values are synchronized
    $('#searchInput, #searchInputMobile').on('input', function () {
        const value = $(this).val();
        $('input[name="searchTerm"]').val(value);
        console.log("Search input updated:", value);
    });

    // Add a special class to highlight that badges are clickable
    $('.badge[role="button"]').addClass('interactive-badge');

    // Direct handler for the X icon
    $(document).on('click', '.badge .bi-x', function (e) {
        e.stopPropagation(); // Prevent event bubbling
        console.log("Badge X icon clicked directly");

        // The actual handler is in LoadSearchProduct.js
        // This just prevents event propagation
    });

    // Additional handler for badge clicks - captures clicks anywhere on the badge
    $(document).on('click', '.badge[role="button"]', function (e) {
        if (!$(e.target).hasClass('bi-x')) {  // Only trigger if not clicking directly on the X
            console.log("Badge area clicked, forwarding to X icon");
            const xIcon = $(this).find('.bi-x');
            if (xIcon.length) {
                xIcon.trigger('click');
            } else {
                console.log("Could not find X icon in badge");
            }
        }
    });

    // Make badges visually respond to hover
    $('.badge[role="button"]').hover(
        function () { $(this).css('transform', 'scale(1.05)'); },
        function () { $(this).css('transform', 'scale(1)'); }
    );

    // Ensure form is properly submitting
    $('#searchForm').on('submit', function () {
        console.log('Form submitted with searchTerm:', $('input[name="searchTerm"]').val());
        return true;
    });
});