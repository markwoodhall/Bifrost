
Bifrost.namespace("web.features.statistics", {
    index: Bifrost.Type.extend(function () {
        this.StatisticModel = {
            Statistics: [
                { name: "HadValidationError", value: 500, max: 10000, title: "Validation Errors", warning: true },
                { name: "HadException", value: 50, max: 10000, title: "Exceptions", warning: true },
                { name: "FailedSecurity", value: 523, max: 10000, title: "Failed Security", warning: true },
            ]
        };
    })
});


Bifrost.features.featureManager.get("Statistics/index").defineViewModel(web.features.statistics.index);

ko.bindingHandlers.statistic = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
        console.log(viewModel);
        var guage = new JustGage({
            id: viewModel.name,
            value: viewModel.value,
            min: 0,
            max: viewModel.max,
            title: viewModel.title,
            levelColorsGradient: false
        });
    }
}

