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
var column_component_1 = require("./column.component");
var collection_component_1 = require("../collection/collection.component");
var TableComponent = (function (_super) {
    __extends(TableComponent, _super);
    function TableComponent() {
        _super.apply(this, arguments);
        this._columns = [];
    }
    Object.defineProperty(TableComponent.prototype, "columnsQuery", {
        set: function (value) {
            this.columns = value.toArray();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TableComponent.prototype, "columns", {
        get: function () {
            return this._columns;
        },
        set: function (value) {
            this._columns = value;
            var index = 1;
            for (var _i = 0, _a = this._columns; _i < _a.length; _i++) {
                var column = _a[_i];
                // default for canSort
                if (column.canSort == undefined && column.property) {
                    column.canSort = true;
                }
                // default for type
                if (column.type == undefined) {
                    if (column.template)
                        column.type = column_component_1.ColumnType.Template;
                    else
                        column.type = column_component_1.ColumnType.Text;
                }
                // default for title
                if (column.title == undefined && column.property) {
                    column.title = column.property;
                }
                // default for size
                if (column.size == undefined) {
                    switch (column.type) {
                        case column_component_1.ColumnType.Text:
                            column.size = "grow";
                            break;
                        default:
                            column.size = "stretch";
                            break;
                    }
                }
                // default for minWidth
                if (column.minWidth == undefined) {
                    switch (column.type) {
                        case column_component_1.ColumnType.Text:
                            column.minWidth = 300;
                            break;
                        case column_component_1.ColumnType.Number:
                            column.minWidth = 100;
                            break;
                        default:
                            column.minWidth = 100;
                            break;
                    }
                }
                // default for priority
                if (column.priority == undefined) {
                    column.priority = index;
                }
                if (column.visible == undefined) {
                    column.visible = true;
                }
            }
            this.setColumnWidths();
        },
        enumerable: true,
        configurable: true
    });
    TableComponent.prototype.toggleSort = function (column) {
        if (column.canSort && column.property) {
            this.dataSource.toggleSort(column.property);
        }
    };
    TableComponent.prototype.setColumnWidths = function () {
        var total = 100;
        var count = 0;
        for (var _i = 0, _a = this._columns; _i < _a.length; _i++) {
            var column = _a[_i];
            if (column.size == "grow") {
                count++;
            }
            else if (column.size.endsWith("%")) {
                var percentSize = parseInt(column.size.substr(0, column.size.length - 1));
                total -= percentSize;
            }
        }
        for (var _b = 0, _c = this._columns; _b < _c.length; _b++) {
            var column = _c[_b];
            switch (column.size) {
                case "grow":
                    column.clientSize = total / count + "%";
                    break;
                case "shrink":
                    column.clientSize = "auto";
                    break;
                default:
                    column.clientSize = column.size;
                    break;
            }
        }
    };
    TableComponent.prototype.sizeChanged = function (e) {
        console.debug(e.clientWidth);
    };
    __decorate([
        core_1.ContentChildren(column_component_1.ColumnComponent), 
        __metadata('design:type', core_1.QueryList), 
        __metadata('design:paramtypes', [core_1.QueryList])
    ], TableComponent.prototype, "columnsQuery", null);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], TableComponent.prototype, "columns", null);
    TableComponent = __decorate([
        core_1.Component({
            selector: 'd-table',
            template: require("./table.component.html"),
            encapsulation: core_1.ViewEncapsulation.None
        }), 
        __metadata('design:paramtypes', [])
    ], TableComponent);
    return TableComponent;
}(collection_component_1.CollectionComponent));
exports.TableComponent = TableComponent;
//# sourceMappingURL=table.component.js.map