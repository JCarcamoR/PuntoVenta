var lblNombre,
    lblApePaterno,
    lblApeMaterno,
    lblTelefono,
    lblCorreo,
    lblAsunto,
    lblAsunto2,
    lblDescripcion;

var txtNombre,
    txtApePaterno,
    txtApeMaterno,
    txtTelefono,
    txtCorreo,
    txtAsunto,
    txtDescripcion,
    error = "";

AsociaDatos();
LimpiaDatos();

function AsociaDatos() {
    lblNombre = document.getElementById("txtNombre");
    lblApePaterno = document.getElementById("txtApePaterno");
    lblApeMaterno = document.getElementById("txtApeMaterno");
    lblTelefono = document.getElementById("txtTelefono");
    lblCorreo = document.getElementById("txtCorreo");
    lblAsunto = document.getElementById("ddlAsunto");
    lblAsunto2 = document.getElementById("txtOasunto");
    lblDescripcion = document.getElementById("txtDescripcion");
}

lblAsunto.addEventListener("click", function () {
    if (lblAsunto.selectedIndex == "3") {
        lblAsunto.style.display = "none";
        lblAsunto2.style.display = "block";
    }
});

function LimpiaDatos() {
    lblNombre.value = "";
    lblApePaterno.value = "";
    lblApeMaterno.value = "";
    lblTelefono.value = "";
    lblCorreo.value = "";
    lblAsunto.selectedIndex = "0";
    lblAsunto.style.display = "block";
    lblAsunto2.value = "";
    lblAsunto2.style.display = "none";
    lblDescripcion.value = "";
}

function ObtenDatos() {
    txtNombre = lblNombre.value;
    txtApePaterno = lblApePaterno.value;
    txtApeMaterno = lblApeMaterno.value;
    txtTelefono = lblTelefono.value;
    txtCorreo = lblCorreo.value
    switch (lblAsunto.selectedIndex) {
        case 1:
            txtAsunto = "Error en el Sistema.";
            break;
        case 2:
            txtAsunto = "Solicitud de Contacto.";
            break;
        case 3:
            txtAsunto = lblAsunto2.value;
            break;
        default:
            txtAsunto = "";
    }
    txtDescripcion = lblDescripcion.value;
}

function ValidaDatos() {
    var caracteres = ["'", ";","=","/"];

    if (validarCadena(txtNombre, true, caracteres, true)) {
        if (validarCadena(txtApePaterno, true, caracteres, true)) {
            if (validarCadena(txtApeMaterno, true, caracteres, true)) {
                if (validarNumero(txtTelefono)) {
                    if (validarCadena(txtCorreo, true, [], false, true)) {
                        if (validarCadena(txtAsunto, true, caracteres, true)) {
                            if (validarCadena(txtDescripcion, true, caracteres, true)) {
                                return true;
                            } else {
                                error = "Se encontraron errores en el campo <strong>DESCRIPCION</strong>";
                                return false;
                            }
                        } else {
                            error = "Se encontraron errores en el campo <strong>ASUNTO</strong>";
                            return false;
                        }
                    } else {
                        error = "Se encontraron errores en el campo <strong>CORREO</strong>";
                        return false;
                    }
                } else {
                    error = "Se encontraron errores en el campo <strong>TELEFONO</strong>";
                    return false;
                }
            } else {
                error = "Se encontraron errores en el campo <strong>APELLIDO MATERNO</strong>";
                return false;
            }
        } else {
            error = "Se encontraron errores en el campo <strong>APELLIDO PATERNO</strong>";
            return false;
        }
    } else {
        error = "Se encontraron errores en el campo <strong>NOMBRE</strong>";
        return false;
    }
}

function SenMailContact() {
    showSpinner();
    ObtenDatos();
    if (ValidaDatos()) {
        SendMailContact();
    } else {
        hideSpinner();
        alert(error);
    }
}


function SendMailContact() {
    var jsonMail = {
        nombre: txtNombre + ' ' + txtApePaterno + ' ' + txtApeMaterno,
        telefono: txtTelefono,
        correo: txtCorreo,
        asunto: txtAsunto,
        descripcion: txtDescripcion
    }

    $.ajax({
        url: '/Home/Contact',
        method: 'POST',
        dataType: 'json',
        contentType: 'application/json',
        data: jsonMail,
        success: function (data) {
            if (response.success) {
                alert("Solicitud de contacto enviada correctamente, a la brevedad un agente se pondrá en contacto con usted");
            } else {
                alert('Ocurrió un error al enviar la solicitud: ' + response.message);
            }
        },
        error: function (xhr, status, error) {
            alert(error);
        },
        complete: function () {
            hideSpinner()
        }
    });
}




