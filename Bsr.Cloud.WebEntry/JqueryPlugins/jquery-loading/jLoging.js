(function ($) {
    $.loading = function (params) {
        _defaults = {
            minWidth: "200px",
            minHeight: "90px",
            top: "100px",
            tip: "",
            img: "Images/loading.gif",
            autoHide: false
        };
        params = $.extend(_defaults, params);
        $("body").addClass("masked-relative masked");
        var loadingMask = $("<div>").addClass("loadmask");
        loadingMask.appendTo($("body"));

        var loadDiv = $("<div>").addClass("loadmask-div");
        loadDiv.css({ "min-height": _defaults.minHeight, "width": _defaults.minWidth, "top": _defaults.top });

        var loadImg = $("<img>").attr("src", _defaults.img).addClass("loadmask-img");

        var loadTip = $("<div>").addClass("loadmask-tip").text(_defaults.tip);

        var left = ($(window).width()) / 2 - (loadDiv.width() / 2);
        loadDiv.css("left", left+"px");
        loadDiv.append(loadImg).append(loadTip).appendTo($("body"));
    }
})(jQuery)