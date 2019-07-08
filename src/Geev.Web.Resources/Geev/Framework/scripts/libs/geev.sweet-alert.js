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

    var showMessage = function (type, message, title, isHtml) {

        if (!title) {
            title = message;
            message = undefined;
        }

        var messageContent = {
            title: title
        };

        if (isHtml) {
            var el = document.createElement('div');
            el.innerHTML = message;

            messageContent.content = el;
        } else {
            messageContent.text = message;
        }

        var opts = $.extend(
            {},
            geev.libs.sweetAlert.config['default'],
            geev.libs.sweetAlert.config[type],
            messageContent
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts).then(function () {
                $dfd.resolve();
            });
        });
    };

    geev.message.info = function (message, title, isHtml) {
        return showMessage('info', message, title, isHtml);
    };

    geev.message.success = function (message, title, isHtml) {
        return showMessage('success', message, title, isHtml);
    };

    geev.message.warn = function (message, title, isHtml) {
        return showMessage('warn', message, title, isHtml);
    };

    geev.message.error = function (message, title, isHtml) {
        return showMessage('error', message, title, isHtml);
    };

    geev.message.confirm = function (message, titleOrCallback, callback, isHtml) {
        var messageContent;

        if (isHtml) {
            var el = document.createElement('div');
            el.innerHTML = message;

            messageContent = {
                content: el
            }
        } else {
            messageContent = {
                text: message
            }
        }

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            messageContent.title = titleOrCallback;
        };

        var opts = $.extend(
            {},
            geev.libs.sweetAlert.config['default'],
            geev.libs.sweetAlert.config.confirm,
            messageContent
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts).then(function (isConfirmed) {
                callback && callback(isConfirmed);
                $dfd.resolve(isConfirmed);
            });
        });
    };

    geev.event.on('geev.dynamicScriptsInitialized', function () {
        geev.libs.sweetAlert.config.confirm.title = geev.localization.geevWeb('AreYouSure');
        geev.libs.sweetAlert.config.confirm.buttons = [geev.localization.geevWeb('Cancel'), geev.localization.geevWeb('Yes')];
    });

})(jQuery);