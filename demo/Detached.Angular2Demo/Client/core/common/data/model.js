"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Subject_1 = require("rxjs/Subject");
var BehaviorSubject_1 = require("rxjs/BehaviorSubject");
var Model = (function () {
    function Model(service) {
        this._busy = new BehaviorSubject_1.BehaviorSubject(false);
        this.model = {};
        this.busy = this._busy.asObservable();
        this._service = service;
    }
    Model.prototype.load = function (key) {
        var _this = this;
        this._busy.next(true);
        this._service.get(key)
            .subscribe(function (data) {
            _this.model = data;
            _this._busy.next(false);
        }, function (error) {
            _this._busy.next(false);
        });
    };
    Model.prototype.save = function () {
        var _this = this;
        var result = new Subject_1.Subject();
        this._busy.next(true);
        this._service.post(this.model)
            .subscribe(function (model) {
            _this.model = model;
            result.next(model);
            _this._busy.next(false);
        }, function (error) {
            result.error(error);
            _this._busy.next(false);
        });
        return result;
    };
    Model.prototype.delete = function (key) {
        this._service.delete(key)
            .subscribe(function (result) {
        }, function (error) {
        });
    };
    return Model;
}());
exports.Model = Model;
//# sourceMappingURL=model.js.map