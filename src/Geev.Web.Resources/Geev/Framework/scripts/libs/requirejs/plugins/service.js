define(function () {
    return {
        load: function (name, req, onload, config) {
            var url = geev.appPath + 'api/GeevServiceProxies/Get?name=' + name;
            req([url], function (value) {
                onload(value);
            });
        }
    };
});