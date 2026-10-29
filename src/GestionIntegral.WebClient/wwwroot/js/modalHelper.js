(function (window, $) {
    'use strict';

    window.ModalHelper = {
        confirm: function (options) {
            var title = options.title || 'Confirmar';
            var message = options.message || '¿Está seguro?';
            var confirmText = options.confirmText || 'Aceptar';
            var cancelText = options.cancelText || 'Cancelar';
            var onConfirm = options.onConfirm || function () { };
            var onCancel = options.onCancel || function () { };

            var $modal = $('#confirmModal');
            $modal.find('.modal-title').text(title);
            $modal.find('#confirmModalBody').html(message);
            $modal.find('#confirmModalAccept').text(confirmText).off('click').on('click', function () {
                $modal.modal('hide');
                onConfirm();
            });
            $modal.find('.btn-secondary').text(cancelText).off('click').on('click', function () {
                $modal.modal('hide');
                onCancel();
            });

            $modal.modal('show');
        },

        notify: function (options) {
            var type = options.type || 'info';
            var title = options.title || 'Notificación';
            var message = options.message || '';

            var $modal = $('#notificationModal');
            var $header = $modal.find('#notificationModalHeader');

            $header.removeClass('bg-success bg-danger bg-info bg-warning text-white');

            switch (type) {
                case 'success':
                    $header.addClass('bg-success text-white');
                    break;
                case 'error':
                case 'danger':
                    $header.addClass('bg-danger text-white');
                    break;
                case 'warning':
                    $header.addClass('bg-warning');
                    break;
                default:
                    $header.addClass('bg-info text-white');
                    break;
            }

            $modal.find('#notificationModalLabel').text(title);
            $modal.find('#notificationModalBody').html(message);
            $modal.modal('show');
        }
    };

    // Inicializar botones de confirmación con data-confirm
    $(document).ready(function () {
        $('body').on('click', '[data-confirm]', function (e) {
            e.preventDefault();
            var $trigger = $(this);
            var message = $trigger.data('confirm') || '¿Está seguro?';
            var title = $trigger.data('confirm-title') || 'Confirmar';
            var target = $trigger.data('confirm-target');

            ModalHelper.confirm({
                title: title,
                message: message,
                onConfirm: function () {
                    if (target) {
                        $(target).submit();
                    } else {
                        var $form = $trigger.closest('form');
                        if ($form.length) {
                            $form.submit();
                        } else {
                            window.location.href = $trigger.attr('href');
                        }
                    }
                }
            });
        });
    });
})(window, jQuery);
