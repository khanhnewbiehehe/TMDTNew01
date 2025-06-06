// Sorting products based on selected option
$(document).ready(function () {
    // Handle sort option change
    $('#sort-option').on('change', function () {
        const sortOption = $(this).val();
        sortProducts(sortOption);
    });

    // Initial sort on page load (if needed)
    const initialSortOption = $('#sort-option').val();
    if (initialSortOption) {
        sortProducts(initialSortOption);
    }
});

function sortProducts(sortOption) {
    const productContainer = $('#list_search_product');
    const productRows = productContainer.find('.row');

    if (productRows.length === 0) return;    // Collect all product cards
    const productCards = [];
    productRows.each(function () {
        $(this).find('.col-md-4').each(function () {
            productCards.push($(this));
        });
    });
    // Sort the product cards based on price
    productCards.sort(function (a, b) {
        // Extract price values by taking the first price in the range (minimum price)
        const priceTextA = a.find('.product-price').text();
        const priceTextB = b.find('.product-price').text();

        // Extract minimum price from price range "500.000₫ - 1.000.000₫"
        const minPriceA = priceTextA.split('-')[0].trim();
        const minPriceB = priceTextB.split('-')[0].trim();

        // Remove all non-digit characters and convert to number
        const priceA = parseInt(minPriceA.replace(/\D/g, ''));
        const priceB = parseInt(minPriceB.replace(/\D/g, ''));

        if (sortOption === 'low-to-high') {
            return priceA - priceB;
        } else {
            return priceB - priceA;
        }
    });

    // Clear existing rows
    productContainer.find('.row').remove();

    // Create new rows and append the sorted cards
    let currentRow;
    productCards.forEach(function (card, index) {
        // Create a new row for every 3 products
        if (index % 3 === 0) {
            currentRow = $('<div class="row mb-4"></div>');
            productContainer.append(currentRow);
        }

        currentRow.append(card);
    });

    // Move pagination to the bottom if exists
    const pagination = productContainer.find('nav[aria-label="Page navigation"]');
    if (pagination.length > 0) {
        productContainer.append(pagination);
    }
}