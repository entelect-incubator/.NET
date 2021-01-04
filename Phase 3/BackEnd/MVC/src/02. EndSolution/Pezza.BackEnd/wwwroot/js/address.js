(function () {
    var zaData = {};
    var city = [];
    var province = [];
    var base_url = window.location.origin;
    $.getJSON(base_url + '/data/za.json', function (data) {
        zaData = data;
        $.each(zaData, function (i, v) {
            city.push(v.city);
            if ($.inArray(v.admin_name, province) === -1) province.push(v.admin_name);
        });

        city = city.sort();
        province = province.sort();

        for (var i = 0; i < city.length; i++) {
            $("#City").append("<option>" + city[i] + "</option>");
        }

        for (var j = 0; j < province.length; j++) {
            $("#Province").append("<option>" + province[j] + "</option>");
        }
    });
})();