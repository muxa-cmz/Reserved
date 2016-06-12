function setCookie(name, value) {
    document.cookie = name + "=" + value;
}

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

function Close() {
    debugger;
    var t = $(this),
        parent = t.parent(".point");

    parent.fadeOut(function () {
        parent.remove();
    });
    return false;
}

//$(function () {
function PaintTable() {

    var freeBox = getCookie("freeBox").split(" ");
    var hall = getCookie("hall");

    var r = Raphael("map" + hall, 500, 340),
        attributes = {
            fill: "#C5BFC1",
            stroke: "#3899E6",
            'stroke-width': 1,
            'stroke-linejoin': "round"
        },
        arr = new Array();

    //if (table in freeBox) alert("Элемент с ключём 3 присутствует в массиве");

    var path = paths[hall];

    for (var table in path) {
        if (path.hasOwnProperty(table)) {
            if ($.inArray(table, freeBox) > -1) {
                var obj = r.path(path[table].path);
                obj.attr(attributes);
                arr[obj.id] = table;
                obj
                    .hover(function() {
                        this.animate({
                            fill: "#E6F21B"
                        }, 300);
                    }, function() {
                        this.animate({
                            fill: attributes.fill
                        }, 300);
                    })
                    .click(function() {
                        document.location.hash = arr[this.id];

                        var point = this.getBBox(0);
                        $("#map" + hall).next(".point").remove();
                        $("#map" + hall).after($("<div />").addClass("point"));
                        setCookie("table", (path[arr[this.id]].name).toString());
                        document.getElementById("ss").innerHTML = path[arr[this.id]].name;

                        $(".point")
                            .html("Стол " + path[arr[this.id]].name + " " + path[arr[this.id]].seats + "\n" + path[arr[this.id]].description)
                            .prepend($("<a />").attr("href", "#").addClass("close").text("Закрыть"))
                            .css({
                                left: point.x + (point.width / 2) - 80,
                                top: point.y + (point.height / 2) - 20
                            })
                            .fadeIn();
                    });

                $(document).on("click", ".close", function() {
                    var t = $(this),
                        parent = t.parent(".point");
                    parent.fadeOut(function() {
                        parent.remove();
                    });
                    return false;
                });
            }
        }
    }
    //});
}

