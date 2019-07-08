var geev = geev || {};
(function () {
    if (!moment || !moment.tz) {
        return;
    }

    /* DEFAULTS *************************************************/

    geev.timing = geev.timing || {};

    /* FUNCTIONS **************************************************/

    geev.timing.convertToUserTimezone = function (date) {
        var momentDate = moment(date);
        var targetDate = momentDate.clone().tz(geev.timing.timeZoneInfo.iana.timeZoneId);
        return targetDate;
    };

})();