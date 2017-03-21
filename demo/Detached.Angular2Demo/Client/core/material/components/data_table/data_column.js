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
var ColumnType;
(function (ColumnType) {
    ColumnType[ColumnType["Text"] = 1] = "Text";
    ColumnType[ColumnType["Number"] = 2] = "Number";
    ColumnType[ColumnType["Template"] = 3] = "Template";
})(ColumnType = exports.ColumnType || (exports.ColumnType = {}));
var MdDataColumnComponent = (function () {
    function MdDataColumnComponent() {
    }
    MdDataColumnComponent.prototype.onResize = function (nativeElement) {
        this.actualSize = nativeElement.clientWidth;
    };
    MdDataColumnComponent.prototype.setup = function () {
        // default for canSort
        if (this.canSort == undefined && this.property) {
            this.canSort = true;
        }
        // default for type
        if (this.type == undefined) {
            if (this.template)
                this.type = ColumnType.Template;
            else
                this.type = ColumnType.Text;
        }
        // default for title
        if (this.title == undefined && this.property) {
            this.title = this.property;
        }
        // default for size
        if (this.size == undefined) {
            switch (this.type) {
                case ColumnType.Text:
                    this.size = "grow";
                    break;
                default:
                    this.size = "stretch";
                    break;
            }
        }
        // default for minWidth
        if (this.minWidth == undefined) {
            switch (this.type) {
                case ColumnType.Text:
                    this.minWidth = 125;
                    break;
                case ColumnType.Number:
                    this.minWidth = 50;
                    break;
                default:
                    this.minWidth = 50;
                    break;
            }
        }
        // default for priority
        if (this.priority == undefined) {
            if (this.type == ColumnType.Template)
                this.priority = 0;
            else
                this.priority = this.index;
        }
        // default for visible
        if (this.visible == undefined) {
            this.visible = true;
        }
    };
    return MdDataColumnComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdDataColumnComponent.prototype, "title", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], MdDataColumnComponent.prototype, "type", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdDataColumnComponent.prototype, "property", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], MdDataColumnComponent.prototype, "canSort", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdDataColumnComponent.prototype, "size", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], MdDataColumnComponent.prototype, "minWidth", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], MdDataColumnComponent.prototype, "priority", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], MdDataColumnComponent.prototype, "visible", void 0);
__decorate([
    core_1.ContentChild(core_1.TemplateRef),
    __metadata("design:type", core_1.TemplateRef)
], MdDataColumnComponent.prototype, "template", void 0);
MdDataColumnComponent = __decorate([
    core_1.Directive({
        selector: "md-data-column"
    })
], MdDataColumnComponent);
exports.MdDataColumnComponent = MdDataColumnComponent;
//# sourceMappingURL=data_column.js.map