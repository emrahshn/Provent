//Wizard
$(document).ready(function () {

    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {

        var $target = $(e.target);
        if ($target.parent().hasClass('disabled')) {
            return false;
        }
        var x = document.getElementById("tKayit");
        var y = document.getElementById("tGrid");
        var z = document.getElementById("btnOperasyon");
        var t = document.getElementById("btnKonfirme");
        var grid = document.getElementById("teklif-grid");
        if ($target.parent().attr('id') == "liYeniTeklif") {
            x.style.display = "block";
            y.style.display = "none";
            
        }
        else {
            if ($target.parent().attr('id') == "liOperasyon") {
                z.style.display = "inline-block";
                t.style.display = "none";
                grid.data("kendoGrid").setDataSource(ds2);
            }
            if ($target.parent().attr('id') == "liKonfirme") {
                z.style.display = "none";
                t.style.display = "inline-block";
                grid.data("kendoGrid").setDataSource(ds3);
            }
            x.style.display = "none";
            y.style.display = "block";
        }
    });

        $(".next-step").click(function (e) {

            var $active = $('.wizard .nav-tabs li.active');
            $active.next().removeClass('disabled');
            nextTab($active);
        });
        $(".prev-step").click(function (e) {

            var $active = $('.wizard .nav-tabs li.active');
            prevTab($active);

        });
    });

    function nextTab(elem) {
        $(elem).next().find('a[data-toggle="tab"]').click();

    }
    function prevTab(elem) {
        $(elem).prev().find('a[data-toggle="tab"]').click();
    }
