﻿if (typeof History !== "undefined" && typeof History.Adapter !== "undefined" && typeof ko !== "undefined") {
    ko.observableQueryParameter = function (parameterName, defaultValue) {
        var self = this;
        var observable = null;

        this.getState = function () {
            var state = History.getState();
            var uri = Bifrost.Uri.create(state.url);
            if (uri.parameters.hasOwnProperty(parameterName)) {
                return uri.parameters[parameterName];
            }

            return null;
        }

        History.Adapter.bind(window, "statechange", function () {
            if (observable != null) {
                observable(self.getState());
            }
        });

        observable = ko.observable(self.getState() || defaultValue);

        observable.subscribe(function (newValue) {
            var state = History.getState();
            state[parameterName] = newValue;

            var parameters = Bifrost.hashString.decode(state.url);
            parameters[parameterName] = newValue;


            var url = "?";
            var parameterIndex = 0;
            for (var parameter in parameters) {
                if (parameterIndex > 0) {
                    url += "&";
                }
                url += parameter + "=" + parameters[parameter];
                parameterIndex++;
            }
            History.pushState(state, state.title, url);
        });

        return observable;
    }
}