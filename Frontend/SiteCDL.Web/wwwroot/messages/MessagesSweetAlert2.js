
function showMessage(tipo, mensagem, titulo = '', functionClicouOk = '', objParams = '') {

    if (tipo && mensagem) {

        var tipo = tipo.toLowerCase();

        switch (tipo) {
            case 'success':
                var confirmButtonClass = 'btn btn-success';
                break;
            case 'error':
                var confirmButtonClass = 'btn btn-danger';
                break;
            case 'warning':
                var confirmButtonClass = 'btn btn-warning';
                break;
            case 'info':
                var confirmButtonClass = 'btn btn-info';
                break;
            default:
                var confirmButtonClass = 'btn btn-black';
                var tipo = 'question';
                var titulo = 'Tipo desconhecido';
                var mensagem = 'O tipo <strong>"' + tipo + '"</strong> não foi reconhecido. ' +
                    'São válidos apenas os tipos:<br /><strong>"success"</strong>, ' +
                    '<strong>"error"</strong>, <strong>"warning"</strong> e <strong>"info"</strong>!';
        }

        Swal.fire({
            title: titulo,
            icon: tipo,
            html: mensagem,
            confirmButtonText: '<strong style="color: #ffffff">Ok!</strong>',
            width: 380,
            buttonsStyling: false,
            customClass: {
                confirmButton: confirmButtonClass,
                title: 'fs-4',
                content: 'fs-3'
            },
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            if (result.value) {
                if (functionClicouOk) {
                    functionClicouOk(objParams);
                }
            }
        });

    } else {

        var mensagem = 'Para usar o método <strong>"showMessage"</strong> os seguintes parâmetros são ' +
            'obrigatórios: <strong>"tipo"</strong> e <strong>"mensagem"</strong>.<br />Há ainda os parâmetros ' +
            'opcionais <strong>"título"</strong>, <strong>"functionClicouOk"</strong> e <strong>"objParams"</strong>.' +
            '<br />Informe-os nesta mesma ordem!';

        showMessage('error', mensagem, 'Erro!');
    }
}

function showConfirmation(titulo, mensagem, functionClicouSIM, objParamsSIM = '', textoBotaoSIM = '', textoBotaoNAO = '', functionClicouNAO = '', objParamsNAO = '') {

    if (titulo && mensagem && functionClicouSIM) {

        var botaoSIM = (textoBotaoSIM) ? textoBotaoSIM : '&nbsp;&nbsp;SIM!&nbsp;&nbsp;';
        var botaoNAO = (textoBotaoNAO) ? textoBotaoNAO : '&nbsp;&nbsp;NÃO!&nbsp;&nbsp;';

        Swal.fire({
            title: titulo,
            icon: 'question',
            html: mensagem,
            confirmButtonText: '<strong style="color: #ffffff">' + botaoSIM + '</strong>',
            showCancelButton: true,
            cancelButtonText: '<strong style="color: #ffffff">' + botaoNAO + '</strong>',
            width: 380,
            buttonsStyling: false,
            customClass: {
                confirmButton: 'btn btn-success btn-confirm',
                cancelButton: 'btn btn-danger btn-confirm',
                title: 'fs-4',
                content: 'fs-3'
            },
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            if (result.value) {
                functionClicouSIM(objParamsSIM);
            } else {
                if (functionClicouNAO) {
                    functionClicouNAO(objParamsNAO);
                }
            }
        });

    } else {

        var mensagem = 'Para usar o método <strong>"showConfirmation"</strong> os seguintes parâmetros são ' +
            'obrigatórios: <strong>"título"</strong>, <strong>"mensagem"</strong> e <strong>"functionClicouSIM".</strong>' +
            '<br />Há ainda os parâmetros opcionais <strong>"objParamsSIM"</strong>, <strong>"textoBotaoSIM"</strong>, ' +
            '<strong>"textoBotaoNAO"</strong>, <strong>"functionClicouNAO"</strong> e <strong>"objParamsNAO"</strong>.' +
            '<br />Informe-os nesta mesma ordem!';

        showMessage('error', mensagem, 'Erro!');
    }
}

function showMessageRedirect(tipo, mensagem, url, timer = 2000, titulo = '') {

    if (tipo && mensagem && timer && url) {

        Swal.fire({
            title: titulo,
            icon: tipo,
            html: mensagem,
            timer: timer,
            timerProgressBar: true,
            showConfirmButton: false,
            allowOutsideClick: false,
            allowEscapeKey: false
        }).then((result) => {
            if (result.dismiss === Swal.DismissReason.timer) {
                window.location.href = window.location.origin + url;
            }
        });

        $('.swal2-timer-progress-bar').addClass('progress-bar-azul');

    } else {

        var mensagem = 'Para usar o método <strong>"showMessageRedirect"</strong> os seguintes parâmetros são ' +
            'obrigatórios: <strong>"tipo"</strong>, <strong>"mensagem"</strong>, <strong>timer</srong> e <strong>url</strong>.<br />' +
            'Há ainda o parâmetro opcional <strong>"título"</strong>.<br />Informe-os nesta mesma ordem!';

        showMessage('error', mensagem, 'Erro!');
    }
}