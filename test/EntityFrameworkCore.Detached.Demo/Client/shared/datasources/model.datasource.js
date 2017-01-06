"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var Observable_1 = require("rxjs/Observable");
var base_datasource_1 = require("./base.datasource");
var HttpRestModelDataSource = (function (_super) {
    __extends(HttpRestModelDataSource, _super);
    function HttpRestModelDataSource(http, baseUrl) {
        _super.call(this);
        this.http = http;
        this.baseUrl = baseUrl;
        this.keyProperty = "id";
        this.model = {};
        this.modelChange = new core_1.EventEmitter();
        this.params = {};
        this.loadUrl = baseUrl;
        this.saveUrl = baseUrl.replace("/:" + this.keyProperty, "");
        this.deleteUrl = baseUrl;
    }
    Object.defineProperty(HttpRestModelDataSource.prototype, "isNew", {
        get: function () {
            return this.model && this.model[this.keyProperty];
        },
        enumerable: true,
        configurable: true
    });
    HttpRestModelDataSource.prototype.buildParams = function () {
        var searchParams = new http_1.URLSearchParams();
        for (var paramProp in this.params) {
            searchParams.append(paramProp, this.params[paramProp]);
        }
        return searchParams;
    };
    HttpRestModelDataSource.prototype.load = function (key) {
        this.params[this.keyProperty] = key;
        var searchParams = this.buildParams();
        var resolvedUrl = this.resolveUrl(this.loadUrl, searchParams);
        searchParams.delete(this.keyProperty);
        var result = this.http.get(resolvedUrl, { search: searchParams })
            .map(function (r) { return r.json(); });
        result.subscribe(this.handleResponse.bind(this), this.handleError.bind(this));
        return result;
    };
    HttpRestModelDataSource.prototype.validate = function () {
        return true;
    };
    HttpRestModelDataSource.prototype.save = function () {
        if (!this.validate) {
            return Observable_1.Observable.throw("invalid.");
        }
        else {
            var searchParams = this.buildParams();
            var resolvedUrl = this.resolveUrl(this.saveUrl, searchParams);
            searchParams.delete(this.keyProperty);
            var result = this.http.post(resolvedUrl, this.model, { search: searchParams })
                .map(function (r) { return r.json(); });
            result.subscribe(this.handleResponse.bind(this), this.handleError.bind(this));
            return result;
        }
    };
    HttpRestModelDataSource.prototype.delete = function () {
        var searchParams = this.buildParams();
        var resolvedUrl = this.resolveUrl(this.deleteUrl, searchParams);
        searchParams.delete(this.keyProperty);
        var result = this.http.delete(resolvedUrl, { search: searchParams });
        result.subscribe(this.handleResponse.bind(this), this.handleError.bind(this));
        return result;
    };
    HttpRestModelDataSource.prototype.handleResponse = function (response) {
        this.model = response;
        this.modelChange.emit(this.model);
    };
    HttpRestModelDataSource.prototype.handleError = function (error) {
    };
    return HttpRestModelDataSource;
}(base_datasource_1.HttpRestBaseDataSource));
exports.HttpRestModelDataSource = HttpRestModelDataSource;
//# sourceMappingURL=model.datasource.js.map