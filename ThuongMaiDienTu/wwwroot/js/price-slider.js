$(document).ready(function () {
    // Setup price range slider
    const slider = document.getElementById('price-range-slider');
    if (!slider) return; // Exit if element doesn't exist

    const minPriceInput = document.getElementById('minPriceInput');
    const maxPriceInput = document.getElementById('maxPriceInput');

    // Format number to VND currency
    function formatVND(value) {
        return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "₫";
    }

    // Get initial values from inputs
    const initialMinPrice = parseFloat(minPriceInput.value) || 0;
    const initialMaxPrice = parseFloat(maxPriceInput.value) || 20000000;
    const isMaxPriceDefault = maxPriceInput.value === "" || maxPriceInput.value === "20000000";

    // Initialize noUiSlider
    noUiSlider.create(slider, {
        start: [
            initialMinPrice,
            isMaxPriceDefault ? 20000000 : initialMaxPrice
        ],
        connect: true,
        step: 100000,
        range: {
            'min': 0,
            'max': 20000000
        }
    });    // Update price display and hidden input values when slider changes
    slider.noUiSlider.on('update', function (values, handle) {
        const min = Math.round(values[0]);
        const max = Math.round(values[1]);

        document.getElementById('price-min').innerHTML = formatVND(min);
        document.getElementById('price-max').innerHTML = formatVND(max);

        minPriceInput.value = min;
        maxPriceInput.value = max === 20000000 ? "" : max;

        // Check if the current values match any of our predefined ranges
        if (min === 0 && max === 20000000) {
            $('#priceOption-all').prop('checked', true);
        } else if (min === 0 && max === 500000) {
            $('#priceOption-under500k').prop('checked', true);
        } else if (min === 500000 && max === 1000000) {
            $('#priceOption-500kto1m').prop('checked', true);
        } else if (min === 1000000 && max === 5000000) {
            $('#priceOption-1mto5m').prop('checked', true);
        } else if (min === 5000000 && max === 20000000) {
            $('#priceOption-above5m').prop('checked', true);
        } else {
            // If no predefined range matches, select custom
            $('#priceOption-custom').prop('checked', true);
        }
    });// Handle radio button changes
    $('.price-option').change(function () {
        const value = $(this).val();
        if (value === 'all') {
            slider.noUiSlider.set([0, 20000000]);
            minPriceInput.value = 0;
            maxPriceInput.value = "";
            // Ensure that the "Tất cả" option is visually selected
            $('#priceOption-all').prop('checked', true);
        } else if (value === 'under500k') {
            slider.noUiSlider.set([0, 500000]);
            minPriceInput.value = 0;
            maxPriceInput.value = 500000;
            $('#priceOption-under500k').prop('checked', true);
        } else if (value === '500kto1m') {
            slider.noUiSlider.set([500000, 1000000]);
            minPriceInput.value = 500000;
            maxPriceInput.value = 1000000;
            $('#priceOption-500kto1m').prop('checked', true);
        } else if (value === '1mto5m') {
            slider.noUiSlider.set([1000000, 5000000]);
            minPriceInput.value = 1000000;
            maxPriceInput.value = 5000000;
            $('#priceOption-1mto5m').prop('checked', true);
        } else if (value === 'above5m') {
            slider.noUiSlider.set([5000000, 20000000]);
            minPriceInput.value = 5000000;
            maxPriceInput.value = "";
            $('#priceOption-above5m').prop('checked', true);
        } else if (value === 'custom') {
            // Just ensure that the "Tùy chỉnh" option is visually selected
            $('#priceOption-custom').prop('checked', true);
        }
    });    // Set initial state based on inputs
    if (initialMinPrice > 0 || !isMaxPriceDefault) {
        // Check if the values match any of our predefined ranges
        if (initialMinPrice === 0 && initialMaxPrice === 500000) {
            $('#priceOption-under500k').prop('checked', true);
        } else if (initialMinPrice === 500000 && initialMaxPrice === 1000000) {
            $('#priceOption-500kto1m').prop('checked', true);
        } else if (initialMinPrice === 1000000 && initialMaxPrice === 5000000) {
            $('#priceOption-1mto5m').prop('checked', true);
        } else if (initialMinPrice === 5000000 && (isMaxPriceDefault || initialMaxPrice === 20000000)) {
            $('#priceOption-above5m').prop('checked', true);
        } else {
            $('#priceOption-custom').prop('checked', true);
        }
    } else {
        $('#priceOption-all').prop('checked', true);
    }
});
