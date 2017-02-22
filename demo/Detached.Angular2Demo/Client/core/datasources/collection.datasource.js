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
        this.pageCountChange = new core_1.EventEmitter();
        this._pageIndex = 1;
        this.pageIndexChange = new core_1.EventEmitter();
        this._items = [];
        this.itemsChange = new core_1.EventEmitter();
        this.pageBaseUrl = baseUrl + "/pages/:pageIndex";
    }
    Object.defineProperty(HttpRestCollectionDataSource.prototype, "pageCount", {
        get: function () { return this._pageCount; },
        set: function (value) {
            if (value !== this._pageCount) {
                this._pageCount = value;
                this.pageCountChange.emit(this._pageCount);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HttpRestCollectionDataSource.prototype, "pageIndex", {
        get: function () { return this._pageIndex; },
        set: function (value) {
            if (value !== this._pageIndex) {
                this._pageIndex = value;
                this.pageIndexChange.emit(this._pageIndex);
            }
        },
        enumerable: true,
        configurable: true
    });
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
        params.orderBy = this.getSortQueryParam();
        if (this.pageSize) {
            params.pageIndex = this.pageIndex;
            params.pageSize = this.pageSize;
            params.noCount = this.noCount;
            var request = this.execute(this.pageBaseUrl, params, "get", null);
            request.subscribe(function (data) {
                _this.items = data.items;
                _this.pageCount = data.pageCount;
            }, function (error) {
            });
            return request;
        }
        else {
            var request = this.execute(this.baseUrl, params, "get", null);
            request.subscribe(function (data) {
                _this.items = data;
            }, function (error) {
            });
            return request;
        }
    };
    HttpRestCollectionDataSource.prototype.getSortQueryParam = function () {
        if (this.orderBy) {
            if (this.sortDirection == 1)
                return this.orderBy + "+desc";
            else
                return this.orderBy + "+asc";
        }
    };
    HttpRestCollectionDataSource.prototype.toggleSort = function (propertyName) {
        if (this.orderBy != propertyName) {
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
        this.orderBy = propertyName;
        return this.load();
    };
    return HttpRestCollectionDataSource;
}(base_datasource_1.HttpRestBaseDataSource));
exports.HttpRestCollectionDataSource = HttpRestCollectionDataSource;
//# sourceMappingURL=collection.datasource.js.map