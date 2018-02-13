$(function () {
    var input = $("#searchInput");
    var templ = $("#searchItemTemplate");
    var container = $("#searchItemsContainer");

    function getDateString(str) {
        var num = str.match(/\d+/g);
        var date = new Date(parseFloat(num));
        return formatDate(date);
    }

    function formatDate(date) {

        var dd = date.getDate();
        if (dd < 10) dd = '0' + dd;

        var mm = date.getMonth() + 1;
        if (mm < 10) mm = '0' + mm;

        var yy = date.getFullYear();
        if (yy < 10) yy = '0' + yy;

        var h = date.getHours();

        var m = date.getMinutes();
        if (m < 10) m = '0' + m;

        var s = date.getSeconds();
        if (s < 10) s = '0' + s;

        return dd + '.' + mm + '.' + yy + ' ' + h + ':' + m + ':' + s
    }

    $(input).on("keyup", function () {
        if (($(this).val() || "").trim()) {
            $.ajax({
                method: "POST",
                url: "/Article/Search",
                data: { query: $(this).val() },
                dataType: "json"
            }).done(function (data) {
                //console.log(data);

                $(container).html('');
                $(container).show();

                if (data && data.length) {

                    for (var i = 0; i < data.length; i++) {
                        var d = data[i];
                        var item = $(templ).html();
                        for (var key in d) {
                            if (key === "Date")
                                d[key] = getDateString(d[key]);
                            item = item.replace('{' + key + '}', d[key]);
                        }

                        $(container).append(item);
                    }
                }
                else {
                    $(container).append($("<div>").append($("<p>").html("No results")));
                }
            });
        }
        else
            $(container).hide();
    });
})