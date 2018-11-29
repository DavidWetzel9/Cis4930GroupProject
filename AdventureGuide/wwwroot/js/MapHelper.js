var map = new L.Map('map', { center: new L.LatLng(0, 0), zoom: 12 });
var markersLayer = new L.LayerGroup();
function InitMap() {
    L.tileLayer('http://{s}.google.com/vt/lyrs=m&x={x}&y={y}&z={z}', {
        subdomains: ['mt0', 'mt1', 'mt2', 'mt3']
    }).addTo(map);
    geocoder = new google.maps.Geocoder();
    markersLayer.addTo(map);
    ChangeMarkerPosition();
}
function ChangeMarkerPosition() {
    //var country = $("#Country").val();
    //var state = $("#State").val();
    //var city = $("#City").val();
    //var address = $("#Address").val();
    //var fullAddress = country + "," + state + "," + city + "," + address;
    var latArray = new Array();
    var lonArray = new Array();
    $("input.hidden-lat").each(function () {
            latArray.push($(this).val());
            
    });
    $("input.hidden-lon").each(function () {
        lonArray.push($(this).val());
    });
    
    var markerArray = new Array();
    var lat = 0;
    var lon = 0;
    for (var i = 0; i < latArray.length; i++) {
        //console.log(latArray[i]);
        //console.log(lonArray[i]);
        lat += parseFloat(latArray[i]);
        lon += parseFloat(lonArray[i]);
        markerArray.push(L.marker([latArray[i], lonArray[i]]));
    }
    lat = lat / latArray.length;
    lon = lon / lonArray.length;
    var markers = L.layerGroup(markerArray);

    //var lat = $("#Lat").val();
    //var lon = $("#Lon").val();
    markersLayer.clearLayers();
    //marker = L.marker([lat, lon]);
    markersLayer.addLayer(markers);
    
    setTimeout(function () {
        map.invalidateSize();
        map.panTo(new L.LatLng(lat, lon));
    }, 100);
    //geocoder.geocode({ 'address': fullAddress }, function (results, status) {
    //    if (status === google.maps.GeocoderStatus.OK) {
    //        markersLayer.clearLayers();
    //        var lat = results[0].geometry.location.lat();
    //        var lon = results[0].geometry.location.lng();
    //        console.log(lat);
    //        console.log(lon);
    //        var marker = L.marker([lat, lon]);
    //        markersLayer.addLayer(marker);
    //        setTimeout(function () {
    //            map.invalidateSize();
    //            map.panTo(new L.LatLng(lat, lon));
    //        }, 100);
    //    } else {
    //        alert('Geocode was not successful for the following reason: ' + status);
    //    }
    //});
}
