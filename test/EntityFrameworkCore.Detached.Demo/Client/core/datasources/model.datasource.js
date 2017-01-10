"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
var rxjs_1 = require("rxjs");
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
        this.modelErrors = {};
        this.modelErrorsChange = new core_1.EventEmitter();
        this.params = {};
        this.isNew = true;
        this.isNewChange = new core_1.EventEmitter();
        this.loadUrl = baseUrl;
        this.saveUrl = baseUrl.replace("/:" + this.keyProperty, "");
        this.deleteUrl = baseUrl;
    }
    HttpRestModelDataSource.prototype.buildParams = function () {
        var searchParams = new http_1.URLSearchParams();
        for (var paramProp in this.params) {
            searchParams.append(paramProp, this.params[paramProp]);
        }
        return searchParams;
    };
    HttpRestModelDataSource.prototype.load = function (key) {
        var _this = this;
        var result = new rxjs_1.ReplaySubject();
        this.params[this.keyProperty] = key;
        var searchParams = this.buildParams();
        var resolvedUrl = this.resolveUrl(this.loadUrl, searchParams);
        searchParams.delete(this.keyProperty);
        this.http.get(resolvedUrl, { search: searchParams })
            .subscribe(function (response) {
            _this.handleResponse(response);
            result.complete();
        }, function (error) {
            _this.handleError(error);
            result.error(error);
        });
        return result;
    };
    HttpRestModelDataSource.prototype.validate = function () {
        return true;
    };
    HttpRestModelDataSource.prototype.save = function () {
        var _this = this;
        var result = new rxjs_1.ReplaySubject();
        if (!this.validate) {
            result.error("invalid.");
        }
        else {
            var searchParams = this.buildParams();
            var resolvedUrl = this.resolveUrl(this.saveUrl, searchParams);
            searchParams.delete(this.keyProperty);
            this.http.post(resolvedUrl, this.model, { search: searchParams })
                .subscribe(function (response) {
                _this.handleResponse(response);
                result.next(_this.model);
            }, function (error) {
                _this.handleError(error);
                result.error(error);
            }, function () { return result.complete(); });
        }
        return result;
    };
    HttpRestModelDataSource.prototype.delete = function () {
        var _this = this;
        var result = new rxjs_1.ReplaySubject();
        var searchParams = this.buildParams();
        var resolvedUrl = this.resolveUrl(this.deleteUrl, searchParams);
        searchParams.delete(this.keyProperty);
        this.http.delete(resolvedUrl, { search: searchParams })
            .subscribe(function (response) {
            result.next(null);
        }, function (error) {
            _this.handleError(error);
            result.error(error);
        }, function () { return result.complete(); });
        return result;
    };
    HttpRestModelDataSource.prototype.handleResponse = function (response) {
        this.model = response.json();
        this.modelChange.emit(this.model);
        this.isNew = false;
        this.isNewChange.emit(this.isNew);
    };
    HttpRestModelDataSource.prototype.addModelError = function (property, code) {
        this.modelErrors[property] = code;
    };
    return HttpRestModelDataSource;
}(base_datasource_1.HttpRestBaseDataSource));
exports.HttpRestModelDataSource = HttpRestModelDataSource;
//# sourceMappingURL=model.datasource.js.map