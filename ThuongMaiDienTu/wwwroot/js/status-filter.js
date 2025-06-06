$(document).ready(function () {
    // Sync the "Tất cả" checkbox with the individual status checkboxes
    function updateSelectAllCheckbox() {
        const allChecked = $('.status-filter:checked').length === $('.status-filter').length;
        const noneChecked = $('.status-filter:checked').length === 0;

        if (allChecked) {
            $('#status-all').prop('checked', true);
            $('#status-all').prop('indeterminate', false);
        } else if (noneChecked) {
            // If none are checked, check at least one (Ongoing) to avoid empty results
            $('#status-ongoing').prop('checked', true);
            $('#status-all').prop('checked', false);
            $('#status-all').prop('indeterminate', true);
        } else {
            $('#status-all').prop('checked', false);
            $('#status-all').prop('indeterminate', true);
        }
    }

    // Initial state
    updateSelectAllCheckbox();    // Handle select-all checkbox click
    $('#status-all').on('click', function () {
        const isChecked = $(this).prop('checked');
        $('.status-filter').prop('checked', isChecked);

        // When "Tất cả" is selected, make sure all checkboxes are submitted as true
        if (isChecked) {
            // Make sure checkboxes are checked visually
            $('#status-ongoing').prop('checked', true);
            $('#status-full').prop('checked', true);
            $('#status-ended').prop('checked', true);

            // Ensure values will be submitted correctly
            if (!$('#status-ongoing-hidden').length) {
                $('<input>').attr({
                    type: 'hidden',
                    id: 'status-ongoing-hidden',
                    name: 'LaunchGoing',
                    value: 'true'
                }).appendTo('#searchForm');
            }

            if (!$('#status-full-hidden').length) {
                $('<input>').attr({
                    type: 'hidden',
                    id: 'status-full-hidden',
                    name: 'LaunchFull',
                    value: 'true'
                }).appendTo('#searchForm');
            }

            if (!$('#status-ended-hidden').length) {
                $('<input>').attr({
                    type: 'hidden',
                    id: 'status-ended-hidden',
                    name: 'LaunchEnded',
                    value: 'true'
                }).appendTo('#searchForm');
            }
        } else {
            // Remove hidden inputs if "Tất cả" is unchecked
            $('#status-ongoing-hidden, #status-full-hidden, #status-ended-hidden').remove();
        }
    });    // Handle individual status checkboxes
    $('.status-filter').on('click', function () {
        // Remove the corresponding hidden input if it exists
        const inputName = $(this).attr('name');
        $(`#${inputName}-hidden`).remove();

        updateSelectAllCheckbox();

        // Ensure at least one checkbox is checked
        if ($('.status-filter:checked').length === 0) {
            $(this).prop('checked', true);
            alert('Vui lòng chọn ít nhất một trạng thái.');
        }
    });

    // Handle form submission - ensure unchecked checkboxes are submitted as false
    $('#searchForm').on('submit', function () {
        $('.status-filter').each(function () {
            const inputName = $(this).attr('name');
            if (!$(this).is(':checked')) {
                $('<input>').attr({
                    type: 'hidden',
                    name: inputName,
                    value: 'false'
                }).appendTo('#searchForm');
            }
        });
    });
});
