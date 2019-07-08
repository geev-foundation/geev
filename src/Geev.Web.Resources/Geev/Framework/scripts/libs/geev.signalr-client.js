var geev = geev || {};
(function () {

    // Check if SignalR is defined
    if (!signalR) {
        return;
    }

    // Create namespaces
    geev.signalr = geev.signalr || {};
    geev.signalr.hubs = geev.signalr.hubs || {};

    // Configure the connection
    function configureConnection(connection) {
        // Set the common hub
        geev.signalr.hubs.common = connection;

        // Reconnect if hub disconnects
        connection.onclose(function (e) {
            if (e) {
                geev.log.debug('Connection closed with error: ' + e);
            }
            else {
                geev.log.debug('Disconnected');
            }

            if (!geev.signalr.autoConnect) {
                return;
            }

            setTimeout(function () {
                connection.start();
            }, 5000);
        });

        // Register to get notifications
        connection.on('getNotification', function (notification) {
            geev.event.trigger('geev.notifications.received', notification);
        });
    }

    // Connect to the server
    geev.signalr.connect = function () {
        var url = geev.signalr.url || (geev.appPath + 'signalr');

        // Start the connection.
        startConnection(url, configureConnection)
            .then(function (connection) {
                geev.log.debug('Connected to SignalR server!'); //TODO: Remove log
                geev.event.trigger('geev.signalr.connected');
                // Call the Register method on the hub.
                connection.invoke('register').then(function () {
                    geev.log.debug('Registered to the SignalR server!'); //TODO: Remove log
                });
            })
            .catch(function (error) {
                geev.log.debug(error.message);
            });
    };

    // Starts a connection with transport fallback - if the connection cannot be started using
    // the webSockets transport the function will fallback to the serverSentEvents transport and
    // if this does not work it will try longPolling. If the connection cannot be started using
    // any of the available transports the function will return a rejected Promise.
    function startConnection(url, configureConnection) {
        if (geev.signalr.remoteServiceBaseUrl) {
            url = geev.signalr.remoteServiceBaseUrl + url;
        }

        // Add query string: https://github.com/aspnet/SignalR/issues/680
        if (geev.signalr.qs) {
            url += (url.indexOf('?') == -1 ? '?' : '&') + geev.signalr.qs;
        }

        return function start(transport) {
            geev.log.debug('Starting connection using ' + signalR.HttpTransportType[transport] + ' transport');
            var connection = new signalR.HubConnectionBuilder()
                .withUrl(url, transport)
                .build();
            if (configureConnection && typeof configureConnection === 'function') {
                configureConnection(connection);
            }

            return connection.start()
                .then(function () {
                    return connection;
                })
                .catch(function (error) {
                    geev.log.debug('Cannot start the connection using ' + signalR.HttpTransportType[transport] + ' transport. ' + error.message);
                    if (transport !== signalR.HttpTransportType.LongPolling) {
                        return start(transport + 1);
                    }

                    return Promise.reject(error);
                });
        }(signalR.HttpTransportType.WebSockets);
    }

    geev.signalr.startConnection = startConnection;

    if (geev.signalr.autoConnect === undefined) {
        geev.signalr.autoConnect = true;
    }

    if (geev.signalr.autoConnect && !geev.signalr.hubs.common) {
        geev.signalr.connect();
    }

})();
