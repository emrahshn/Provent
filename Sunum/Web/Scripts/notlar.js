$(document).ready(function () {
    $('div.k-grid').on('mouseenter', '#notlink', function () {
        var element = $(this);
        var parent = element.closest("tr").find("div.not")[0];
        $(parent).addClass('active');
    });
    $('div.k-grid').on('mouseleave', '#notlink', function () {
        var element = $(this);
        var parent = element.closest("tr").find("div.not")[0];
        $(parent).removeClass('active');
    });
    $('div.k-grid').on('mouseenter', 'div.not', function () {
        var element = $(this);
        var parent = element.closest("tr").find("div.not")[0];
        $(parent).addClass('active');
    });
    $('div.k-grid').on('mouseleave', 'div.not', function () {
        var element = $(this);
        var parent = element.closest("tr").find("div.not")[0];
        $(parent).removeClass('active');
    });
});