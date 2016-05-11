(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());
    
    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    // Define the routes 
    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: 'app/dashboard/dashboard.html',
                    title: 'dashboard',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            }, {
                url: '/admin',
                config: {
                    title: 'admin',
                    templateUrl: 'app/admin/admin.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-lock"></i> Admin'
                    }
                }
            }, {
                url: '/event',
                config: {
                    title: 'event',
                    templateUrl: 'app/event/event.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Event'
                    }
                }
            }, {
                url: '/event/:id',
                config: {
                    title: 'event',
                    templateUrl: 'app/event/eventdetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/events/search/:search',
                config: {
                    title: 'Events search',
                    templateUrl: 'app/event/event.html',
                    settings: {

                    }
                }
            }, {
                url: '/ticket',
                config: {
                    title: 'ticket',
                    templateUrl: 'app/ticket/ticket.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Ticket'
                    }
                }
            }, {
                url: '/ticket/:id',
                config: {
                    title: 'ticket',
                    templateUrl: 'app/ticket/ticketdetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/tickets/search/:search',
                config: {
                    title: 'tickets search',
                    templateUrl: 'app/ticket/ticket.html',
                    settings: {

                    }
                }
            }, {
                url: '/transaction',
                config: {
                    title: 'transaction',
                    templateUrl: 'app/transaction/transaction.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-lock"></i> Transaction'
                    }
                }
            }, {
                url: '/transaction/:id',
                config: {
                    title: 'transaction',
                    templateUrl: 'app/transaction/transactiondetail.html',
                    settings: {

                    }
                }
            }, {
                url: '/transactions/search/:search',
                config: {
                    title: 'transactions search',
                    templateUrl: 'app/transaction/transaction.html',
                    settings: {

                    }
                }
            }
            , {
                url: '/login',
                config: {
                    title: 'login',
                    templateUrl: 'app/security/login/login.html',
                    settings: {
                        nav: 0,
                        content: 'Login'
                    }
                }
            }, {
                url: '/join',
                config: {
                    title: 'join',
                    templateUrl: 'app/security/join/join.html',
                    settings: {
                        nav: 0,
                        content: 'Join'
                    }
                }
            }, {
                url: '/forgotpassword',
                config: {
                    title: 'forgot password',
                    templateUrl: 'app/security/forgotPassword/forgotPassword.html',
                    settings: {
                        nav: 0,
                        content: 'Forgot Password'
                    }
                }
            },
            {
                url: '/manage',
                config: {
                    title: 'manage',
                    templateUrl: 'app/security/manage/manage.html',
                    settings: {
                        nav: 0,
                        content: 'Manage'
                    }
                }
            }, {
                url: '/confirmemail',
                config: {
                    title: 'confirm email',
                    templateUrl: 'app/security/confirmEmail/confirmEmail.html',
                    settings: {
                        nav: 0,
                        content: 'Confirm Email'
                    }
                }
            }, {
                url: '/resetpassword',
                config: {
                    title: 'reset password',
                    templateUrl: 'app/security/resetPassword/resetPassword.html',
                    settings: {
                        nav: 0,
                        content: 'Reset Password'
                    }
                }
            }, {
                url: '/registerExternal',
                config: {
                    title: 'register external',
                    templateUrl: 'app/security/registerExternal/registerexternal.html',
                    settings: {
                        nav: 0,
                        content: 'Register External'
                    }
                }
            }

        ];
    }
})();