(function () {
    'use strict';

    var controllerId = 'modalInstanceCtrl';

    angular.module('app').controller(controllerId, ['$scope', '$upload', '$modalInstance', 'gifservice', '$rootScope', 'username', modal]);

    function modal($scope, $upload, $modalInstance, gifservice, $rootScope, username) {
        cleanVariables();

        $scope.dropComplete = function(src, file) {
            $scope.image.source = src;
            $scope.image.file = file;
            showImage();
        };

        $scope.submit = function (isValid) {
            if (isValid && $scope.image.file) {
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
                    $modalInstance.close('close');
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
