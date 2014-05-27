(function () {
    'use strict';

    var controllerId = 'modalInstanceCtrl';

    angular.module('app').controller(controllerId, ['$scope', '$upload', '$modalInstance', 'gifservice', '$rootScope', 'username', modal]);

    function modal($scope, $upload, $modalInstance, gifservice, $rootScope, username) {
        cleanVariables();

        $scope.dropComplete = function (src, file) {
            $scope.image.source = src;
            $scope.image.file = file;
            showImage();
        };

        $scope.submit = function (isValid) {
            if (isValid && $scope.image.file) {
                var image = $scope.image.source;

                $scope.upload = $upload.upload({
                    url: '/api/gif',
                    data: {
                        textOverlay: $scope.image.text,
                        hubId: $rootScope.hubid,
                        username: username
                    },
                    file: $scope.image.file,
                }).progress(function (evt) {
                    if (!$scope.loading && evt.loaded != evt.total) {
                        $scope.loading = true;
                    }
                }).success(function (data, status, headers, config) {
                    cleanVariables();
                    $modalInstance.close({
                        URL: image,
                        user: username,
                        text: "Processing",
                        date: new Date()
                    });
                }).error(function (err) {
                    console.log(err);
                    cleanVariables();
                    $modalInstance.dismiss('close');
                });
            } else {
                $scope.submitted = true;
                $scope.filemissing = !$scope.image.file;
            }
        };

        $scope.cancel = closeModal;

        $scope.init = function () {
            var element = $('#dropzone'),
                scope = $scope;
            
            //Refactor to a directive
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

                var file = event.originalEvent.dataTransfer.files[0]
                if (file && file.type == "image/gif") {
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
        };

        function closeModal() {
            cleanVariables();
            $modalInstance.dismiss('cancel');
        }

        function cleanVariables() {
            $scope.loading = false;
            $scope.fileloaded = false;

            $scope.image = {
                source: '',
                text: '',
                file: ''
            };
        }

        function showImage() {
            $scope.fileloaded = true;
            $scope.loading = false;
            $scope.filemissing = false;
            $scope.$apply();
        }
    }
})();
