
function updateDateTime(){
    var now = new Date();
    var month = now.toLocaleString('default', {month: 'long'});
    var day = now.getDate();
    var year = now.getFullYear();
    var hours = now.getHours();
    var minutes = now.getMinutes();
    var ampm = hours >= 12? 'PM': 'AM';

    hours = hours %12;
    hours = hours? hours: 12;
    minutes = minutes < 10? '0' + minutes: minutes;
    var currentTime = `${month} ${day}, ${year}, ${hours}:${minutes} ${ampm}`
    document.getElementById("datetime").textContent = `Displaying Current Conditions for: ${currentTime}`;
}

function initMap(){
    var connestogoMap = L.map('map').setView([43.572270, -80.580270], 11);
    // Add a tile layer to add to our map, in this case using OpenStreetMap's tiles:
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors'
    }).addTo(connestogoMap);

    var damIcon = new L.Icon({
        iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png',
        shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/0.7.7/images/marker-shadow.png',
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41]
    });
    // Markers for access points or monitoring stations:
    var stJacobsDam = L.marker([43.5353237, -80.5734525], {icon: damIcon}).bindPopup('St. Jacobs Dam').openPopup().addTo(connestogoMap);
    var stJacobsBridge = L.marker([43.5403865, -80.5525462]).bindPopup('St. Jacobs Bridge').openPopup().addTo(connestogoMap);
    var mactonBridge = L.marker([43.6462778, -80.6778242]).bindPopup('Macton Bridge').openPopup().addTo(connestogoMap);
    var glenAllan = L.marker([43.655276, -80.7028285]).bindPopup('Glen Allan Park').openPopup().addTo(connestogoMap);
    var conestogoDam = L.marker([43.6744907, -80.7161118], {icon: damIcon}).bindPopup('Conestogo Dam').openPopup().addTo(connestogoMap);

    //Legend to distinguish between markers
    var legend = L.control({ position: 'bottomright' });
    legend.onAdd = function (map) {
        var div = L.DomUtil.create('div', 'info legend');
        labels = ['<strong>Categories</strong>'],
        // add to categories for new future markings that are not damns, or simple river access 
        categories = [
            { name: "River Access with Parking", icon: "https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-blue.png" },
            { name: "Dam River Access", icon: "https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-green.png" }
        ]; 

        div.innerHTML += '<strong> River Access Types </strong><br>';
        for (var i = 0; i < categories.length; i++) {
            var category = categories[i];
            div.innerHTML += '<img src="' + category.icon + '" style="width: 18px; height: 18px; margin-right: 8px;"> ' + category.name + '<br>';
        }
        return div;
    };
    legend.addTo(connestogoMap);
        
    }

document.addEventListener('DOMContentLoaded', function(){
    initMap();
    updateDateTime();
});
