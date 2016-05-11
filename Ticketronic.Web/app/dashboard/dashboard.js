(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', dashboard]);

    function dashboard(common) {
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(controllerId);

        var vm = this;
        vm.news = {
            title: 'Hot Towel Angular',
            description: 'Hot Towel Angular is a SPA template for Angular developers.'
        };
        vm.messageCount = 0;
      
        vm.title = 'Dashboard';


        activate();

        

        function activate() {
            var promises = [];
            common.activateController(promises, controllerId)
                .then(function () {
                    log('Activated DashBoard View');
                });
        }

 
    }
})();