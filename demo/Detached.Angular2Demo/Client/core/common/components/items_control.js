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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Observable_1 = require("rxjs/Observable");
var selection_1 = require("../data/selection");
var item_1 = require("./item");
var ItemsControlComponent = (function () {
    function ItemsControlComponent() {
        this._allSelected = false;
        this.selectedItemsChange = new core_1.EventEmitter();
    }
    Object.defineProperty(ItemsControlComponent.prototype, "collection", {
        get: function () {
            return this._collection;
        },
        set: function (value) {
            this._collection = value;
            this._subscribeAllSelectedChange();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemsControlComponent.prototype, "selection", {
        get: function () {
            return this._selection;
        },
        set: function (value) {
            this._selection = value;
            this._subscribeAllSelectedChange();
            this._subscribeSelection();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemsControlComponent.prototype, "allSelected", {
        get: function () {
            return this._allSelected;
        },
        set: function (value) {
            if (this._allSelected != value) {
                if (this._selection && this._collection) {
                    this._allSelected = value;
                    if (value) {
                        this._selection.replace(this._collection.getValues());
                    }
                    else {
                        this._selection.clear();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemsControlComponent.prototype, "selectedItems", {
        get: function () {
            return this._selectedItems;
        },
        set: function (value) {
            if (value != this._selectedItems) {
                if (this._selection) {
                    this._selectedItems = value;
                    this._selection.replace(value);
                    this.selectedItemsChange.emit(value);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ItemsControlComponent.prototype.ngAfterViewInit = function () {
        if (!this.selection) {
            this.selection = new selection_1.Selection();
        }
        this._subscribeItemChanges();
    };
    ItemsControlComponent.prototype._subscribeAllSelectedChange = function () {
        var _this = this;
        if (this._allSelectedSubscription)
            this._allSelectedSubscription.unsubscribe();
        if (this._collection && this._selection) {
            this._allSelectedSubscription = Observable_1.Observable.combineLatest(this._collection.values, this._selection.values)
                .subscribe(function (sources) { return _this._onAllSelectedChange(sources[0], sources[1]); });
        }
    };
    ItemsControlComponent.prototype._onAllSelectedChange = function (collectionValues, selectionValues) {
        var result = false;
        if (collectionValues && selectionValues) {
            if (collectionValues.length > 0 && collectionValues.length >= selectionValues.length) {
                result = true;
                for (var _i = 0, collectionValues_1 = collectionValues; _i < collectionValues_1.length; _i++) {
                    var item = collectionValues_1[_i];
                    if (!this.selection.has(item)) {
                        result = false;
                        break;
                    }
                }
            }
        }
        this._allSelected = result;
    };
    ItemsControlComponent.prototype._subscribeItemChanges = function () {
        var _this = this;
        if (this.items) {
            this.items.changes.startWith([])
                .subscribe(function (items) {
                _this._subscribeItemsModelChange(items);
                _this._subscribeItemsSelectedChange(items);
            });
        }
    };
    ItemsControlComponent.prototype._subscribeItemsSelectedChange = function (items) {
        if (this._itemSelectedSubscription) {
            this._itemSelectedSubscription.unsubscribe();
        }
        this._itemSelectedSubscription = Observable_1.Observable.merge.apply(Observable_1.Observable, items.map(function (r) { return r.selectedChange; })).subscribe(this._onItemSelectedChange.bind(this));
    };
    ItemsControlComponent.prototype._onItemSelectedChange = function (item) {
        if (item.selected) {
            this.selection.add(item.model);
        }
        else {
            this.selection.remove(item.model);
        }
    };
    ItemsControlComponent.prototype._subscribeItemsModelChange = function (items) {
        if (this._modelSelectedSubscription) {
            this._modelSelectedSubscription.unsubscribe();
        }
        this._modelSelectedSubscription = Observable_1.Observable.merge.apply(Observable_1.Observable, items.map(function (r) { return r.modelChange; })).subscribe(this._onItemModelChange.bind(this));
    };
    ItemsControlComponent.prototype._onItemModelChange = function (item) {
        item.selected = this._selection.has(item.model);
    };
    ItemsControlComponent.prototype._subscribeSelection = function () {
        this._selectionSubscription = this.selection.values
            .subscribe(this._onSelectionChange.bind(this));
    };
    ItemsControlComponent.prototype._onSelectionChange = function (values) {
        var _this = this;
        if (this.items) {
            this.items.forEach(function (row) {
                row.selected = _this.selection.has(row.model);
            });
        }
        this._selectedItems = values;
        this.selectedItemsChange.emit(values);
    };
    ItemsControlComponent.prototype.ngOnDestroy = function () {
        if (this._itemSelectedSubscription) {
            this._itemSelectedSubscription.unsubscribe();
        }
        if (this._modelSelectedSubscription) {
            this._modelSelectedSubscription.unsubscribe();
        }
        if (this._selectionSubscription) {
            this._selectionSubscription.unsubscribe();
        }
        if (this._allSelectedSubscription) {
            this._allSelectedSubscription.unsubscribe();
        }
    };
    return ItemsControlComponent;
}());
__decorate([
    core_1.ViewChildren(item_1.ItemComponent),
    __metadata("design:type", core_1.QueryList)
], ItemsControlComponent.prototype, "items", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], ItemsControlComponent.prototype, "collection", null);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Object])
], ItemsControlComponent.prototype, "selection", null);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean),
    __metadata("design:paramtypes", [Boolean])
], ItemsControlComponent.prototype, "allSelected", null);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object),
    __metadata("design:paramtypes", [Array])
], ItemsControlComponent.prototype, "selectedItems", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], ItemsControlComponent.prototype, "selectedItemsChange", void 0);
exports.ItemsControlComponent = ItemsControlComponent;
//# sourceMappingURL=items_control.js.map