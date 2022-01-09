// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () { //when document is loaded
    $(document).on('click', '.delete', function (event) {
        if (!confirm("Delete an item?")) {
            event.preventDefault();
        }
    });
});

