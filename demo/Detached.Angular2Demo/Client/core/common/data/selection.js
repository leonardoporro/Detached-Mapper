"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var BehaviorSubject_1 = require("rxjs/BehaviorSubject");
require("rxjs");
var Selection = (function () {
    function Selection() {
        this._values = new BehaviorSubject_1.BehaviorSubject([]);
        this._keys = new BehaviorSubject_1.BehaviorSubject([]);
        this._map = new Map();
        this.values = this._values.asObservable();
        this.keys = this._keys.asObservable();
    }
    Object.defineProperty(Selection.prototype, "length", {
        get: function () {
            return this._map.size;
        },
        enumerable: true,
        configurable: true
    });
    Selection.prototype.has = function (model) {
        return this._map.has(this.getKey(model));
    };
    Selection.prototype.add = function (model) {
        var key = this.getKey(model);
        if (!this._map.has(key)) {
            this._map.set(key, model);
            this._nextValues();
        }
    };
    Selection.prototype.remove = function (model) {
        if (this._map.delete(this.getKey(model))) {
            this._nextValues();
        }
    };
    Selection.prototype.toggle = function (model, selected) {
        if (selected == undefined) {
            selected = this.has(model);
        }
        if (selected)
            this.add(model);
        else
            this.remove(model);
    };
    Selection.prototype.clear = function () {
        if (this._map.size > 0) {
            this._map.clear();
            this._nextValues();
        }
    };
    Selection.prototype.replace = function (models) {
        if (models && models.length) {
            this._map.clear();
            for (var _i = 0, models_1 = models; _i < models_1.length; _i++) {
                var model = models_1[_i];
                this._map.set(this.getKey(model), model);
            }
            this._nextValues();
        }
    };
    Selection.prototype.getKey = function (model) {
        return model["id"];
    };
    Selection.prototype._nextValues = function () {
        this._values.next(Array.from(this._map.values()));
        this._keys.next(Array.from(this._map.keys()));
    };
    return Selection;
}());
exports.Selection = Selection;
//# sourceMappingURL=selection.js.map