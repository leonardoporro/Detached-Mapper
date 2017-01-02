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
var column_directive_1 = require("../core/column.directive");
var items_component_1 = require("../core/items.component");
var DataTableComponent = (function (_super) {
    __extends(DataTableComponent, _super);
    function DataTableComponent() {
        _super.apply(this, arguments);
        this._columns = [];
        this.orderByChanged = new core_1.EventEmitter();
    }
    Object.defineProperty(DataTableComponent.prototype, "columns", {
        get: function () {
            return this._columns;
        },
        set: function (value) {
            if (this._columns) {
                for (var _i = 0, _a = this._columns; _i < _a.length; _i++) {
                    var column = _a[_i];
                    column.orderChanged.unsubscribe();
                }
            }
            this._columns = value;
            if (this._columns) {
                for (var _b = 0, _c = this._columns; _b < _c.length; _b++) {
                    var column = _c[_b];
                    column.orderChanged.subscribe(this.columnOrderChanged.bind(this));
                }
            }
            this.syncOrderBy();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DataTableComponent.prototype, "orderBy", {
        get: function () {
            return this._orderBy;
        },
        set: function (value) {
            if (this._orderBy !== value) {
                this._orderBy = value;
                this.syncOrderBy();
                this.orderByChanged.emit(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    DataTableComponent.prototype.ngAfterContentInit = function () {
        if (this._columnQuery)
            this.columns = this._columnQuery.toArray();
    };
    DataTableComponent.prototype.columnOrderChanged = function (sortedColumn) {
        if (sortedColumn.order !== column_directive_1.ColumnOrder.None) {
            for (var _i = 0, _a = this._columns; _i < _a.length; _i++) {
                var column = _a[_i];
                if (sortedColumn !== column) {
                    column.order = column_directive_1.ColumnOrder.None;
                }
            }
            this._orderBy = (sortedColumn.order == column_directive_1.ColumnOrder.Asc ? "+" : "-") + sortedColumn.property;
            this.orderByChanged.emit(this.orderBy);
        }
    };
    DataTableComponent.prototype.syncOrderBy = function () {
        if (this._columns && this._orderBy) {
            var order = this._orderBy.substr(0, 1);
            var property = this._orderBy.substr(1);
            for (var _i = 0, _a = this._columns; _i < _a.length; _i++) {
                var column = _a[_i];
                if (column.property == property) {
                    if (order == "+")
                        column.order = column_directive_1.ColumnOrder.Asc;
                    else
                        column.order = column_directive_1.ColumnOrder.Desc;
                    return;
                }
            }
        }
    };
    __decorate([
        core_1.ContentChildren(column_directive_1.ColumnDirective), 
        __metadata('design:type', core_1.QueryList)
    ], DataTableComponent.prototype, "_columnQuery", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], DataTableComponent.prototype, "columns", null);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], DataTableComponent.prototype, "orderBy", null);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DataTableComponent.prototype, "orderByChanged", void 0);
    DataTableComponent = __decorate([
        core_1.Component({
            selector: 'md-data-table',
            template: require("./data-table.component.html"),
            encapsulation: core_1.ViewEncapsulation.None,
            styles: [require("./data-table.component.scss")],
        }), 
        __metadata('design:paramtypes', [])
    ], DataTableComponent);
    return DataTableComponent;
}(items_component_1.ItemsComponent));
exports.DataTableComponent = DataTableComponent;
//# sourceMappingURL=data-table.component.js.map