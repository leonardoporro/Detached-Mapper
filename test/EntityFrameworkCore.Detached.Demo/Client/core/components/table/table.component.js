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
        this.columns = [];
    }
    TableComponent.prototype.toggleSort = function (canSort, propertyName) {
        if (canSort) {
            this.dataSource.toggleSort(propertyName);
        }
    };
    __decorate([
        core_1.ContentChildren(column_component_1.ColumnComponent), 
        __metadata('design:type', Array)
    ], TableComponent.prototype, "columns", void 0);
    TableComponent = __decorate([
        core_1.Component({
            selector: 'd-table',
            template: require("./table.component.html"),
            encapsulation: core_1.ViewEncapsulation.None,
            styles: [require("./table.component.scss")],
        }), 
        __metadata('design:paramtypes', [])
    ], TableComponent);
    return TableComponent;
}(collection_component_1.CollectionComponent));
exports.TableComponent = TableComponent;
//# sourceMappingURL=table.component.js.map