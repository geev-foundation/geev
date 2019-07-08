var geev = geev || {};
(function ($) {

    if (!$) {
        return;
    }

    /* JQUERY ENHANCEMENTS ***************************************************/

    // geev.ajax -> uses $.ajax ------------------------------------------------

    geev.ajax = function (userOptions) {
        userOptions = userOptions || {};

        var options = $.extend(true, {}, geev.ajax.defaultOpts, userOptions);
        var oldBeforeSendOption = options.beforeSend;		
        options.beforeSend = function(xhr) {
            if (oldBeforeSendOption) {
                 oldBeforeSendOption(xhr);
            }

            xhr.setRequestHeader("Pragma", "no-cache");
            xhr.setRequestHeader("Cache-Control", "no-cache");
            xhr.setRequestHeader("Expires", "Sat, 01 Jan 2000 00:00:00 GMT");
        };

        options.success = undefined;
        options.error = undefined;

        return $.Deferred(function ($dfd) {
            $.ajax(options)
                .done(function (data, textStatus, jqXHR) {
                    if (data.__geev) {
                        geev.ajax.handleResponse(data, userOptions, $dfd, jqXHR);
                    } else {
                        $dfd.resolve(data);
                        userOptions.success && userOptions.success(data);
                    }
                }).fail(function (jqXHR) {
                    if (jqXHR.responseJSON && jqXHR.responseJSON.__geev) {
                        geev.ajax.handleResponse(jqXHR.responseJSON, userOptions, $dfd, jqXHR);
                    } else {
                        geev.ajax.handleNonGeevErrorResponse(jqXHR, userOptions, $dfd);
                    }
                });
        });
    };

    $.extend(geev.ajax, {
        defaultOpts: {
            dataType: 'json',
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        },

        defaultError: {
            message: 'An error has occurred!',
            details: 'Error detail not sent by server.'
        },

        defaultError401: {
            message: 'You are not authenticated!',
            details: 'You should be authenticated (sign in) in order to perform this operation.'
        },

        defaultError403: {
            message: 'You are not authorized!',
            details: 'You are not allowed to perform this operation.'
        },

        defaultError404: {
            message: 'Resource not found!',
            details: 'The resource requested could not found on the server.'
        },

        logError: function (error) {
            geev.log.error(error);
        },

        showError: function (error) {
            if (error.details) {
                return geev.message.error(error.details, error.message);
            } else {
                return geev.message.error(error.message || geev.ajax.defaultError.message);
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = geev.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleNonGeevErrorResponse: function (jqXHR, userOptions, $dfd) {
            if (userOptions.geevHandleError !== false) {
                switch (jqXHR.status) {
                    case 401:
                        geev.ajax.handleUnAuthorizedRequest(
                            geev.ajax.showError(geev.ajax.defaultError401),
                            geev.appPath
                        );
                        break;
                    case 403:
                        geev.ajax.showError(geev.ajax.defaultError403);
                        break;
                    case 404:
                        geev.ajax.showError(geev.ajax.defaultError404);
                        break;
                    default:
                        geev.ajax.showError(geev.ajax.defaultError);
                        break;
                }
            }

            $dfd.reject.apply(this, arguments);
            userOptions.error && userOptions.error.apply(this, arguments);
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    geev.ajax.handleTargetUrl(targetUrl);
                });
            } else {
                geev.ajax.handleTargetUrl(targetUrl);
            }
        },

        handleResponse: function (data, userOptions, $dfd, jqXHR) {
            if (data) {
                if (data.success === true) {
                    $dfd && $dfd.resolve(data.result, data, jqXHR);
                    userOptions.success && userOptions.success(data.result, data, jqXHR);

                    if (data.targetUrl) {
                        geev.ajax.handleTargetUrl(data.targetUrl);
                    }
                } else if (data.success === false) {
                    var messagePromise = null;

                    if (data.error) {
                        if (userOptions.geevHandleError !== false) {
                            messagePromise = geev.ajax.showError(data.error);
                        }
                    } else {
                        data.error = geev.ajax.defaultError;
                    }

                    geev.ajax.logError(data.error);

                    $dfd && $dfd.reject(data.error, jqXHR);
                    userOptions.error && userOptions.error(data.error, jqXHR);

                    if (jqXHR.status === 401 && userOptions.geevHandleError !== false) {
                        geev.ajax.handleUnAuthorizedRequest(messagePromise, data.targetUrl);
                    }
                } else { //not wrapped result
                    $dfd && $dfd.resolve(data, null, jqXHR);
                    userOptions.success && userOptions.success(data, null, jqXHR);
                }
            } else { //no data sent to back
                $dfd && $dfd.resolve(jqXHR);
                userOptions.success && userOptions.success(jqXHR);
            }
        },

        blockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //block whole page
                    geev.ui.setBusy();
                } else { //block an element
                    geev.ui.setBusy(options.blockUI);
                }
            }
        },

        unblockUI: function (options) {
            if (options.blockUI) {
                if (options.blockUI === true) { //unblock whole page
                    geev.ui.clearBusy();
                } else { //unblock an element
                    geev.ui.clearBusy(options.blockUI);
                }
            }
        },

        ajaxSendHandler: function (event, request, settings) {
            var token = geev.security.antiForgery.getToken();
            if (!token) {
                return;
            }

            if (!geev.security.antiForgery.shouldSendToken(settings)) {
                return;
            }

            if (!settings.headers || settings.headers[geev.security.antiForgery.tokenHeaderName] === undefined) {
                request.setRequestHeader(geev.security.antiForgery.tokenHeaderName, token);
            }
        }
    });

    $(document).ajaxSend(function (event, request, settings) {
        return geev.ajax.ajaxSendHandler(event, request, settings);
    });

    /* JQUERY PLUGIN ENHANCEMENTS ********************************************/

    /* jQuery Form Plugin 
     * http://www.malsup.com/jquery/form/
     */

    // geevAjaxForm -> uses ajaxForm ------------------------------------------

    if ($.fn.ajaxForm) {
        $.fn.geevAjaxForm = function (userOptions) {
            userOptions = userOptions || {};

            var options = $.extend({}, $.fn.geevAjaxForm.defaults, userOptions);

            options.beforeSubmit = function () {
                geev.ajax.blockUI(options);
                userOptions.beforeSubmit && userOptions.beforeSubmit.apply(this, arguments);
            };

            options.success = function (data) {
                geev.ajax.handleResponse(data, userOptions);
            };

            //TODO: Error?

            options.complete = function () {
                geev.ajax.unblockUI(options);
                userOptions.complete && userOptions.complete.apply(this, arguments);
            };

            return this.ajaxForm(options);
        };

        $.fn.geevAjaxForm.defaults = {
            method: 'POST'
        };
    }

    geev.event.on('geev.dynamicScriptsInitialized', function () {
        geev.ajax.defaultError.message = geev.localization.geevWeb('DefaultError');
        geev.ajax.defaultError.details = geev.localization.geevWeb('DefaultErrorDetail');
        geev.ajax.defaultError401.message = geev.localization.geevWeb('DefaultError401');
        geev.ajax.defaultError401.details = geev.localization.geevWeb('DefaultErrorDetail401');
        geev.ajax.defaultError403.message = geev.localization.geevWeb('DefaultError403');
        geev.ajax.defaultError403.details = geev.localization.geevWeb('DefaultErrorDetail403');
        geev.ajax.defaultError404.message = geev.localization.geevWeb('DefaultError404');
        geev.ajax.defaultError404.details = geev.localization.geevWeb('DefaultErrorDetail404');
    });

})(jQuery);
