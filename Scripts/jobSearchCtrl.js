
var app = angular.module('MyApp', []);

app.controller('jobSearchCtrl', ['$scope', '$http', '$q', function ($scope, $http, $q) {

    $scope.init = function () {

        $scope.Stupid = "you are a loser";
        $scope.AddSaveLabel = "Add";
        $scope.EditCancelLabel = "Edit";
        $scope.disabled = true;
        $scope.tableDisabled = !$scope.disabled;
        GetJobSearches();
    }

    GetJobSearches = function ()
    {

        //var deferred = $q.defer();
        return $http({
            url: 'api/WebApi/GetJobSearchs',
            method: 'GET'
        }).success(function (data) {
            $scope.jobSearches = data.jobSearches;
            //deferred.resolve(data);
        }).error(function (error) {
            deferred.reject(error);
        })
        //return deferred.promise;
    }

    $scope.LoadDetail = function (jobSearch) {
        if ($scope.disabled)
            $scope.selectedJobSearch = angular.copy(jobSearch);
    }

    $scope.AddSave = function () {
        if ($scope.AddSaveLabel == "Add") {
            $scope.AddSaveLabel = "Save";
            $scope.EditCancelLabel = "Cancel";
            $scope.disabled = false;

            $scope.selectedJobSearch.description = "";
            $scope.selectedJobSearch.start = "";
            $scope.selectedJobSearch.completed = "";
            $scope.selectedJobSearch.id = 0;
        }
        else {
            $scope.Stupid = "trying";
            return $http.post('api/WebApi/AddEditJobSearch', $scope.selectedJobSearch)
            .success(function (data) {
                $scope.Stupid = data;
                $scope.AddSaveLabel = "Add";
                $scope.EditCancelLabel = "Edit";
                $scope.disabled = true;
                GetJobSearches();
            }).error(function (error) {
                $scope.Stupid = error.message;
            })
        }
    }

    $scope.EditCancel = function () {
        if ($scope.selectedJobSearch != undefined) {
            if ($scope.EditCancelLabel == "Edit") {
                $scope.AddSaveLabel = "Save";
                $scope.EditCancelLabel = "Cancel";
                $scope.disabled = false;
            }
            else {
                $scope.AddSaveLabel = "Add";
                $scope.EditCancelLabel = "Edit";
                $scope.disabled = true;
            }
        }
        else
            $scope.Stupid = 'not';
    }

}])