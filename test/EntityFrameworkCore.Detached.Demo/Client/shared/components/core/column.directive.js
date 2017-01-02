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
(function (ColumnOrder) {
    ColumnOrder[ColumnOrder["None"] = 0] = "None";
    ColumnOrder[ColumnOrder["Asc"] = 1] = "Asc";
    ColumnOrder[ColumnOrder["Desc"] = 2] = "Desc";
})(exports.ColumnOrder || (exports.ColumnOrder = {}));
var ColumnOrder = exports.ColumnOrder;
var ColumnDirective = (function () {
    function ColumnDirective(viewContainer) {
        this.viewContainer = viewContainer;
        this.type = "text";
        this._order = ColumnOrder.None;
        this.orderChanged = new core_1.EventEmitter();
    }
    Object.defineProperty(ColumnDirective.prototype, "order", {
        get: function () {
            return this._order;
        },
        set: function (value) {
            this._order = value;
            this.orderChanged.emit(this);
        },
        enumerable: true,
        configurable: true
    });
    ColumnDirective.prototype.toggleOrder = function () {
        switch (this._order) {
            case ColumnOrder.None:
                this.order = ColumnOrder.Asc;
                break;
            case ColumnOrder.Asc:
                this.order = ColumnOrder.Desc;
                break;
            case ColumnOrder.Desc:
                this.order = ColumnOrder.Asc;
                break;
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnDirective.prototype, "title", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnDirective.prototype, "type", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], ColumnDirective.prototype, "property", void 0);
    __decorate([
        core_1.ContentChild(core_1.TemplateRef), 
        __metadata('design:type', core_1.TemplateRef)
    ], ColumnDirective.prototype, "template", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], ColumnDirective.prototype, "order", null);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', core_1.EventEmitter)
    ], ColumnDirective.prototype, "orderChanged", void 0);
    ColumnDirective = __decorate([
        core_1.Directive({
            selector: "column"
        }), 
        __metadata('design:paramtypes', [core_1.ViewContainerRef])
    ], ColumnDirective);
    return ColumnDirective;
}());
exports.ColumnDirective = ColumnDirective;
//# sourceMappingURL=column.directive.js.map