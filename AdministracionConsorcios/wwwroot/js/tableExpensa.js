$(document).ready(function () {
    $('#tablaExpensas').DataTable({
        ajax: {
            url: '/Expensa/GetExpensas?Id=' + consorcioId,
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'año' },
            { data: 'mes' },
            { data: 'gastoMes', render: data => '$' + data.toLocaleString('es-AR') },
            { data: 'montoXUnidad', render: data => '$' + data.toLocaleString('es-AR') }
        ],
        ordering: false,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
        }
    });
});