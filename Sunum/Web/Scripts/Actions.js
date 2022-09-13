$(".mb-control").on("click", function () {
    var box = $($(this).data("box"));
    if (box.length > 0) {
        box.toggleClass("open");

        var sound = box.data("sound");

        if (sound === 'alert')
            playAudio('alert');

        if (sound === 'fail')
            playAudio('fail');

    }
    return false;
});
$(".mb-control-close").on("click", function () {
    $(this).parents(".message-box").removeClass("open");
    return false;
});