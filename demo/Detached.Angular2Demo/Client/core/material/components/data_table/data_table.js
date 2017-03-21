"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var collection_1 = require("../../../common/data/collection");
var data_column_1 = require("./data_column");
var items_control_1 = require("../../../common/components/items_control");
var MdDataTableComponent = (function (_super) {
    __extends(MdDataTableComponent, _super);
    function MdDataTableComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    MdDataTableComponent.prototype.ngAfterViewInit = function () {
        _super.prototype.ngAfterViewInit.call(this);
        this.columns.forEach(function (col, index) {
            col.index = index + 1;
            col.setup();
        });
        this.updateColumnWidths();
    };
    MdDataTableComponent.prototype.toggleSort = function (column) {
        if (this.collection) {
            this.collection.params.sortBy = column.property;
            if (this.collection.params.sortOrder == collection_1.SortOrder.Desc)
                this.collection.params.sortOrder = collection_1.SortOrder.Asc;
            else
                this.collection.params.sortOrder = collection_1.SortOrder.Desc;
            this.collection.load();
        }
    };
    MdDataTableComponent.prototype.updateColumnWidths = function () {
        var total = 100;
        var count = 0;
        this.columns.forEach(function (column) {
            if (column.visible) {
                if (column.size == "grow") {
                    count++;
                }
                else if (column.size.endsWith("%")) {
                    var percentSize = parseInt(column.size.substr(0, column.size.length - 1));
                    total -= percentSize;
                }
            }
        });
        this.columns.forEach(function (column) {
            if (column.visible) {
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
        });
    };
    MdDataTableComponent.prototype.onResize = function (nativeElement) {
        if (this.columns) {
            var minWidthAll_1 = 0;
            var pVisibleColumn_1 = null;
            var pHiddenColumn_1 = null;
            this.columns.forEach(function (column) {
                if (column.visible) {
                    minWidthAll_1 += column.minWidth;
                    if (column.priority > 0 && (pVisibleColumn_1 == null || column.priority > pVisibleColumn_1.priority)) {
                        pVisibleColumn_1 = column;
                    }
                }
                else {
                    if (column.priority > 0 && (pHiddenColumn_1 == null || column.priority < pHiddenColumn_1.priority)) {
                        pHiddenColumn_1 = column;
                    }
                }
            });
            var delta = nativeElement.clientWidth - minWidthAll_1;
            if (delta < 0 && pVisibleColumn_1) {
                pVisibleColumn_1.visible = false;
                this.updateColumnWidths();
            }
            if (pHiddenColumn_1 && delta > pHiddenColumn_1.minWidth) {
                pHiddenColumn_1.visible = true;
                this.updateColumnWidths();
            }
        }
    };
    return MdDataTableComponent;
}(items_control_1.ItemsControlComponent));
__decorate([
    core_1.ContentChildren(data_column_1.MdDataColumnComponent),
    __metadata("design:type", core_1.QueryList)
], MdDataTableComponent.prototype, "columns", void 0);
MdDataTableComponent = __decorate([
    core_1.Component({
        selector: 'md-data-table',
        template: require("./data_table.html"),
        encapsulation: core_1.ViewEncapsulation.None
    })
], MdDataTableComponent);
exports.MdDataTableComponent = MdDataTableComponent;
//# sourceMappingURL=data_table.js.map