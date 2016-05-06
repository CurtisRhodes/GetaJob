var app = angular.module('MyApp', []);

app.controller('adminCtrl', ['$scope', '$http', '$q', function ($scope, $http, $q) {

    $scope.init = function () {
        //$scope.Stupid = "you are a loser";

        $scope.AddSaveLabel = "Add";
        $scope.EditCancelLabel = "Edit";
        GetRefTypes()
        $scope.DisplayMode = false;
        $scope.EditMode = true;
        $scope.selectedRefType = [];
        $scope.SelectedRef = {};
    }

    GetRefTypes = function () {
        return $http({
            url: 'api/WebApi/GetRefs',
            params: { refType: 0 },
            method: 'GET'
        }).success(function (data) {
            $scope.refTypes = data;
            $scope.Stupid = 'reftypes loaded';
        }).error(function (error) {
            $scope.Stupid = error.ExceptionMessage;
        })
    }

    $scope.refTypeSelect = function () {
        return $http({
            url: 'api/WebApi/GetRefs',
            params: { refType: $scope.selectedRefType.RefCode },
            method: 'GET'
        }).success(function (data) {
            $scope.SelectedRefs = data;
            
        }).error(function (error) {
            $scope.Stupid = error.Message;
        })
    }

    $scope.LoadEdit = function (selectedRef) {
        if ($scope.EditMode)
            $scope.SelectedRef = selectedRef;
    }

    $scope.AddSave = function () {
        if ($scope.AddSaveLabel == "Add") {
            $scope.AddSaveLabel = "Save";
            $scope.EditCancelLabel = "Cancel";
            $scope.DisplayMode = true;
            $scope.EditMode = false;

            $scope.SelectedRef.RefCode = -1;
            $scope.SelectedRef.RefDescription = "";
            $scope.SelectedRef.RefType = $scope.selectedRefType.RefCode;
        }
        else {
            $scope.Stupid = "trying";
            return $http.post('api/WebApi/AddEditRef', $scope.SelectedRef)
            .success(function (newId) {
                $scope.Stupid = "ok";
                $scope.AddSaveLabel = "Add";
                $scope.EditCancelLabel = "Edit";
                $scope.refTypeSelect();
                $scope.SelectedRef = {};
                $scope.EditMode = true;
                $scope.DisplayMode = false;

            }).error(function (error) {
                $scope.Stupid = error.ExceptionMessage;
            })
        }
    }

    $scope.EditCancel = function () {
        if ($scope.SelectedRef.RefDescription != null) {
            if ($scope.EditCancelLabel == "Edit") {
                $scope.AddSaveLabel = "Save";
                $scope.EditCancelLabel = "Cancel";
                $scope.DisplayMode = true;
                $scope.EditMode = false;
            }
            else {
                $scope.AddSaveLabel = "Add";
                $scope.EditCancelLabel = "Edit";
                $scope.DisplayMode = false;
                $scope.EditMode = true;
            }
        }
        else
            $scope.Stupid = 'not';
    }

}]);