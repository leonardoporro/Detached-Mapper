"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var core_1 = require("@angular/core");
var base_datasource_1 = require("./base.datasource");
var HttpRestModelDataSource = (function (_super) {
    __extends(HttpRestModelDataSource, _super);
    function HttpRestModelDataSource(http, baseUrl) {
        _super.call(this, http);
        this.baseUrl = baseUrl;
        this.model = {};
        this.modelChange = new core_1.EventEmitter();
        this.modelErrors = {};
        this.modelErrorsChange = new core_1.EventEmitter();
        this.isNew = true;
        this.isNewChange = new core_1.EventEmitter();
        this.loadUrl = baseUrl;
        this.saveUrl = baseUrl.replace("/:" + this.keyProperty, "");
        this.deleteUrl = baseUrl;
    }
    HttpRestModelDataSource.prototype.load = function (key) {
        var _this = this;
        var params = Object.assign({}, this.requestParams);
        params[this.keyProperty] = key;
        var request = this.execute(this.loadUrl, params, "get", null);
        request.subscribe(function (data) {
            _this.model = data;
        }, function (error) { });
        return request;
    };
    HttpRestModelDataSource.prototype.save = function () {
        var _this = this;
        var request = this.execute(this.saveUrl, this.requestParams, "post", this.model);
        request.subscribe(function (data) {
            _this.model = data;
        }, function (error) { });
        return request;
    };
    HttpRestModelDataSource.prototype.delete = function () {
        return null;
    };
    return HttpRestModelDataSource;
}(base_datasource_1.HttpRestBaseDataSource));
exports.HttpRestModelDataSource = HttpRestModelDataSource;
//# sourceMappingURL=model.datasource.js.map