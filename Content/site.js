$(document).ready(function () {
    // Función para alternar el menú lateral
    function toggleSidebar() {
        $('#sidebar').toggleClass('collapsed');
        $('#mainContent').toggleClass('expanded');

        // En móviles
        if ($(window).width() < 768) {
            $('#sidebar').toggleClass('show');
            $('#overlay').toggleClass('active');
        }

        localStorage.setItem('sidebarCollapsed', $('#sidebar').hasClass('collapsed'));
    }

    // Evento para el botón del menú
    $('#sidebarToggle').on('click', toggleSidebar);

    // Evento para el overlay
    $('#overlay').on('click', function () {
        if ($(window).width() < 768) {
            toggleSidebar();
        }
    });

    // Cargar estado inicial
    if (localStorage.getItem('sidebarCollapsed') === 'true') {
        $('#sidebar').addClass('collapsed');
        $('#mainContent').addClass('expanded');
    }

    // Manejar redimensionamiento
    $(window).on('resize', function () {
        if ($(window).width() >= 768) {
            $('#overlay').removeClass('active');
        }
    }).trigger('resize');
});

function showSpinner() {
    const spinner = document.getElementById('globalSpinner');
    if (spinner) {
        spinner.style.display = 'flex';
        document.body.style.overflow = 'hidden';
    }
}

function hideSpinner() {
    const spinner = document.getElementById('globalSpinner');
    if (spinner) {
        spinner.style.display = 'none';
        document.body.style.overflow = '';
    }
}

function validarCadena(
    cadena,             // Parámetro A: Cadena a validar (string)
    validarNoVacia,     // Parámetro B: Validar que no esté vacía (boolean)
    caracteresProhibidos, // Parámetro C: Array de caracteres no permitidos (array)
    usarCaracteresProhibidos, // Parámetro D: Si se aplica la validación de caracteres (boolean)
    validarEmail        // Parámetro E: Validar formato de email (boolean)
) {
    // Validación 1: Verificar que es una cadena (Parámetro A)
    if (typeof cadena !== 'string') {
        return false;
    }

    // Validación 2: Cadena no vacía (Parámetro B)
    if (validarNoVacia && cadena.trim() === '') {
        return false;
    }

    // Validación 3: Caracteres prohibidos (Parámetros C y D)
    if (usarCaracteresProhibidos && caracteresProhibidos.some(c => cadena.includes(c))) {
        return false;
    }

    // Validación 4: Formato de email (Parámetro E)
    if (validarEmail) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(cadena)) {
            return false;
        }
    }

    // Si pasa todas las validaciones
    return true;
}

function validarNumero(cadena) {
    // Elimina espacios en blanco al inicio y final
    const cadenaLimpia = cadena.trim();

    // Verifica si la cadena está vacía después de trim
    if (cadenaLimpia === "") {
        return false;
    }

    // Usa una expresión regular para validar números (enteros o decimales)
    // Permite números como: 123, -123, 123.45, -123.45, .45, -.45
    const numeroRegex = /^-?\d*\.?\d+$/;

    return numeroRegex.test(cadenaLimpia);
}