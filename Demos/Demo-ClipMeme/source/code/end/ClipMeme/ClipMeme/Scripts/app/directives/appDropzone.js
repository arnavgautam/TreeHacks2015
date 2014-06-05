(function() {
    'use strict';

    // TODO: replace app with your module name
    angular.module('app').directive('appDropzone', ['$window', appDropzone]);
    
    function appDropzone ($window) {
        // Usage:
        // 
        // Creates:
        // 
        var directive = {
            link: link,
            restrict: 'A'
        };
        return directive;

        function link(scope, element, attrs) {
            element.on('dragover', function (event) {
                event.preventDefault();
                event.stopPropagation();
                event.originalEvent.dataTransfer.dropEffect = 'copy';
            });

            element.on('drop', function (event) {
                event.preventDefault();
                event.stopPropagation();
                removeErrorMessage();
                this.classList.remove('over');

                var file = event.originalEvent.dataTransfer.files[0];
                if (file && file.type === "image/gif") {
                    var reader = new FileReader();

                    reader.onloadend = function (evt) {
                        if (evt.target.readyState === FileReader.DONE) {
                            if (scope.dropComplete) {
                                scope.dropComplete(evt.target.result, file);
                            }
                        }
                    };

                    reader.readAsDataURL(file);
                } else {
                    addErrorMessage(file.type);
                }
            });

            element.on('dragenter', function () {
                this.classList.add('over');
            });

            element.on('dragleave', function () {
                this.classList.remove('over');
            });

            function addErrorMessage(error) {
                scope.typeerror = error;
                scope.filemissing = false;
                scope.$apply();
            }

            function removeErrorMessage() {
                if (scope.typeerror) {
                    scope.filemissing = false;
                    scope.typeerror = '';
                    scope.$apply();
                }
            }
        }
    }

})();