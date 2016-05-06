
var app = angular.module('MyApp', []);

app.controller('jobListingCtrl', ['$scope', '$http', '$q', function ($scope, $http, $q) {

    $scope.init = function () {

        $scope.Stupid = "you are a loser";

        $scope.AddSaveLabel = "Add";
        $scope.EditCancelLabel = "Edit";
        $scope.disabled = true;
        $scope.ActivityAddSaveLabel = "Add";
        $scope.ActivityEditCancelLabel = "Edit";
        $scope.ActivityDisabled = true;
        $scope.tableDisabled = !$scope.disabled;
        $scope.showDetails = false;
        $scope.showAgentDetails = false;
        $scope.showAgencyDetails = false;
        $scope.showCompanyDetails = false;
        $scope.showActivities = false;
        GetJobListings();
    }

    GetJobListings = function () {
        return $http({
            url: 'api/WebApi/GetJobListings',
            method: 'GET'
        }).success(function (jobListingModel) {
            $scope.jobListings = jobListingModel.JobListings;
            $scope.ListingStatuses = jobListingModel.ListingStatuses;
            $scope.EmploymentTypes = jobListingModel.EmploymentTypes;
            $scope.ListingSources = jobListingModel.ListingSources;
            $scope.Desirabilities = jobListingModel.Desirabilities;
            $scope.Fits = jobListingModel.Fits;
        }).error(function (error) {
            $scope.Stupid = error.ExceptionMessage;
        })
    }

    $scope.LoadDetail = function (jobListing) {
        if ($scope.disabled) {
            $scope.selectedJobListing = angular.copy(jobListing);
            ClearActivity();
            GetJobSearchActivities();
        }
    }

    $scope.LoadActivityDetail = function (activity) {
        $scope.selectedActivity = angular.copy(activity);
    }

    function ClearListing() {
        $scope.selectedJobListing.Id = 0;
        $scope.selectedJobListing.PostedDate = GetToday();
        $scope.selectedJobListing.JobTitle = "";
        $scope.selectedJobListing.Comments = "";
        $scope.selectedJobListing.Rate = "";
        $scope.selectedJobListing.EmploymentType = "";
        $scope.selectedJobListing.ListingSource = "";
        $scope.selectedJobListing.Desirability = "";
        $scope.selectedJobListing.Fit = "";
        $scope.selectedJobListing.Distance = "";
        $scope.selectedJobListing.AgentName = "";
        $scope.selectedJobListing.AgentWorkPhone = "";
        $scope.selectedJobListing.AgentEmail = "";
        $scope.selectedJobListing.AgentCellPhone = "";
        $scope.selectedJobListing.AgencyName = "";
        $scope.selectedJobListing.AgencyEmail = "";
        $scope.selectedJobListing.AgencyWorkPhone = "";
        $scope.selectedJobListing.AgencyStreetAddress = "";
        $scope.selectedJobListing.AgencyCity = "";
        $scope.selectedJobListing.AgencyZip = "";
        $scope.selectedJobListing.TargetCompanyName = "";
        $scope.selectedJobListing.TargetCompanyAddress = "";
        $scope.selectedJobListing.TargetCompanyCity = "";
        $scope.selectedJobListing.TargetCompanyState = "";
        $scope.selectedJobListing.TargetCompanyZip = "";
    }

    GetJobSearchActivities = function () {
        $scope.Stupid = "trying";
        return $http({
            url: 'api/WebApi/GetJobSearchActivities',
            params: { selectedJobListingId: $scope.selectedJobListing.Id },
            method: 'GET'
        })
        .success(function (data) {
            $scope.jobSearchActivities = data.Activitites;
            $scope.activityTypes = data.ActivityTypes;
            $scope.Stupid = "ok";
        }).error(function (error) {
            $scope.Stupid = error.ExceptionMessage;
        })
    }

    function ClearActivity() {
        if ($scope.selectedActivity != null) {
            $scope.selectedActivity.ActivityDate = "";
            $scope.selectedActivity.ActivityStatus = '';
            $scope.selectedActivity.Comments = "";
        }
    }

    $scope.ActivityAddSave = function () {
        if ($scope.ActivityAddSaveLabel == "Add") {
            $scope.ActivityAddSaveLabel = "Save";
            $scope.ActivityEditCancelLabel = "Cancel";
            $scope.ActivityDisabled = false;
            ClearActivity();
            $scope.selectedActivity.ActivityId = 0;
            $scope.selectedActivity.ActivityDate = GetToday();
        }
        else {
            $scope.Stupid = "trying";
            return $http.post('api/WebApi/AddEditActivity', $scope.selectedActivity)
            .success(function (newId) {
                $scope.selectedActivity.ActivityId = newId;
                $scope.Stupid = "ok";
                $scope.ActivityAddSaveLabel = "Add";
                $scope.ActivityEditCancelLabel = "Edit";
                $scope.ActivityDisabled = true;
                GetJobSearchActivities();

            }).error(function (error) {
                $scope.Stupid = error.message;
            })
        }
    };
    $scope.ActivityEditCancel = function () {
        if ($scope.selectedActivity.ActivityDate != "") {
            if ($scope.ActivityEditCancelLabel == "Edit") {
                $scope.ActivityAddSaveLabel = "Save";
                $scope.ActivityEditCancelLabel = "Cancel";
                $scope.ActivityDisabled = false;
            }
            else {
                $scope.ActivityAddSaveLabel = "Add";
                $scope.ActivityEditCancelLabel = "Edit";
                $scope.ActivityDisabled = true;
            }
        }
        else
            $scope.Stupid = 'not';
    }


    $scope.AddSave = function () {
        if ($scope.AddSaveLabel == "Add") {
            $scope.AddSaveLabel = "Save";
            $scope.EditCancelLabel = "Cancel";
            $scope.disabled = false;
            ClearListing();
        }
        else {
            $scope.Stupid = "trying";
            return $http.post('api/WebApi/AddEditJobListing', $scope.selectedJobListing)
            .success(function (newId) {
                $scope.selectedJobListing.Id = newId;
                $scope.Stupid = "ok";
                $scope.AddSaveLabel = "Add";
                $scope.EditCancelLabel = "Edit";
                $scope.disabled = true;
                GetJobListings();

            }).error(function (error) {
                $scope.Stupid = error.message;
            })
        }
    }

    $scope.EditCancel = function () {
        if ($scope.selectedJobListing != undefined) {
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

    $scope.ShowJobDetails = function () {
        $scope.showAgentDetails = false;
        $scope.showAgencyDetails = false;
        $scope.showCompanyDetails = false;
        $scope.showActivities = false;
        $scope.showDetails = $scope.showDetails == false ? true : false;
    }
    $scope.ShowAgent = function () {
        $scope.showDetails = false;
        $scope.showAgencyDetails = false;
        $scope.showCompanyDetails = false;
        $scope.showActivities = false;
        $scope.showAgentDetails = $scope.showAgentDetails == false ? true : false;
    }
    $scope.ShowAgency = function () {
        $scope.showDetails = false;
        $scope.showAgentDetails = false;
        $scope.showCompanyDetails = false;
        $scope.showActivities = false;
        $scope.showAgencyDetails = $scope.showAgencyDetails == false ? true : false;
    }
    $scope.ShowTarget = function () {
        $scope.showDetails = false;
        $scope.showAgentDetails = false;
        $scope.showAgencyDetails = false;
        $scope.showActivities = false;
        $scope.showCompanyDetails = $scope.showCompanyDetails == false ? true : false;
    }
    $scope.ShowActivityLog = function () {
        $scope.showDetails = false;
        $scope.showAgentDetails = false;
        $scope.showAgencyDetails = false;
        $scope.showCompanyDetails = false;
        $scope.showActivities = $scope.showActivities == false ? true : false;
    }

}])