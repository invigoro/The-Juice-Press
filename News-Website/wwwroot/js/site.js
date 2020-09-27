// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    $("form#main-search-bar").on("submit", function (e) {
        e.preventDefault();
        var searchString = $(this).find("input#searchString").val();
        $(this).attr("action", "/home/search/" + searchString);
        $(this).unbind();
        $(this).submit();

    });


    $(".text-only").each(function () {
        $(this).text($(this).text());
    })
})