
function initMap(){
    var connestogoMap = L.map('map').setView([43.572270, -80.580270], 11);
    // Add a tile layer to add to our map, in this case using OpenStreetMap's tiles:
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors'
    }).addTo(connestogoMap);


    // Markers for access points or monitoring stations:
    var stJacobsDam = L.marker([43.5353237, -80.5734525]).bindPopup('St. Jacobs Dam').openPopup().addTo(connestogoMap);
    var stJacobsBridge = L.marker([43.5403865, -80.5525462]).bindPopup('St. Jacobs Bridge').openPopup().addTo(connestogoMap);
    var mactonBridge = L.marker([43.6462778, -80.6778242]).bindPopup('Macton Bridge').openPopup().addTo(connestogoMap);
    var glenAllan = L.marker([43.655276, -80.7028285]).bindPopup('Glen Allan Park').openPopup().addTo(connestogoMap);
    var conestogoDam = L.marker([43.6744907, -80.7161118]).bindPopup('Conestogo Dam').openPopup().addTo(connestogoMap);
}

document.addEventListener('DOMContentLoaded', initMap);
