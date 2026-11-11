
function showLoader(tipo = null, texto = null) {

	var tipos = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];

	if (!tipo || !tipos.includes(tipo)) {
		var tipo = 8;
	}

	Swal.fire({
		html: (!texto ? 'Aguarde...' : texto),
		imageUrl: '/loader/loaders/loader_' + tipo + '.gif',
		imageWidth: 64,
		imageHeight: 64,
		width: 'max-content',
		customClass: {
			htmlContainer: 'margin-container',
			image: 'image-margin',
			popup: 'popup-padding'
		},
		showConfirmButton: false,
		allowOutsideClick: false,
		allowEscapeKey: false
	});

	$('#swal2-content').addClass('padding-message');
}

function closeLoader() {

	if (Swal.isVisible()) {
		Swal.close();
	}
}