(function () {
    'use strict';
    var controllerId = 'event';
    angular.module('app').controller(controllerId,
        ['common', 'datacontext', '$location', '$routeParams', 'config', '$rootScope', event]);

    function event(common, datacontext, $location, $routeParams, config, $rootScope) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);
        var KeyCodes = config.KeyCodes;


        var vm = this;

        vm.eventCount = 0;
        vm.events = [];
        vm.title = 'Event';
        vm.gotoEvent = gotoEvent;

        vm.eventSearch = $routeParams.search || '';

        vm.eventCount = 0;

      

        activate();

        function activate() {
           // vm.user = $rootScope.user;
            var promises = [getEvents()];
            common.activateController(promises, controllerId)
                .then(function () {
                    log('Activated Event View');
                });
        }


        function getEvents() {
            return datacontext.event.getEvents()
                .then(function (data) {
                    vm.events = data.data;
                    vm.eventCount = data.data.length;
                    console.log(data.headers());

                    return vm.events;
                });
        }

        function gotoEvent(event) {
            if (event && event.id) {
                $location.path('event/' + event.id);
            }
        }
    }
})();