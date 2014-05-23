(function () {
    'use strict';

    angular.module('app').directive('appImagefreeze', ['$window', appImagefreeze]);

    function appImagefreeze($window) {

        var directive = {
            template: '<div class="outsideWrapper">' +
                            '<div class="insideWrapper">' +
                                '<img />' +
                                '<canvas>' +
                            '</div>' +
                        '</div>',
            link: link,
            restrict: 'A',
            transclude: true,
            replace: true,
            scope: {
                source: '='
            }
        };

        return directive;

        function link(scope, element, attrs) {
            var canvas = element.find('canvas')[0];
            var img = element.find('img')[0];

            var renderCanvas = img.onload = function () {
                img.style.display = "block";

                var width = canvas.width = img.width;
                var height = canvas.height = img.height;

                element.width('100%');
                element.height(img.height);

                setTimeout(function () {
                    try {
                        // workaround for multiple canvas in chrome - http://stackoverflow.com/questions/12114283/how-to-get-multiple-canvas-tags-to-render-reliably-in-chrome
                        $(canvas).addClass("canvas_" + ((new Date() * 10000) + 621355968000000000));

                        canvas.getContext('2d').drawImage(img, 0, 0, width, height);
                        img.style.display = "";
                    } catch(e) {
                        img.src = "Content/Images/error.png";
                    }
                }, 0);

                /*Fixed Styles after drawing the image*/
                canvas.style.width = '100%';
                img.style.width = '100%';
                element.height(Math.floor(img.offsetWidth * height / width) -1);
            };

            scope.$watch('source', function () {
                if (scope.source) {
                    img.src = scope.source;
                    //Hack to trigger load event when the image is cached
                    renderCanvas();
                }
            });
        }
    }
})();