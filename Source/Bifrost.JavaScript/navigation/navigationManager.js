Bifrost.namespace("Bifrost.navigation", {
    navigateTo: function (featureName, queryString) {
        var url = featureName;

        if (featureName.charAt(0) !== "/")
            url = "/" + url;
        if (queryString)
            url += queryString;

        // TODO: Support title somehow
        if (typeof History !== "undefined" && typeof History.Adapter !== "undefined") {
            History.pushState({}, "", url);
        }
    },
    navigationManager: {
        hookup: function () {
            if (typeof History !== "undefined" && typeof History.Adapter !== "undefined") {
                $("body").click(function (e) {
                    var href = e.target.href;
                    if (typeof href == "undefined") {
                        var closestAnchor = $(e.target).closest("a")[0];
                        if (!closestAnchor) {
                            return;
                        }
                        href = closestAnchor.href;
                    }
                    if (href.indexOf("#") > 0) {
                        href = href.substr(0, href.indexOf("#"));
                    }

                    if (href.length == 0) {
                        href = "/";
                    }
                    var targetUri = Bifrost.Uri.create(href);
                    if (targetUri.isSameAsOrigin &&
                        targetUri.queryString.indexOf("postback")<0) {
                        var target = targetUri.path;
                        while (target.indexOf("/") == 0) {
                            target = target.substr(1);
                        }
                        e.preventDefault();
                        var queryString = targetUri.queryString.length > 0 ? "?" + targetUri.queryString : "";
                        History.pushState({}, "", "/" + target + queryString);
                    }
                });
            }
        }
    }
});