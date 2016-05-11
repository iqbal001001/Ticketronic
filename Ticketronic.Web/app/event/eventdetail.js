(function () {
    'use strict';

    var controllerId = 'eventdetail';
    angular.module('app').
        controller(controllerId,
        ['$routeParams', '$location', '$scope', '$rootScope', '$window', '$filter',
            'bootstrap.dialog', 'common', 'config', 'datacontext',
            eventdetail]);

    function eventdetail($routeParams, $location, $scope, $rootScope, $window, $filter,
        bsDialog, common, config, datacontext) {
        var vm = this;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var logError = common.logger.getLogFn(controllerId, 'error');
        var KeyCodes = config.KeyCodes;

        vm.detailTitle = 'Session Detail'
        vm.activate = activate;
        vm.eventIdParameter = $routeParams.id;
        
        vm.cancel = cancel;
        vm.goBack = goBack;
        vm.save = save;
        vm.gotoSession = gotoSession;
        vm.addSession = addSession;
        vm.removeSession = removeSession;
        vm.newSession = false;
        vm.newevent;
        vm.deleteEvent = deleteEvent;
        vm.hasChanges = false;
        vm.isSaving = false;

        vm.openedSessionDate = false;
        vm.btnSessionDate = btnSessionDate;

        Object.defineProperty(vm, 'canSave', {
            get: canSave
        });

        function canSave() { return vm.hasChanges && !vm.isSaving; }

        activate();

        function activate() {
            common.activateController([getRequestedEvent()], controllerId)
                                .then(function () {
                                    log('Activated Eventdetail View');
                                });

        }

        function btnSessionDate($event) {
            $event.preventDefault();
            $event.stopPropagation();
            vm.openedSessionDate = true;
        };

        function deleteEvent() {
            return bsDialog.deleteDialog('Event').
                then(confirmDelete);

            function confirmDelete() {
                datacontext.event.deleteEvent(vm.event.id)
                .then(success, failed);

                function success() {
                    gotoEvents();
                }

                function failed(error) { cancel(); }
            }
        }

        function getRequestedEvent() {
            var val = $routeParams.id;
            if (val === 'new') {

                vm.event = {};
                //vm.event.user = vm.companyId;
                vm.sessions = [];
                vm.event.sessions = vm.sessions;
                return vm.newevent = true;
            }

            return datacontext.event.getEvent(val)
            .then(function (data) {
                vm.event = angular.copy(data.data);
                vm.originalEvent = angular.copy(data.data);
                bindData(data.data);
                vm.newevent = false;
            }, function (error) {
                logError('Unable to get event ' + val);
                gotoEvents();
            });
        }

        function bindData(data) {
            if (!data.sessions) data.sessions = [];
            vm.sessions = data.sessions;
        }

        function gotoSession(session) {
            if (session) {   //&& session.id
                vm.newSession = false;
                var found = $filter('filter')(vm.sessions, { id: session.id }, true);
                if (found.length) {
                    vm.session = found[0];
                } else {
                    vm.session = 'Not found';
                }
            }
        }

        function addSession() {

            if (vm.newSession == false) {

                var newsession =
                    {
                        "id": 0,
                        "name": '',
                        "date": '',
                        "starttime": '',
                        "duration": 0
                    }
                vm.newSession = true;
                vm.session = newsession;
            } else {

                vm.sessions.push(vm.session);
                vm.newSession = false;
            }
        }

        function removeSession(idx) {
            if (vm.sessions && vm.sessions.length > 0) {
                vm.sessions.splice(idx, 1);
            }
        };

        function sessionsFilter(session) {
            var isMatch = vm.sessionSearch
            ? common.textContains(session.description, vm.eventItemSearch)
           // || common.textContains(employee.fullName, vm.employeeSearch)
                : true;
            return isMatch;
        }

        function gotoEvents() {
            $location.path('/events');
        }

        function goBack() { $window.history.back(); }

        function cancel() {
            vm.event = angular.copy(vm.originalEvent);
        }

        function save() {
            //SaveEvent();
            vm.isSaving = true;
            if (vm.newevent === true) {
                return datacontext.event.saveEvent(vm.event)
                    .then(function (saveResult) {
                        vm.event.id = saveResult.data.id;
                        removeFromStore();
                        vm.originalEvent = angular.copy(vm.event);
                        vm.newevent = false;
                        vm.hasChanges = false;
                    }, function (error) {

                    }).finally(function () {
                        vm.isSaving = false;
                    })
            }
            else {
                return datacontext.event.updateEvent(vm.event.id, vm.event)
                           .then(function (saveResult) {
                               removeFromStore();
                               vm.originalEvent = angular.copy(vm.event);
                               vm.hasChanges = false;
                           }, function (error) {

                           }).finally(function () {
                               vm.isSaving = false;
                           })
            }

        }

        function SaveEvent() {

        }

        $scope.$watch('vm.event', function (newValue, oldValue) {
            if (newValue != oldValue) {
                vm.hasChanges = !angular.equals(vm.event, vm.originalEvent);
            }
        }, true);

    }
})();