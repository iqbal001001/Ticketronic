(function () { 
    'use strict';
    
    var controllerId = 'sidebar';
    angular.module('app').controller(controllerId,
        ['$route', 'config', 'routes', 'datacontext', '$rootScope', 'bootstrap.dialog', '$location', 'security', '$scope', sidebar]);

    function sidebar($route, config, routes, datacontext, $rootScope, bsDialog, $location, security, $scope) {
        var vm = this;
        var KeyCodes = config.KeyCodes;

        vm.security = security;
        vm.isCurrent = isCurrent;
        vm.routes = routes;

        activate();

        function activate() {
            getNavRoutes();
        }

        $scope.$watch("vm.security.user", function () {
            if (vm.security.user) {
                //
            }
        }, true);
        
        function getNavRoutes() {
            vm.navRoutes = routes.filter(function(r) {
                return r.config.settings && r.config.settings.nav;
            }).sort(function(r1, r2) {
                return r1.config.settings.nav - r2.config.settings.nav;
            });
        }

        
    

        function isCurrent(route) {
            if (!route.config.title || !$route.current || !$route.current.title) {
                return '';
            }
            var menuName = route.config.title;
            return $route.current.title.substr(0, menuName.length) === menuName ? 'current' : '';
        }

   
    };
})();
