function setCookie(name, value) {
    document.cookie = name + "=" + value;
}

function getCookie(name) {
    var matches = document.cookie.match(new RegExp(
      "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

$(function () {
	
	var r = Raphael("map", 500, 340),
		attributes = {
		    fill: "#C5BFC1",
            stroke: "#3899E6",
            'stroke-width': 1,
            'stroke-linejoin': "round"
		},
		arr = new Array();

	var freeBox = getCookie("freeBox").split(" ");

	//if (table in freeBox) alert("Элемент с ключём 3 присутствует в массиве");

    //debugger;
	 for (var table in paths) {
        if (paths.hasOwnProperty(table)) {
            if ($.inArray(table, freeBox) > -1) {
                var obj = r.path(paths[table].path);
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
                        $("#map").next(".point").remove();
                        $("#map").after($("<div />").addClass("point"));
                        setCookie("table", (paths[arr[this.id]].name).toString());
                        document.getElementById("ss").innerHTML = paths[arr[this.id]].name;

                        $(".point")
                            .html(paths[arr[this.id]].name + " " + paths[arr[this.id]].seats)
                            .prepend($("<a />").attr("href", "#").addClass("close").text("Закрыть"))
                            .css({
                                left: point.x + (point.width / 2) - 80,
                                top: point.y + (point.height / 2) - 20
                            })
                            .fadeIn();
                    });

                $(".point").find(".close").live("click", function() {
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
});

