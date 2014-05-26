(function () {
    'use strict';

    var serviceId = 'signalrservice';

    angular.module('app').factory(serviceId, ['$rootScope', signalrservice]);

    function signalrservice($rootScope) {
        var hubName = "GifServerHub";

        var connection = $.hubConnection();
        var proxy = connection.createHubProxy(hubName);
        connection.error(function (error) {
            console.log('SignalR error: ' + error);
        });

        return {
            on: function (eventName, callback) {
                proxy.on(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            off: function (eventName, callback) {
                proxy.off(eventName, function (result) {
                    $rootScope.$apply(function () {
                        if (callback) {
                            callback(result);
                        }
                    });
                });
            },
            invoke: function (methodName, callback) {
                proxy.invoke(methodName)
                    .done(function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
            },
            init: function() {
                return connection.start();
            },
            connection: connection
        };
    }
})();