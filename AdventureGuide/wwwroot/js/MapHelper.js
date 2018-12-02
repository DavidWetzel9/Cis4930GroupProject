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
    var imgArray = new Array();

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
    $("input.hidden-img").each(function () {
        imgArray.push($(this).val());
    });
    //var desArray = new Array();
    //$("input.hidden-desc").each(function () {
    //    desArray.push($(this).val());
    //});
    var markerArray = new Array();
    for (var i = 0; i < latArray.length; i++) {
        var mark = L.marker([latArray[i], lonArray[i]]);
        if (nameArray.length > 0) {
            var id = "input-" + idArray[i];
            var parentDiv = "<div class='d-flex pin-flex'>";
            var imgDiv = "<div class='img-pin mr-1'> <img class='img-pin' src='" + imgArray[i] + "' alt='Destination icon' /> </div>";
            var startDiv = "<div class='my-star ml-1'>";
            var nameDiv = "<h2>" + nameArray[i] + "</h2>";
            var inputDiv = "<div class='d-flex pin-flex'><input id='" + id + "' name='" + id + "' class='rating rating-loading'" + " value='" + ratingArray[i] + "' data-min='0' data-max='5' data-step='.5' data-readonly='true' data-size='sm'/>";
            var endDiv = "</div>";
            var button = "<button class='btn btn-sm btn-success' onclick='DetailsUrl(this)' data-string='" + idArray[i] + "'>Details</button> </div>";
            //var desDiv = "<div>" + desArray[i] + "</div>";
            //var markInfo = parentDiv + imgDiv + startDiv + nameDiv + inputDiv + desDiv + endDiv + endDiv;
            var markInfo = parentDiv + imgDiv + startDiv + nameDiv + inputDiv + button + endDiv + endDiv;
            mark.bindPopup(markInfo);
            mark.on('click', function () {
                $("input.rating").rating();
            });
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


