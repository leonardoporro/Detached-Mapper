"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var http_1 = require("@angular/http");
var error_1 = require("./error");
var Service = (function () {
    function Service(http) {
        this.http = http;
    }
    Service.prototype.request = function (baseUrl, params, method, data) {
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
            if (!value) {
                throw new URIError("parameter " + match + " in url '" + url + "' is not defined.");
            }
            searchParams.delete(paramName); // don't send duplicated parameters.
            return value;
        });
        var headers = new http_1.Headers();
        headers.append("Content-Type", "application/json");
        return this.http.request(url, {
            search: searchParams,
            method: method,
            headers: headers,
            body: data
        }).catch(this._mapException.bind(this))
            .map(this._mapResult.bind(this));
    };
    Service.prototype._mapException = function (response, observable) {
        var errorValue;
        if (response.type == http_1.ResponseType.Default) {
            errorValue = response.json();
        }
        else {
            errorValue = new error_1.ApiError();
            errorValue.code = error_1.ApiErrorCodes.NoInternetConnection;
            errorValue.message = "No internet connection";
        }
        throw errorValue;
    };
    Service.prototype._mapResult = function (response) {
        return response.json();
    };
    return Service;
}());
exports.Service = Service;
var EntityService = (function (_super) {
    __extends(EntityService, _super);
    function EntityService(http, baseUrl) {
        var _this = _super.call(this, http) || this;
        _this._queryUrl = baseUrl;
        _this._pagedQueryUrl = baseUrl + "/pages/:pageIndex";
        _this._postUrl = baseUrl;
        _this._getUrl = baseUrl + "/:key";
        _this._deleteUrl = baseUrl + "/:key";
        return _this;
    }
    EntityService.prototype.query = function (params) {
        if (params.pageSize) {
            return _super.prototype.request.call(this, this._pagedQueryUrl, params, http_1.RequestMethod.Get, null);
        }
        else {
            return _super.prototype.request.call(this, this._queryUrl, params, http_1.RequestMethod.Get, null)
                .map(function (data) {
                return {
                    items: data,
                    pageCount: 1,
                    rowCount: data.length
                };
            });
        }
    };
    EntityService.prototype.get = function (key) {
        return _super.prototype.request.call(this, this._getUrl, { key: key }, http_1.RequestMethod.Get, null);
    };
    EntityService.prototype.post = function (entity) {
        return _super.prototype.request.call(this, this._postUrl, {}, http_1.RequestMethod.Post, entity);
    };
    EntityService.prototype.delete = function (key) {
        return _super.prototype.request.call(this, this._deleteUrl, { key: key }, http_1.RequestMethod.Delete, null);
    };
    return EntityService;
}(Service));
exports.EntityService = EntityService;
var IServiceError = (function () {
    function IServiceError() {
    }
    return IServiceError;
}());
exports.IServiceError = IServiceError;
var SortOrder;
(function (SortOrder) {
    SortOrder[SortOrder["Asc"] = 1] = "Asc";
    SortOrder[SortOrder["Desc"] = 2] = "Desc";
})(SortOrder = exports.SortOrder || (exports.SortOrder = {}));
var QueryParams = (function () {
    function QueryParams() {
    }
    return QueryParams;
}());
exports.QueryParams = QueryParams;
//# sourceMappingURL=service.js.map