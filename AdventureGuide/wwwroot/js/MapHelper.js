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
    $("input.hidden-lat").each(function () {
            latArray.push($(this).val());
            
    });
    $("input.hidden-lon").each(function () {
        lonArray.push($(this).val());
    });
    var markerArray = new Array();
    for (var i = 0; i < latArray.length; i++) {
        markerArray.push(L.marker([latArray[i], lonArray[i]]));
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


