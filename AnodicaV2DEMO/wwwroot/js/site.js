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


// DARK MODE / LIGHT MODE
document.addEventListener('DOMContentLoaded', () => {
    const elementoHtml = document.documentElement;
    const btnCambiarTema = document.getElementById('btnCambiarTema');
    const iconoTema = document.getElementById('iconoTema');

    function actualizarIcono(tema) {
        if (tema === 'dark') {
            // Modo Oscuro
            iconoTema.className = 'bi bi-sun-fill text-warning fs-5';
            btnCambiarTema.className = 'btn btn-outline-warning rounded-circle ms-auto me-4 border-opacity-50';
        } else {
            // Modo Claro
            iconoTema.className = 'bi bi-moon-fill text-white fs-5';
            btnCambiarTema.className = 'btn btn-outline-light rounded-circle ms-auto me-4 border-opacity-50';
        }
    }

    const temaActual = elementoHtml.getAttribute('data-bs-theme');
    actualizarIcono(temaActual);

    btnCambiarTema.addEventListener('click', () => {
        const temaActivo = elementoHtml.getAttribute('data-bs-theme');
        const nuevoTema = temaActivo === 'light' ? 'dark' : 'light';

        elementoHtml.setAttribute('data-bs-theme', nuevoTema);
        localStorage.setItem('tema', nuevoTema);
        actualizarIcono(nuevoTema);
    });
});