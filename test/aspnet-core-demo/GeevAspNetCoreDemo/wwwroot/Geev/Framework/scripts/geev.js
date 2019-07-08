﻿var geev = geev || {};
(function ($) {

    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).
    geev.appPath = geev.appPath || '/';

    geev.pageLoadTime = new Date();

    //Converts given path to absolute path using geev.appPath variable.
    geev.toAbsAppPath = function (path) {
        if (path.indexOf('/') == 0) {
            path = path.substring(1);
        }

        return geev.appPath + path;
    };

    /* LOCALIZATION ***********************************************/
    //Implements Localization API that simplifies usage of localization scripts generated by Geev.

    geev.localization = geev.localization || {};

    geev.localization.localize = function (key, sourceName) {
        sourceName = sourceName || geev.localization.defaultSourceName;

        var source = geev.localization.values[sourceName];

        if (!source) {
            geev.log.warn('Could not find localization source: ' + sourceName);
            return key;
        }

        var value = source[key];
        if (value == undefined) {
            return key;
        }

        var copiedArguments = Array.prototype.slice.call(arguments, 0);
        copiedArguments.splice(1, 1);
        copiedArguments[0] = value;

        return geev.utils.formatString.apply(this, copiedArguments);
    };

    geev.localization.getSource = function (sourceName) {
        return function (key) {
            var copiedArguments = Array.prototype.slice.call(arguments, 0);
            copiedArguments.splice(1, 0, sourceName);
            return geev.localization.localize.apply(this, copiedArguments);
        };
    };

    geev.localization.isCurrentCulture = function (name) {
        return geev.localization.currentCulture
            && geev.localization.currentCulture.name
            && geev.localization.currentCulture.name.indexOf(name) == 0;
    };

    geev.localization.defaultSourceName = undefined;
    geev.localization.geevWeb = geev.localization.getSource('GeevWeb');

    /* AUTHORIZATION **********************************************/
    //Implements Authorization API that simplifies usage of authorization scripts generated by Geev.

    geev.auth = geev.auth || {};

    geev.auth.allPermissions = geev.auth.allPermissions || {};

    geev.auth.grantedPermissions = geev.auth.grantedPermissions || {};

    //Deprecated. Use geev.auth.isGranted instead.
    geev.auth.hasPermission = function (permissionName) {
        return geev.auth.isGranted.apply(this, arguments);
    };

    //Deprecated. Use geev.auth.isAnyGranted instead.
    geev.auth.hasAnyOfPermissions = function () {
        return geev.auth.isAnyGranted.apply(this, arguments);
    };

    //Deprecated. Use geev.auth.areAllGranted instead.
    geev.auth.hasAllOfPermissions = function () {
        return geev.auth.areAllGranted.apply(this, arguments);
    };

    geev.auth.isGranted = function (permissionName) {
        return geev.auth.allPermissions[permissionName] != undefined && geev.auth.grantedPermissions[permissionName] != undefined;
    };

    geev.auth.isAnyGranted = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (geev.auth.isGranted(arguments[i])) {
                return true;
            }
        }

        return false;
    };

    geev.auth.areAllGranted = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (!geev.auth.isGranted(arguments[i])) {
                return false;
            }
        }

        return true;
    };

    /* FEATURE SYSTEM *********************************************/
    //Implements Features API that simplifies usage of feature scripts generated by Geev.

    geev.features = geev.features || {};

    geev.features.allFeatures = geev.features.allFeatures || {};

    geev.features.get = function (name) {
        return geev.features.allFeatures[name];
    }

    geev.features.getValue = function (name) {
        var feature = geev.features.get(name);
        if (feature == undefined) {
            return undefined;
        }

        return feature.value;
    }

    geev.features.isEnabled = function (name) {
        var value = geev.features.getValue(name);
        return value == 'true' || value == 'True';
    }

    /* SETTINGS **************************************************/
    //Implements Settings API that simplifies usage of setting scripts generated by Geev.

    geev.setting = geev.setting || {};

    geev.setting.values = geev.setting.values || {};

    geev.setting.get = function (name) {
        return geev.setting.values[name];
    };

    geev.setting.getBoolean = function (name) {
        var value = geev.setting.get(name);
        return value == 'true' || value == 'True';
    };

    geev.setting.getInt = function (name) {
        return parseInt(geev.setting.values[name]);
    };

    /* REALTIME NOTIFICATIONS ************************************/

    geev.notifications = geev.notifications || {};

    geev.notifications.severity = {
        INFO: 0,
        SUCCESS: 1,
        WARN: 2,
        ERROR: 3,
        FATAL: 4
    };

    geev.notifications.userNotificationState = {
        UNREAD: 0,
        READ: 1
    };

    geev.notifications.getUserNotificationStateAsString = function (userNotificationState) {
        switch (userNotificationState) {
            case geev.notifications.userNotificationState.READ:
                return 'READ';
            case geev.notifications.userNotificationState.UNREAD:
                return 'UNREAD';
            default:
                geev.log.warn('Unknown user notification state value: ' + userNotificationState)
                return '?';
        }
    };

    geev.notifications.getUiNotifyFuncBySeverity = function (severity) {
        switch (severity) {
            case geev.notifications.severity.SUCCESS:
                return geev.notify.success;
            case geev.notifications.severity.WARN:
                return geev.notify.warn;
            case geev.notifications.severity.ERROR:
                return geev.notify.error;
            case geev.notifications.severity.FATAL:
                return geev.notify.error;
            case geev.notifications.severity.INFO:
            default:
                return geev.notify.info;
        }
    };

    geev.notifications.messageFormatters = {};

    geev.notifications.messageFormatters['Geev.Notifications.MessageNotificationData'] = function (userNotification) {
        return userNotification.notification.data.message;
    };

    geev.notifications.messageFormatters['Geev.Notifications.LocalizableMessageNotificationData'] = function (userNotification) {
        var localizedMessage = geev.localization.localize(
            userNotification.notification.data.message.name,
            userNotification.notification.data.message.sourceName
        );

        if (userNotification.notification.data.properties) {
            if ($) {
                //Prefer to use jQuery if possible
                $.each(userNotification.notification.data.properties, function (key, value) {
                    localizedMessage = localizedMessage.replace('{' + key + '}', value);
                });
            } else {
                //alternative for $.each
                var properties = Object.keys(userNotification.notification.data.properties);
                for (var i = 0; i < properties.length; i++) {
                    localizedMessage = localizedMessage.replace('{' + properties[i] + '}', userNotification.notification.data.properties[properties[i]]);
                }
            }
        }

        return localizedMessage;
    };

    geev.notifications.getFormattedMessageFromUserNotification = function (userNotification) {
        var formatter = geev.notifications.messageFormatters[userNotification.notification.data.type];
        if (!formatter) {
            geev.log.warn('No message formatter defined for given data type: ' + userNotification.notification.data.type)
            return '?';
        }

        if (!geev.utils.isFunction(formatter)) {
            geev.log.warn('Message formatter should be a function! It is invalid for data type: ' + userNotification.notification.data.type)
            return '?';
        }

        return formatter(userNotification);
    }

    geev.notifications.showUiNotifyForUserNotification = function (userNotification, options) {
        var message = geev.notifications.getFormattedMessageFromUserNotification(userNotification);
        var uiNotifyFunc = geev.notifications.getUiNotifyFuncBySeverity(userNotification.notification.severity);
        uiNotifyFunc(message, undefined, options);
    }

    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log

    geev.log = geev.log || {};

    geev.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    geev.log.level = geev.log.levels.DEBUG;

    geev.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel != undefined && logLevel < geev.log.level) {
            return;
        }

        console.log(logObject);
    };

    geev.log.debug = function (logObject) {
        geev.log.log("DEBUG: ", geev.log.levels.DEBUG);
        geev.log.log(logObject, geev.log.levels.DEBUG);
    };

    geev.log.info = function (logObject) {
        geev.log.log("INFO: ", geev.log.levels.INFO);
        geev.log.log(logObject, geev.log.levels.INFO);
    };

    geev.log.warn = function (logObject) {
        geev.log.log("WARN: ", geev.log.levels.WARN);
        geev.log.log(logObject, geev.log.levels.WARN);
    };

    geev.log.error = function (logObject) {
        geev.log.log("ERROR: ", geev.log.levels.ERROR);
        geev.log.log(logObject, geev.log.levels.ERROR);
    };

    geev.log.fatal = function (logObject) {
        geev.log.log("FATAL: ", geev.log.levels.FATAL);
        geev.log.log(logObject, geev.log.levels.FATAL);
    };

    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it

    geev.notify = geev.notify || {};

    geev.notify.success = function (message, title, options) {
        geev.log.warn('geev.notify.success is not implemented!');
    };

    geev.notify.info = function (message, title, options) {
        geev.log.warn('geev.notify.info is not implemented!');
    };

    geev.notify.warn = function (message, title, options) {
        geev.log.warn('geev.notify.warn is not implemented!');
    };

    geev.notify.error = function (message, title, options) {
        geev.log.warn('geev.notify.error is not implemented!');
    };

    /* MESSAGE **************************************************/
    //Defines Message API, not implements it

    geev.message = geev.message || {};

    var showMessage = function (message, title) {
        alert((title || '') + ' ' + message);

        if (!$) {
            geev.log.warn('geev.message can not return promise since jQuery is not defined!');
            return null;
        }

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    geev.message.info = function (message, title) {
        geev.log.warn('geev.message.info is not implemented!');
        return showMessage(message, title);
    };

    geev.message.success = function (message, title) {
        geev.log.warn('geev.message.success is not implemented!');
        return showMessage(message, title);
    };

    geev.message.warn = function (message, title) {
        geev.log.warn('geev.message.warn is not implemented!');
        return showMessage(message, title);
    };

    geev.message.error = function (message, title) {
        geev.log.warn('geev.message.error is not implemented!');
        return showMessage(message, title);
    };

    geev.message.confirm = function (message, titleOrCallback, callback) {
        geev.log.warn('geev.message.confirm is not implemented!');

        if (titleOrCallback && !(typeof titleOrCallback == 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);

        if (!$) {
            geev.log.warn('geev.message can not return promise since jQuery is not defined!');
            return null;
        }

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    /* UI *******************************************************/

    geev.ui = geev.ui || {};

    /* UI BLOCK */
    //Defines UI Block API, not implements it

    geev.ui.block = function (elm) {
        geev.log.warn('geev.ui.block is not implemented!');
    };

    geev.ui.unblock = function (elm) {
        geev.log.warn('geev.ui.unblock is not implemented!');
    };

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    geev.ui.setBusy = function (elm, optionsOrPromise) {
        geev.log.warn('geev.ui.setBusy is not implemented!');
    };

    geev.ui.clearBusy = function (elm) {
        geev.log.warn('geev.ui.clearBusy is not implemented!');
    };

    /* SIMPLE EVENT BUS *****************************************/

    geev.event = (function () {

        var _callbacks = {};

        var on = function (eventName, callback) {
            if (!_callbacks[eventName]) {
                _callbacks[eventName] = [];
            }

            _callbacks[eventName].push(callback);
        };

        var off = function (eventName, callback) {
            var callbacks = _callbacks[eventName];
            if (!callbacks) {
                return;
            }

            var index = -1;
            for (var i = 0; i < callbacks.length; i++) {
                if (callbacks[i] === callback) {
                    index = i;
                    break;
                }
            }

            if (index < 0) {
                return;
            }

            _callbacks[eventName].splice(index, 1);
        };

        var trigger = function (eventName) {
            var callbacks = _callbacks[eventName];
            if (!callbacks || !callbacks.length) {
                return;
            }

            var args = Array.prototype.slice.call(arguments, 1);
            for (var i = 0; i < callbacks.length; i++) {
                callbacks[i].apply(this, args);
            }
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            on: on,
            off: off,
            trigger: trigger
        };
    })();


    /* UTILS ***************************************************/

    geev.utils = geev.utils || {};

    /* Creates a name namespace.
    *  Example:
    *  var taskService = geev.utils.createNamespace(geev, 'services.task');
    *  taskService will be equal to geev.services.task
    *  first argument (root) must be defined first
    ************************************************************/
    geev.utils.createNamespace = function (root, ns) {
        var parts = ns.split('.');
        for (var i = 0; i < parts.length; i++) {
            if (typeof root[parts[i]] == 'undefined') {
                root[parts[i]] = {};
            }

            root = root[parts[i]];
        }

        return root;
    };

    /* Find and replaces a string (search) to another string (replacement) in
    *  given string (str).
    *  Example:
    *  geev.utils.replaceAll('This is a test string', 'is', 'X') = 'ThX X a test string'
    ************************************************************/
    geev.utils.replaceAll = function (str, search, replacement) {
        var fix = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        return str.replace(new RegExp(fix, 'g'), replacement);
    };

    /* Formats a string just like string.format in C#.
    *  Example:
    *  geev.utils.formatString('Hello {0}','Tuana') = 'Hello Tuana'
    ************************************************************/
    geev.utils.formatString = function () {
        if (arguments.length < 1) {
            return null;
        }

        var str = arguments[0];

        for (var i = 1; i < arguments.length; i++) {
            var placeHolder = '{' + (i - 1) + '}';
            str = geev.utils.replaceAll(str, placeHolder, arguments[i]);
        }

        return str;
    };

    geev.utils.toPascalCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toUpperCase();
        }

        return str.charAt(0).toUpperCase() + str.substr(1);
    }

    geev.utils.toCamelCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toLowerCase();
        }

        return str.charAt(0).toLowerCase() + str.substr(1);
    }

    geev.utils.truncateString = function (str, maxLength) {
        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        return str.substr(0, maxLength);
    };

    geev.utils.truncateStringWithPostfix = function (str, maxLength, postfix) {
        postfix = postfix || '...';

        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        if (maxLength <= postfix.length) {
            return postfix.substr(0, maxLength);
        }

        return str.substr(0, maxLength - postfix.length) + postfix;
    };

    geev.utils.isFunction = function (obj) {
        if ($) {
            //Prefer to use jQuery if possible
            return $.isFunction(obj);
        }

        //alternative for $.isFunction
        return !!(obj && obj.constructor && obj.call && obj.apply);
    };

    /**
     * parameterInfos should be an array of { name, value } objects
     * where name is query string parameter name and value is it's value.
     * includeQuestionMark is true by default.
     */
    geev.utils.buildQueryString = function (parameterInfos, includeQuestionMark) {
        if (includeQuestionMark === undefined) {
            includeQuestionMark = true;
        }

        var qs = '';

        for (var i = 0; i < parameterInfos.length; ++i) {
            var parameterInfo = parameterInfos[i];
            if (parameterInfo.value === undefined) {
                continue;
            }

            if (!qs.length) {
                if (includeQuestionMark) {
                    qs = qs + '?';
                }
            } else {
                qs = qs + '&';
            }

            qs = qs + parameterInfo.name + '=' + encodeURIComponent(parameterInfo.value);
        }

        return qs;
    }

    /**
     * Sets a cookie value for given key.
     * @param {string} key
     * @param {string} value 
     * @param {Date} expireDate Optional expire date (default: 30 days).
     */
    geev.utils.setCookieValue = function (key, value, expireDate) {
        if (!expireDate) {
            expireDate = new Date();
            expireDate.setDate(expireDate.getDate() + 30);
        }

        document.cookie = encodeURIComponent(key) + '=' + encodeURIComponent(value) + "; expires=" + expireDate.toUTCString();
    };

    /**
     * Gets a cookie with given key.
     * @param {string} key
     * @returns {string} Cookie value
     */
    geev.utils.getCookieValue = function (key) {
        var equalities = document.cookie.split('; ');
        for (var i = 0; i < equalities.length; i++) {
            if (!equalities[i]) {
                continue;
            }

            var splitted = equalities[i].split('=');
            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }

        return null;
    };

    /* TIMING *****************************************/
    geev.timing = geev.timing || {};

    geev.timing.utcClockProvider = (function () {

        var toUtc = function (date) {
            return Date.UTC(
                date.getUTCFullYear()
                , date.getUTCMonth()
                , date.getUTCDate()
                , date.getUTCHours()
                , date.getUTCMinutes()
                , date.getUTCSeconds()
                , date.getUTCMilliseconds()
            );
        }

        var now = function () {
            return new Date();
        };

        var normalize = function (date) {
            if (!date) {
                return date;
            }

            return new Date(toUtc(date));
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            now: now,
            normalize: normalize
        };
    })();

    geev.timing.localClockProvider = (function () {

        var toLocal = function (date) {
            return new Date(
                date.getFullYear()
                , date.getMonth()
                , date.getDate()
                , date.getHours()
                , date.getMinutes()
                , date.getSeconds()
                , date.getMilliseconds()
            );
        }

        var now = function () {
            return toLocal(new Date());
        }

        var normalize = function (date) {
            if (!date) {
                return date;
            }

            return toLocal(date);
        }

        // Public interface ///////////////////////////////////////////////////

        return {
            now: now,
            normalize: normalize
        };
    })();

    geev.timing.unspecifiedClockProvider = (function () {

        var now = function () {
            return new Date();
        }

        var normalize = function (date) {
            return date;
        }

        // Public interface ///////////////////////////////////////////////////

        return {
            now: now,
            normalize: normalize
        };
    })();

    geev.timing.convertToUserTimezone = function (date) {
        var localTime = date.getTime();
        var utcTime = localTime + (date.getTimezoneOffset() * 60000);
        var targetTime = parseInt(utcTime) + parseInt(geev.timing.timeZoneInfo.windows.currentUtcOffsetInMilliseconds);
        return new Date(targetTime);
    };

    /* CLOCK *****************************************/
    geev.clock = geev.clock || {};

    geev.clock.now = function () {
        if (geev.clock.provider) {
            return geev.clock.provider.now();
        }

        return new Date();
    }

    geev.clock.normalize = function (date) {
        if (geev.clock.provider) {
            return geev.clock.provider.normalize(date);
        }

        return date;
    }

    geev.clock.provider = geev.timing.unspecifiedClockProvider;

    /* SECURITY ***************************************/
    geev.security = geev.security || {};
    geev.security.antiForgery = geev.security.antiForgery || {};

    geev.security.antiForgery.tokenCookieName = 'XSRF-TOKEN';
    geev.security.antiForgery.tokenHeaderName = 'X-XSRF-TOKEN';

    geev.security.antiForgery.getToken = function () {
        return geev.utils.getCookieValue(geev.security.antiForgery.tokenCookieName);
    };

})(jQuery);