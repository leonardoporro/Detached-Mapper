"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var data_column_component_1 = require("./data-column.component");
var Item = (function () {
    function Item(model, parent) {
        this.model = model;
        this.parent = parent;
        this._selected = false;
    }
    Object.defineProperty(Item.prototype, "selected", {
        get: function () {
            return this._selected;
        },
        set: function (value) {
            this._selected = value;
            this.parent.updateSelection(this);
        },
        enumerable: true,
        configurable: true
    });
    return Item;
}());
var ItemsComponent = (function () {
    function ItemsComponent() {
        this.items = [];
        this.selection = [];
        this.selectionChanged = new core_1.EventEmitter();
    }
    Object.defineProperty(ItemsComponent.prototype, "itemsSource", {
        set: function (values) {
            var _this = this;
            this.items = values.map(function (v) { return new Item(v, _this); });
        },
        enumerable: true,
        configurable: true
    });
    ItemsComponent.prototype.updateSelection = function (item) {
        if (!this.selection) {
            this.selection = [];
        }
        var existing = this.selection.findIndex(function (v) { return v === item.model; });
        if (item.selected && existing < 0)
            this.selection.push(item.model);
        else if (!item.selected && existing >= 0)
            this.selection.splice(existing, 1);
        this.selectionChanged.emit(this.selection);
        this.allSelected = this.items.length == this.selection.length;
    };
    ItemsComponent.prototype.selectAll = function () {
        var sel = this.allSelected;
        for (var _i = 0, _a = this.items; _i < _a.length; _i++) {
            var item = _a[_i];
            item.selected = sel;
        }
    };
    return ItemsComponent;
}());
var DataTableComponent = (function (_super) {
    __extends(DataTableComponent, _super);
    function DataTableComponent() {
        _super.apply(this, arguments);
    }
    __decorate([
        core_1.ContentChildren(data_column_component_1.DataColumnComponent), 
        __metadata('design:type', Array)
    ], DataTableComponent.prototype, "columns", void 0);
    DataTableComponent = __decorate([
        core_1.Component({
            selector: 'md-data-table',
            template: require("./data-table.component.html"),
            encapsulation: core_1.ViewEncapsulation.None,
            styles: [require("./data-table.component.scss")],
            inputs: ["itemsSource", "selection"]
        }), 
        __metadata('design:paramtypes', [])
    ], DataTableComponent);
    return DataTableComponent;
}(ItemsComponent));
exports.DataTableComponent = DataTableComponent;
//# sourceMappingURL=data-table.component.js.map