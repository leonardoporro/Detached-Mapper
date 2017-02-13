"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var core_1 = require("@angular/core");
var base_datasource_1 = require("./base.datasource");
(function (SortDirection) {
    SortDirection[SortDirection["Asc"] = 0] = "Asc";
    SortDirection[SortDirection["Desc"] = 1] = "Desc";
})(exports.SortDirection || (exports.SortDirection = {}));
var SortDirection = exports.SortDirection;
var DataPage = (function () {
    function DataPage() {
    }
    return DataPage;
}());
exports.DataPage = DataPage;
var HttpRestCollectionDataSource = (function (_super) {
    __extends(HttpRestCollectionDataSource, _super);
    function HttpRestCollectionDataSource(http, baseUrl) {
        _super.call(this, http);
        this.baseUrl = baseUrl;
        this.sortDirection = SortDirection.Asc;
        this.pageIndex = 1;
        this._items = [];
        this.itemsChange = new core_1.EventEmitter();
    }
    Object.defineProperty(HttpRestCollectionDataSource.prototype, "items", {
        get: function () {
            return this._items;
        },
        set: function (value) {
            if (value != this._items) {
                this._items = value;
                this.itemsChange.emit(this._items);
            }
        },
        enumerable: true,
        configurable: true
    });
    HttpRestCollectionDataSource.prototype.load = function () {
        var _this = this;
        var params = Object.assign({}, this.requestParams);
        params.searchText = this.searchText;
        params.sortBy = this.sortBy;
        params.sortDirection = this.sortDirection;
        params.pageIndex = this.pageIndex;
        params.pageSize = this.pageSize;
        params.noCount = this.noCount;
        var request = this.execute(this.baseUrl, this.requestParams, "get", null);
        request.subscribe(function (data) { return _this.items = data; }, function (error) { });
        return request;
    };
    HttpRestCollectionDataSource.prototype.toggleSort = function (propertyName) {
        if (this.sortBy != propertyName) {
            this.sortDirection = SortDirection.Asc;
        }
        else {
            switch (this.sortDirection) {
                case SortDirection.Desc:
                    this.sortDirection = SortDirection.Asc;
                    break;
                case SortDirection.Asc:
                    this.sortDirection = SortDirection.Desc;
                    break;
            }
        }
        this.sortBy = propertyName;
        return this.load();
    };
    return HttpRestCollectionDataSource;
}(base_datasource_1.HttpRestBaseDataSource));
exports.HttpRestCollectionDataSource = HttpRestCollectionDataSource;
//# sourceMappingURL=collection.datasource.js.map