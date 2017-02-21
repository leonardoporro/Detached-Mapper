"use strict";
var http_1 = require("@angular/http");
var ReplaySubject_1 = require("rxjs/ReplaySubject");
var HttpRestBaseDataSource = (function () {
    function HttpRestBaseDataSource(http) {
        this.http = http;
        this.keyProperty = "id";
        this.requestParams = {};
    }
    HttpRestBaseDataSource.prototype.execute = function (baseUrl, params, verb, data) {
        var _this = this;
        var result = new ReplaySubject_1.ReplaySubject();
        this.isBusy = true;
        var searchParams = new http_1.URLSearchParams();
        for (var paramProp in params) {
            var value = params[paramProp];
            if (value !== undefined) {
                searchParams.append(paramProp, value);
            }
        }
        var url = baseUrl.replace(/:[a-zA-z0-9]+/, function (match, args) {
            var paramName = match.substr(1);
            var value = searchParams.get(paramName);
            if (!value)
                throw new URIError("parameter " + match + " in url '" + this.url + "' is not defined.");
            searchParams.delete(paramName); //don't send duplicated parameters.
            return value;
        }.bind(this));
        var request;
        switch (verb) {
            case "get":
                request = this.http.get(url, { search: searchParams });
                break;
            case "post":
                request = this.http.post(url, data, { search: searchParams });
                break;
            case "put":
                request = this.http.put(url, data, { search: searchParams });
                break;
            case "delete":
                request = this.http.delete(url, { search: searchParams });
                break;
        }
        request.subscribe(function (data) { return result.next(_this.handleData(data)); }, function (error) { return result.error(_this.handleError(error)); }, function () {
            result.complete();
            _this.isBusy = false;
        });
        return result;
    };
    HttpRestBaseDataSource.prototype.handleData = function (data) {
        return data.json();
    };
    HttpRestBaseDataSource.prototype.handleError = function (error) {
        return error.json();
    };
    return HttpRestBaseDataSource;
}());
exports.HttpRestBaseDataSource = HttpRestBaseDataSource;
var ApiError = (function () {
    function ApiError() {
    }
    return ApiError;
}());
exports.ApiError = ApiError;
//# sourceMappingURL=base.datasource.js.map