(function () {
    'use strict';

    var serviceId = 'gifservice';

    angular.module('app').factory(serviceId, ['$resource', gifservice]);

    function gifservice($resource) {
        return {
            "data": $resource('/api/gif/:id', { id: '@id' })
        };
    }
})();