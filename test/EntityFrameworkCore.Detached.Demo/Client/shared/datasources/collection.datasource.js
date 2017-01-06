"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
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
        _super.call(this);
        this.http = http;
        this.baseUrl = baseUrl;
        this.keyProperty = "id";
        this.searchParams = {};
        this.sortDirection = SortDirection.Asc;
        this.pageIndex = 1;
        this.items = [];
        this.itemsChange = new core_1.EventEmitter();
    }
    /** builds a URLSearchParams instance from searchParams and collection parameters. */
    HttpRestCollectionDataSource.prototype.buildParameters = function () {
        var searchParams = new http_1.URLSearchParams();
        for (var queryProp in this.searchParams) {
            var value = this.searchParams[queryProp];
            if (value) {
                searchParams.append(queryProp, value);
            }
        }
        if (this.searchText) {
            searchParams.append("searchText", this.searchText);
        }
        if (this.sortBy) {
            searchParams.append("orderBy", this.sortBy + (this.sortDirection == SortDirection.Asc ? "+asc" : "+desc"));
        }
        if (this.pageIndex) {
            searchParams.append("pageIndex", this.pageIndex.toString());
        }
        if (this.pageSize) {
            searchParams.append("pageSize", this.pageSize.toString());
        }
        if (this.noCount !== undefined) {
            searchParams.append("noCount", this.noCount ? "true" : "false");
        }
        return searchParams;
    };
    HttpRestCollectionDataSource.prototype.load = function () {
        var searchParams = this.buildParameters();
        var resolvedUrl = this.resolveUrl(this.baseUrl, searchParams);
        var result = this.http.get(resolvedUrl, { search: searchParams }).map(function (r) { return r.json(); });
        result.subscribe(this.handleResponse.bind(this), this.handleError.bind(this));
        return result;
    };
    HttpRestCollectionDataSource.prototype.handleResponse = function (result) {
        if (result instanceof Array) {
            this.handleArray(result);
        }
        else {
            this.handlePage(result);
        }
    };
    HttpRestCollectionDataSource.prototype.handleArray = function (array) {
        this.items = array;
        this.itemsChange.emit(this.items);
    };
    HttpRestCollectionDataSource.prototype.handlePage = function (page) {
        this.items = page.items;
        this.pageCount = page.pageCount;
        this.rowCount = page.rowCount;
        this.itemsChange.emit(this.items);
    };
    HttpRestCollectionDataSource.prototype.handleError = function (error) {
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
    HttpRestCollectionDataSource.prototype.loadFirstPage = function () {
    };
    HttpRestCollectionDataSource.prototype.loadPreviousPage = function () {
    };
    HttpRestCollectionDataSource.prototype.loadNextPage = function () {
    };
    HttpRestCollectionDataSource.prototype.loadLastPage = function () {
    };
    return HttpRestCollectionDataSource;
}(base_datasource_1.HttpRestBaseDataSource));
exports.HttpRestCollectionDataSource = HttpRestCollectionDataSource;
//# sourceMappingURL=collection.datasource.js.map