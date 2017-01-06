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
var core_1 = require("@angular/core");
var ColumnComponent = (function () {
    function ColumnComponent(viewContainer) {
        this.viewContainer = viewContainer;
        this.type = "text";
        this.canSort = false;
    }
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnComponent.prototype, "title", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnComponent.prototype, "type", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnComponent.prototype, "property", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean)
    ], ColumnComponent.prototype, "canSort", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnComponent.prototype, "width", void 0);
    __decorate([
        core_1.ContentChild(core_1.TemplateRef), 
        __metadata('design:type', core_1.TemplateRef)
    ], ColumnComponent.prototype, "template", void 0);
    ColumnComponent = __decorate([
        core_1.Directive({
            selector: "d-column"
        }), 
        __metadata('design:paramtypes', [core_1.ViewContainerRef])
    ], ColumnComponent);
    return ColumnComponent;
}());
exports.ColumnComponent = ColumnComponent;
//# sourceMappingURL=column.component.js.map