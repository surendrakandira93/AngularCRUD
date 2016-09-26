var app = angular.module("mvcCRUDApp", []);

app.service("crudAJService", function ($http) {

    this.GetProducts = function () {
        return $http.get("/api/Product/All");
    }

    this.GetProduct = function (productID) {
        return $http.get("/api/Product/ProductById/" + productID);
    }

    this.AddUpdateProduct = function (product) {
        console.log(product);
        var response = $http({
            method: "Post",
            url: "/api/Product/Add",
            data: product,
            dataType: "json"
        });
        return response;
    }
    
})

app.controller("mvcCRUDCtrl", function ($scope, crudAJService) {
    $scope.divProduct = false;
    GetAppProducts();
    function GetAppProducts() {
        var allProducts = crudAJService.GetProducts();
        allProducts.then(function (product) {
            $scope.Products = product.data;
        }, function () {
            alert("Error");
        });
       
    }

    $scope.EditProduct = function (productId) {
        var getProductData = crudAJService.GetProduct(productId);        
        getProductData.then(function (product) {
            $scope.Id = product.data.Id;
            $scope.CategoryId = product.data.CategoryId;
            $scope.subCategoryId = product.data.SubCategoryId;
            $scope.Name = product.data.Name;
            $scope.Price = product.data.Price;
            $scope.Image = product.data.Image;
            $scope.IsActive = product.data.IsActive;
            $scope.Action = "Update";
            $scope.divProduct = true;
        }, function () {
            alert('Error in getting product records');
        });
    }

    $scope.AddUpdateProduct = function (ngForm) {
        if (ngForm.$invalid) {       
            $scope.invalidSubmitAttempt = true;
            return;
        }
        var file = document.getElementById('inputFile');
        var product = {
            CategoryId: $scope.CategoryId,
            subCategoryId: $scope.subCategoryId,
            Name: $scope.Name,
            Price: $scope.Price,
            Image: $scope.Image,
            IsActive: $scope.IsActive
        };
        var getAction = $scope.Action;
        if (getAction == "Update") {
            product.Id = $scope.Id;
        }

        var formData = new FormData();
        formData.append('fileObj', file.files[0]);
        formData.append('request', JSON.stringify(product));
        var xhr = new XMLHttpRequest();
        xhr.onabort = $scope.HideOverlay;
        xhr.onerror = $scope.HideOverlay;
        xhr.ontimeout = $scope.HideOverlay;
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                alert(xhr.responseText);
                var data = JSON.parse(xhr.responseText);
                if (data !== undefined && data.Result.Status === 0) {                    
                    
                }
                else {
                    alert(data.Result.Error[0]);                }
            }
        };
        xhr.open("Post", "/api/Product/Add", true);
        xhr.send(formData);       

        //var getProductData = crudAJService.AddUpdateProduct(product);
        //getProductData.then(function (msg) {
        //    GetAppProducts();
        //    alert(msg.data);
        //    $scope.divProduct = false;
        //}, function () {
        //    alert('Error in updating book record');
        //});

    }

    $scope.AddProductDiv = function () {
        ClearFields();
        $scope.Action = "Add";
        $scope.divProduct = true;
    }

    function ClearFields() {
        $scope.Id = 0;
        $scope.CategoryId = 0;
        $scope.subCategoryId = 0;
        $scope.Name = "";
        $scope.Price = 0;
        $scope.Image = "";
        $scope.IsActive = false;
    }
    
    $scope.Cancel = function () {
        $scope.divProduct = false;
    };

})