var geev = geev || {};
(function () {

    if (!toastr) {
        return;
    }

    /* DEFAULTS *************************************************/

    toastr.options.positionClass = 'toast-bottom-right';

    /* NOTIFICATION *********************************************/

    var showNotification = function (type, message, title, options) {
        toastr[type](message, title, options);
    };

    geev.notify.success = function (message, title, options) {
        showNotification('success', message, title, options);
    };

    geev.notify.info = function (message, title, options) {
        showNotification('info', message, title, options);
    };

    geev.notify.warn = function (message, title, options) {
        showNotification('warning', message, title, options);
    };

    geev.notify.error = function (message, title, options) {
        showNotification('error', message, title, options);
    };

})();