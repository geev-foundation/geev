var geev = geev || {};
(function ($) {
    if (!sweetAlert || !$) {
        return;
    }

    /* DEFAULTS *************************************************/

    geev.libs = geev.libs || {};
    geev.libs.sweetAlert = {
        config: {
            'default': {

            },
            info: {
                icon: 'info'
            },
            success: {
                icon: 'success'
            },
            warn: {
                icon: 'warning'
            },
            error: {
                icon: 'error'
            },
            confirm: {
                icon: 'warning',
                title: 'Are you sure?',
                buttons: ['Cancel', 'Yes']
            }
        }
    };

    /* MESSAGE **************************************************/

    var showMessage = function (type, message, title) {
        if (!title) {
            title = message;
            message = undefined;
        }

        var opts = $.extend(
            {},
            geev.libs.sweetAlert.config['default'],
            geev.libs.sweetAlert.config[type],
            {
                title: title,
                text: message
            }
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts).then(function () {
                $dfd.resolve();
            });
        });
    };

    geev.message.info = function (message, title) {
        return showMessage('info', message, title);
    };

    geev.message.success = function (message, title) {
        return showMessage('success', message, title);
    };

    geev.message.warn = function (message, title) {
        return showMessage('warn', message, title);
    };

    geev.message.error = function (message, title) {
        return showMessage('error', message, title);
    };

    geev.message.confirm = function (message, titleOrCallback, callback) {
        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            geev.libs.sweetAlert.config['default'],
            geev.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts).then(function (isConfirmed) {
                if (isConfirmed) {
                    callback && callback(isConfirmed);
                }
                $dfd.resolve(isConfirmed);
            });
        });
    };

    geev.event.on('geev.dynamicScriptsInitialized', function () {
        geev.libs.sweetAlert.config.confirm.title = geev.localization.geevWeb('AreYouSure');
        geev.libs.sweetAlert.config.confirm.buttons = [geev.localization.geevWeb('Cancel'), geev.localization.geevWeb('Yes')];
    });

})(jQuery);