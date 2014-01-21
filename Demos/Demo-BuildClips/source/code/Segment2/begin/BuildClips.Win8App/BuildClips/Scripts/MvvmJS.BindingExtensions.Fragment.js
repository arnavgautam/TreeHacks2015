/*
    Code extracted from http://mvvmjs.codeplex.com/ (MvvmJS project)
*/

(function () {
    "use strict";

    WinJS.Namespace.define("MvvmJS.Binding", {
        urlStyle: function (source, sourceProperty, dest, destProperty) {
            var dummy = {};

            Object.defineProperties(dummy, {
                style: {
                    enumerable: true,
                    get: function () {
                        return dest[destProperty];
                    },
                    set: function (value) {
                        dest.style[destProperty[0]] = "url(" + value + ")";
                    }
                }
            });

            return window.WinJS.Binding.defaultBind(source, sourceProperty, dummy, ["style"]);
        }
    });

    var markSupportedForProcessing = WinJS.Utilities.markSupportedForProcessing;
    Object.keys(MvvmJS.Binding).forEach(function (key) {
        markSupportedForProcessing(MvvmJS.Binding[key]);
    });
}());