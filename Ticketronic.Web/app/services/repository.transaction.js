(function () {
    'use strict';

    var serviceId = 'repository.transaction';
    angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryTransaction]);

    function RepositoryTransaction(AbstractRepository, $http) {
        var entityName = 'Transaction';

        function Ctor(endpoint) {
            this.serviceId = serviceId;
            this.entityName = entityName;
            this.hostEndPoint = endpoint;
            //Exposed data access functions
            this.getTransactions = getTransactions;
            this.getCount = getCount;
            this.getTransaction = getTransaction;
            this.saveTransaction = saveTransaction;
            this.deleteTransaction = deleteTransaction;
            this.updateTransaction = updateTransaction;
            this.getTransactionCount = getTransactionCount;
        }

        AbstractRepository.extend(Ctor);

        return Ctor;

        function getTransactions() {


            return $http.get(this.hostEndPoint + "/api/Transaction");
        }

        function getCount() {
            //return $http.get(this.hostEndPoint + "/api/Transaction");
        }

        function getTransaction(ComNo) {
            return $http.get(this.hostEndPoint + "/api/Transaction/" + ComNo);
        }

        function getTransactionCount() {

            return $http.get(this.hostEndPoint + "/api/Transaction/" + ComNo);
        }

        function saveTransaction(Transaction) {
            var headers = {
            };

            var request = $http({
                method: "post",
                headers: headers,
                url: this.hostEndPoint + "/api/Transaction",
                data: Transaction
            });
            return request
        }

        function deleteTransaction(ComNo) {
            var headers = {
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': '*',
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            };

            var request = $http({
                method: "delete",
                //headers:  headers,
                url: this.hostEndPoint + "/api/Transaction/" + ComNo
            });

            return request;
        }

        function updateTransaction(ComNo, Transaction) {
            var headers = {
            };

            var request = $http({
                method: "put",
                headers: headers,
                url: this.hostEndPoint + "/api/Transaction/" + ComNo,
                data: Transaction
            });
            return request;
        }


    }
})();




