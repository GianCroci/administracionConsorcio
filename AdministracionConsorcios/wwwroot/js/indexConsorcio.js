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
    $(".custom-button").append('<a href="/Consorcio/Crear" class="bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2 px-4 rounded transition-colors">Nuevo Consorcio</a>');
});