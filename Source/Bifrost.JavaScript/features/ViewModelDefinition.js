Bifrost.namespace("Bifrost.features");
Bifrost.features.ViewModelDefinition = (function () {
    function ViewModelDefinition(target, options) {
        var self = this;
        this.target = target;
        this.options = {
            isSingleton: false
        }
        Bifrost.extend(this.options, options);

        this.getInstance = function () {
            var instance = null;
            if (self.options.isSingleton) {
                if (!self.instance) {
                    if (typeof self.target.create === "function") {
                        var promise = Bifrost.execution.Promise.create();
                        self.target.beginCreate().continueWith(function (instance) {
                            self.instance = instance;
                            promise.signal(instance);
                        });
                        instance = promise;
                    } else {
                        self.instance = new self.target();
                        instance = self.instance;
                    }
                } else {
                    instance = self.instance;
                }

                
            } else {
                if (typeof self.target.create === "function") {
                    var promise = Bifrost.execution.Promise.create();
                    self.target.beginCreate().continueWith(function (instance) {
                        self.instance = instance;
                        promise.signal(instance);
                    });
                    instance = promise;
                } else {
                    self.instance = new self.target();
                    instance = self.instance;
                }
                
            }
            if (typeof instance.onActivated == "function") {
                instance.onActivated();
            }
            return instance;
        };
    }

    return {
        define: function (target, options) {
            Bifrost.features.ViewModel.baseFor(target);
            var viewModel = new ViewModelDefinition(target, options);
            return viewModel;
        }
    }
})();