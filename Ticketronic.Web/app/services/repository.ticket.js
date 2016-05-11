(function () {
    'use strict';

    var serviceId = 'repository.ticket';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryTicket]);

    function RepositoryTicket(AbstractRepository, $http) {
        var entityName = 'Ticket';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getTickets = getTickets;
            this.getCount = getCount;
            this.getTicket = getTicket;
            this.saveTicket = saveTicket;
            this.deleteTicket = deleteTicket;
            this.updateTicket = updateTicket;
            this.getTicketCount = getTicketCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getTickets() {


            return $http.get(this.hostEndPoint + "/api/Ticket");
        }

        function getCount() {
            //return $http.get(this.hostEndPoint + "/api/Ticket");
        }

        function getTicket(ComNo) {
            return $http.get(this.hostEndPoint + "/api/Ticket/" + ComNo);
        }

        function getTicketCount() {

            return $http.get(this.hostEndPoint + "/api/Ticket/" + ComNo);
        }

        function saveTicket(Ticket) {
            var headers = {
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Ticket",
                data: Ticket
            });
            return request
        }

        function deleteTicket(ComNo) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Ticket/" + ComNo
            });

            return request;
        }

        function updateTicket(ComNo, Ticket) {
            var headers = {
            };

            var request = $http({
                method: "put",
                headers: headers,
                url: this.hostEndPoint + "/api/Ticket/" + ComNo,
                data: Ticket
            });
            return request;
        }


    }
})();




