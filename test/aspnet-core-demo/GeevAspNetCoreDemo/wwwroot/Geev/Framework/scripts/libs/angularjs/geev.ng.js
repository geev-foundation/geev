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
            location.href = targetUrl;
        },

        handleUnAuthorizedRequest: function (messagePromise, targetUrl) {
            if (messagePromise) {
                messagePromise.done(function () {
                    if (!targetUrl) {
                        location.reload();
                    } else {
                        geev.ng.http.handleTargetUrl(targetUrl);
                    }
                });
            } else {
                if (!targetUrl) {
                    location.reload();
                } else {
                    geev.ng.http.handleTargetUrl(targetUrl);
                }
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
                    messagePromise = geev.ng.http.showError(originalData.error);
                } else {
                    originalData.error = defaultError;
                }

                geev.ng.http.logError(originalData.error);

                response.data = originalData.error;
                defer.reject(response);

                if (originalData.unAuthorizedRequest) {
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
                            return response;
                        }

                        var defer = $q.defer();
                        geev.ng.http.handleResponse(response, defer);
                        return defer.promise;
                    },

                    'responseError': function (ngError) {
                        if (!ngError.data || !ngError.data.__geev) {
                            geev.ng.http.showError(geev.ng.http.defaultError);
                            return ngError;
                        }

                        var defer = $q.defer();
                        geev.ng.http.handleResponse(ngError, defer);
                        return defer.promise;
                    }

                };
            }]);
        }
    ]);

    geev.event.on('geev.dynamicScriptsInitialized', function () {
        geev.ng.http.defaultError.message = geev.localization.geevWeb('DefaultError');
        geev.ng.http.defaultError.details = geev.localization.geevWeb('DefaultErrorDetail');
    });

})((geev || (geev = {})), (angular || undefined));