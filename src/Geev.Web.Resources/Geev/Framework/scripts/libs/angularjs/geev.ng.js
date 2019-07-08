(function (geev, angular) {

    if (!angular) {
        return;
    }

    geev.ng = geev.ng || {};

    geev.ng.http = {
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
            details: 'The resource requested could not be found on the server.'
        },

        logError: function (error) {
            geev.log.error(error);
        },

        showError: function (error) {
            if (error.details) {
                return geev.message.error(error.details, error.message || geev.ng.http.defaultError.message);
            } else {
                return geev.message.error(error.message || geev.ng.http.defaultError.message);
            }
        },

        handleTargetUrl: function (targetUrl) {
            if (!targetUrl) {
                location.href = geev.appPath;
            } else {
                location.href = targetUrl;
            }
        },

        handleNonGeevErrorResponse: function (response, defer) {
            if (response.config.geevHandleError !== false) {
                switch (response.status) {
                    case 401:
                        geev.ng.http.handleUnAuthorizedRequest(
                            geev.ng.http.showError(geev.ng.http.defaultError401),
                            geev.appPath
                        );
                        break;
                    case 403:
                        geev.ng.http.showError(geev.ajax.defaultError403);
                        break;
                    case 404:
                        geev.ng.http.showError(geev.ajax.defaultError404);
                        break;
                    default:
                        geev.ng.http.showError(geev.ng.http.defaultError);
                        break;
                }
            }

            defer.reject(response);
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    geev.ng.http.handleTargetUrl(targetUrl || geev.appPath);
                });
            } else {
                geev.ng.http.handleTargetUrl(targetUrl || geev.appPath);
            }
        },

        handleResponse: function (response, defer) {
            var originalData = response.data;

            if (originalData.success === true) {
                response.data = originalData.result;
                defer.resolve(response);

                if (originalData.targetUrl) {
                    geev.ng.http.handleTargetUrl(originalData.targetUrl);
                }
            } else if (originalData.success === false) {
                var messagePromise = null;

                if (originalData.error) {
                    if (response.config.geevHandleError !== false) {
                        messagePromise = geev.ng.http.showError(originalData.error);
                    }
                } else {
                    originalData.error = defaultError;
                }

                geev.ng.http.logError(originalData.error);

                response.data = originalData.error;
                defer.reject(response);

                if (response.status == 401 && response.config.geevHandleError !== false) {
                    geev.ng.http.handleUnAuthorizedRequest(messagePromise, originalData.targetUrl);
                }
            } else { //not wrapped result
                defer.resolve(response);
            }
        }
    }

    var geevModule = angular.module('geev', []);

    geevModule.config([
        '$httpProvider', function ($httpProvider) {
            $httpProvider.interceptors.push(['$q', function ($q) {

                return {

                    'request': function (config) {
                        if (config.url.indexOf('.cshtml') !== -1) {
                            config.url = geev.appPath + 'GeevAppView/Load?viewUrl=' + config.url + '&_t=' + geev.pageLoadTime.getTime();
                        }

                        return config;
                    },

                    'response': function (response) {
                        if (!response.data || !response.data.__geev) {
                            //Non ABP related return value
                            return response;
                        }

                        var defer = $q.defer();
                        geev.ng.http.handleResponse(response, defer);
                        return defer.promise;
                    },

                    'responseError': function (ngError) {
                        var defer = $q.defer();

                        if (!ngError.data || !ngError.data.__geev) {
                            geev.ng.http.handleNonGeevErrorResponse(ngError, defer);
                        } else {
                            geev.ng.http.handleResponse(ngError, defer);
                        }

                        return defer.promise;
                    }

                };
            }]);
        }
    ]);

    geev.event.on('geev.dynamicScriptsInitialized', function () {
        geev.ng.http.defaultError.message = geev.localization.geevWeb('DefaultError');
        geev.ng.http.defaultError.details = geev.localization.geevWeb('DefaultErrorDetail');
        geev.ng.http.defaultError401.message = geev.localization.geevWeb('DefaultError401');
        geev.ng.http.defaultError401.details = geev.localization.geevWeb('DefaultErrorDetail401');
        geev.ng.http.defaultError403.message = geev.localization.geevWeb('DefaultError403');
        geev.ng.http.defaultError403.details = geev.localization.geevWeb('DefaultErrorDetail403');
        geev.ng.http.defaultError404.message = geev.localization.geevWeb('DefaultError404');
        geev.ng.http.defaultError404.details = geev.localization.geevWeb('DefaultErrorDetail404');
    });

})((geev || (geev = {})), (angular || undefined));