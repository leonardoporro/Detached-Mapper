"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BehaviorSubject_1 = require("rxjs/BehaviorSubject");
var Subject_1 = require("rxjs/Subject");
var Collection = (function () {
    function Collection(service) {
        this._values = new BehaviorSubject_1.BehaviorSubject(new Array());
        this._busy = new BehaviorSubject_1.BehaviorSubject(false);
        this._error = new Subject_1.Subject();
        this.params = { pageIndex: 1, pageSize: 0 };
        this.values = this._values.asObservable();
        this.busy = this._busy.asObservable();
        this.error = this._error.asObservable();
        this._service = service;
    }
    Object.defineProperty(Collection.prototype, "length", {
        get: function () {
            return this._values.getValue().length;
        },
        enumerable: true,
        configurable: true
    });
    Collection.prototype.load = function () {
        var _this = this;
        this._busy.next(true);
        this._service.query(this.params)
            .subscribe(function (result) {
            _this._values.next(result.items);
            _this.pageCount = result.pageCount;
            _this.rowCount = result.pageCount;
            _this._busy.next(false);
        }, function (error) {
            _this._error.next(error);
            _this._busy.next(false);
        });
    };
    Collection.prototype.getValues = function () {
        return this._values.getValue();
    };
    Collection.prototype.getKey = function (model) {
        return model["id"];
    };
    return Collection;
}());
exports.Collection = Collection;
var service_1 = require("./service");
exports.SortOrder = service_1.SortOrder;
//# sourceMappingURL=collection.js.map