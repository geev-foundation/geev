var geev = geev || {};
(function ($) {

    //Check if SignalR is defined
    if (!$ || !$.connection) {
        return;
    }

    //Create namespaces
    geev.signalr = geev.signalr || {};
    geev.signalr.hubs = geev.signalr.hubs || {};

    //Get the common hub
    geev.signalr.hubs.common = $.connection.geevCommonHub;

    var commonHub = geev.signalr.hubs.common;
    if (!commonHub) {
        return;
    }

    //Register to get notifications
    commonHub.client.getNotification = function (notification) {
        geev.event.trigger('geev.notifications.received', notification);
    };

    //Connect to the server
    geev.signalr.connect = function() {
        $.connection.hub.start().done(function () {
            geev.log.debug('Connected to SignalR server!'); //TODO: Remove log
            geev.event.trigger('geev.signalr.connected');
            commonHub.server.register().done(function () {
                geev.log.debug('Registered to the SignalR server!'); //TODO: Remove log
            });
        });
    };

    if (geev.signalr.autoConnect === undefined) {
        geev.signalr.autoConnect = true;
    }

    if (geev.signalr.autoConnect) {
        geev.signalr.connect();
    }

})(jQuery);