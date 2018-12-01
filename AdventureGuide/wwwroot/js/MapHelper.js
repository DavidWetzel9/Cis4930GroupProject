var map = new L.Map('map', { center: new L.LatLng(0, 0), zoom: 12 });
var markersLayer = new L.LayerGroup();
function ShowMap() {
    L.tileLayer('http://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
    }).addTo(map);
    markersLayer.addTo(map);
    ChangeMarkerPosition();
}
function ChangeMarkerPosition() {

    var latArray = new Array();
    var lonArray = new Array();
    var nameArray = new Array();
    var ratingArray = new Array();
    var idArray = new Array();

    $("input.hidden-lat").each(function () {
            latArray.push($(this).val());
    });
    $("input.hidden-lon").each(function () {
        lonArray.push($(this).val());
    });
    $("input.hidden-name").each(function () {
        nameArray.push($(this).val());
    });
    $("input.hidden-rating").each(function () {
        ratingArray.push($(this).val());
    });
    $("input.hidden-id").each(function () {
        idArray.push($(this).val());
    });

    var markerArray = new Array();
    for (var i = 0; i < latArray.length; i++) {
        var mark = L.marker([latArray[i], lonArray[i]]);
        if (nameArray.length === 0) {
            mark.bindPopup();
        }
        else {
            var id = "input-" + idArray[i];
            var onclick = "InitRating(this)";
            var startDiv = "<div class='my-star' data-string='" + id + "' onclick='" + onclick + "'>";
            var nameDiv = "<h2>" + nameArray[i] + "</h2>";
            var inputDiv = "<input id='" + id + "' name='"+ id + "' class='rating rating-loading'" + " value='" + ratingArray[i] + "' data-min='0' data-max='5' data-step='.5' data-readonly='true' />";
            var endDiv = "</div>";
            var markInfo = startDiv + nameDiv + inputDiv + endDiv;
            console.log(markInfo);
            mark.bindPopup(markInfo);
        }

        markerArray.push(mark);
    }
    var markers = L.layerGroup(markerArray);
    var group = new L.featureGroup(markerArray);
    markersLayer.clearLayers();
    markersLayer.addLayer(markers);
    setTimeout(function () {
        map.invalidateSize();
        map.fitBounds(group.getBounds());
    }, 100);    
}


