"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var Item = (function () {
    function Item(model) {
        this.model = model;
        this._selected = false;
        this.selectedChange = new core_1.EventEmitter();
    }
    Object.defineProperty(Item.prototype, "selected", {
        get: function () {
            return this._selected;
        },
        set: function (value) {
            if (this._selected !== value) {
                this._selected = value;
                this.selectedChange.emit(this);
            }
        },
        enumerable: true,
        configurable: true
    });
    Item.prototype.updateSelected = function (value) {
        this._selected = value;
    };
    return Item;
}());
exports.Item = Item;
var ItemsComponent = (function () {
    function ItemsComponent() {
        this._items = [];
        this._selection = [];
        this._suspendFlag = false;
        this.itemsChange = new core_1.EventEmitter();
        this.selectionChange = new core_1.EventEmitter();
        this.allSelectedChange = new core_1.EventEmitter();
    }
    Object.defineProperty(ItemsComponent.prototype, "items", {
        get: function () {
            return this._items;
        },
        set: function (value) {
            if (this._items !== value) {
                if (this._items) {
                    for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                        var item = _a[_i];
                        item.selectedChange.unsubscribe();
                    }
                }
                this._items = value;
                if (this._items) {
                    for (var _b = 0, _c = this._items; _b < _c.length; _b++) {
                        var item = _c[_b];
                        item.selectedChange.subscribe(this.itemSelectedChanged.bind(this));
                    }
                }
                this.syncSelection();
                this.itemsChange.emit(this._items);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemsComponent.prototype, "itemsSource", {
        get: function () {
            return this._items;
        },
        set: function (values) {
            if (this._itemsSource !== values) {
                this._itemsSource = values;
                this.items = values.map(function (v) { return new Item(v); });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemsComponent.prototype, "selection", {
        get: function () {
            return this._selection;
        },
        set: function (value) {
            if (this._selection !== value) {
                this._selection = value;
                this.syncSelection();
                this.selectionChange.emit(this._selection);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemsComponent.prototype, "allSelected", {
        get: function () {
            return this._allSelected;
        },
        set: function (value) {
            if (this._allSelected !== value) {
                this._allSelected = value;
                this._suspendFlag = true;
                this._selection = [];
                if (value) {
                    for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                        var item = _a[_i];
                        item.selected = true;
                        this._selection.push(item.model);
                    }
                }
                else {
                    for (var _b = 0, _c = this._items; _b < _c.length; _b++) {
                        var item = _c[_b];
                        item.selected = false;
                    }
                }
                this.selectionChange.emit(this._selection);
                this.allSelectedChange.emit(this._allSelected);
                this._suspendFlag = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemsComponent.prototype.itemSelectedChanged = function (item) {
        var _this = this;
        if (!this._suspendFlag) {
            if (!this._selection) {
                this._selection = [];
            }
            var existing = this._selection.findIndex(function (s) { return s[_this.valueProperty] == item.model[_this.valueProperty]; });
            if (item.selected && existing < 0)
                this._selection.push(item.model);
            else if (!item.selected && existing >= 0)
                this._selection.splice(existing, 1);
            this.selectionChange.emit(this._selection);
            this.syncAllSelected();
        }
    };
    ItemsComponent.prototype.syncSelection = function () {
        var _this = this;
        this._suspendFlag = true;
        if (this._items && this._selection) {
            var _loop_1 = function(item) {
                item.selected = this_1._selection.find(function (s) { return item.model[_this.valueProperty] == s[_this.valueProperty]; }) !== undefined;
            };
            var this_1 = this;
            for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                var item = _a[_i];
                _loop_1(item);
            }
            this.syncAllSelected();
        }
        this._suspendFlag = false;
    };
    ItemsComponent.prototype.syncAllSelected = function () {
        var allSelected = this._items.length == this._selection.length;
        if (this._allSelected !== allSelected) {
            this._allSelected = allSelected;
            this.allSelectedChange.emit(allSelected);
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ItemsComponent.prototype, "valueProperty", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ItemsComponent.prototype, "displayProperty", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ItemsComponent.prototype, "items", null);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], ItemsComponent.prototype, "itemsChange", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ItemsComponent.prototype, "itemsSource", null);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ItemsComponent.prototype, "selection", null);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], ItemsComponent.prototype, "selectionChange", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean)
    ], ItemsComponent.prototype, "allSelected", null);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', core_1.EventEmitter)
    ], ItemsComponent.prototype, "allSelectedChange", void 0);
    return ItemsComponent;
}());
exports.ItemsComponent = ItemsComponent;
//# sourceMappingURL=items.component.js.map