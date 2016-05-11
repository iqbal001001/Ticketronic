(function () {
	'use strict';

	var serviceId = 'repository.event';
	angular.module('app').factory(serviceId,
        ['repository.abstract', '$http', RepositoryEvent]);

	function RepositoryEvent(AbstractRepository, $http) {
		var entityName = 'Event';

		function Ctor(endpoint) {
			this.serviceId = serviceId;
			this.entityName = entityName;
			this.hostEndPoint = endpoint;
			//Exposed data access functions
			this.getEvents = getEvents;
			this.getCount = getCount;
			this.getEvent = getEvent;
			this.saveEvent = saveEvent;
			this.deleteEvent = deleteEvent;
			this.updateEvent = updateEvent;
			this.getEventCount = getEventCount;
		}

		AbstractRepository.extend(Ctor);

		return Ctor;

		function getEvents() {
			

			return $http.get(this.hostEndPoint + "/api/Event");
		}

		function getCount() {
			//return $http.get(this.hostEndPoint + "/api/Event");
		}

		function getEvent(ComNo) {
			return $http.get(this.hostEndPoint + "/api/Event/" + ComNo);
		}

		function getEventCount() {

			return $http.get(this.hostEndPoint + "/api/Event/" + ComNo);
		}

		function saveEvent(Event) {
			var headers = {
			};

			var request = $http({
				method: "post",
				headers: headers,
				url: this.hostEndPoint + "/api/Event",
				data: Event
			});
			return request
		}

		function deleteEvent(ComNo) {
			var headers = {
				'Access-Control-Allow-Origin': '*',
				'Access-Control-Allow-Methods': '*',
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			};

			var request = $http({
				method: "delete",
				//headers:  headers,
				url: this.hostEndPoint + "/api/Event/" + ComNo
			});

			return request;
		}

		function updateEvent(ComNo, Event) {
			var headers = {
			};

			var request = $http({
				method: "put",
				headers: headers,
				url: this.hostEndPoint + "/api/Event/" + ComNo,
				data: Event
			});
			return request;
		}


	}
})();




