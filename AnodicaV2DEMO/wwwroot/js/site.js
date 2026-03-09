function ConfirmarEliminacion(id, nombre) {
    Swal.fire({
        title: '¿Estás seguro?',
        text: "Vas a borrar: " + nombre,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        confirmButtonText: 'Sí, borrar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            Swal.fire({
                title: 'Eliminando...',
                didOpen: () => { Swal.showLoading(); }
            });
            document.getElementById('form-eliminar-' + id).submit();
        }
    })
}

$(document).ready(function () {
    $('.tabla-anodica').DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json"
        },
        "pageLength": 10,
        "ordering": true,
        "info": true
    });
});

