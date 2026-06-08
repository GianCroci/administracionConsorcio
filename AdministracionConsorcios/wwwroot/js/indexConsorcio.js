$(document).ready(function () {
    $('#consorciosTable').DataTable({
        pageLength: 5,
        lengthChange: false,
        language: {
            search: "Buscar:",
            paginate: {
                first: "Primero",
                last: "Último",
                next: "→",
                previous: "←"
            },
            zeroRecords: "No se encontraron resultados",
            info: "Mostrando _START_ a _END_ de _TOTAL_ consorcios"
        },
        dom: "<'flex justify-between items-center mb-4'<'custom-button'>f>rtip"
    });
    $(".custom-button").append(`
        <div class="flex gap-2">
            <a href="/Consorcio/Crear"
               class="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded transition-colors">
                Nuevo Consorcio
            </a>

            <button
                onclick="openMapModal()"
                class="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition">
                Ver mapa
            </button>
        </div>
    `);
});

let map = null;
let markers = null;


function openMapModal() {

    const modal = document.getElementById('mapModal');

    modal.classList.remove('hidden');
    modal.classList.add('flex');

    initMap();

    setTimeout(() => {
        if (map) {
            map.invalidateSize();
        }
    }, 250);
}


function closeMapModal() {

    const modal = document.getElementById('mapModal');

    modal.classList.add('hidden');
    modal.classList.remove('flex');
}


function initMap() {

    if (map) {
        setTimeout(() => {
            map.invalidateSize();
        }, 200);
        return;
    }

    map = L.map('map').setView([-34.6037, -58.3816], 12);

    
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; OpenStreetMap'
    }).addTo(map);

    
    markers = L.layerGroup().addTo(map);

    
    cargarConsorcios();
}


function cargarConsorcios() {

    fetch('/api/consorcio/listado-mapa')
        .then(r => r.json())
        .then(consorcios => {

            console.log(consorcios);

            const bounds = L.latLngBounds([]);

            consorcios.forEach(c => {

                const lat = parseFloat(c.latitud);
                const lng = parseFloat(c.longitud);

                if (isNaN(lat) || isNaN(lng)) return;

                const pos = [lat, lng];

                bounds.extend(pos);

                L.marker(pos)
                    .addTo(markers)
                    .bindPopup(`
                        <b>${c.nombre}</b><br>
                        ${c.calle} ${c.altura}
                    `);
            });

            if (consorcios.length > 0) {
                map.fitBounds(bounds, {
                    padding: [30, 30]
                });
            }

            
            setTimeout(() => {
                map.invalidateSize();
            }, 200);
        })
        .catch(err => {
            console.error("Error cargando consorcios:", err);
        });
}