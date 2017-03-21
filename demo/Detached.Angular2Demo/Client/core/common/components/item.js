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
var ItemComponent = (function () {
    function ItemComponent() {
        this._selected = false;
        this.selectedChange = new core_1.EventEmitter();
        this.modelChange = new core_1.EventEmitter();
    }
    Object.defineProperty(ItemComponent.prototype, "selected", {
        get: function () {
            return this._selected;
        },
        set: function (value) {
            if (this._selected != value) {
                this._selected = value;
                this.selectedChange.emit(this);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ItemComponent.prototype, "model", {
        get: function () {
            return this._model;
        },
        set: function (value) {
            if (this._model != value) {
                this._model = value;
                this.modelChange.emit(this);
            }
        },
        enumerable: true,
        configurable: true
    });
    return ItemComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean),
    __metadata("design:paramtypes", [Boolean])
], ItemComponent.prototype, "selected", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], ItemComponent.prototype, "selectedChange", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean),
    __metadata("design:paramtypes", [Boolean])
], ItemComponent.prototype, "model", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], ItemComponent.prototype, "modelChange", void 0);
ItemComponent = __decorate([
    core_1.Component({
        selector: "item",
        template: "<ng-content></ng-content>",
        exportAs: "listItem"
    })
], ItemComponent);
exports.ItemComponent = ItemComponent;
//# sourceMappingURL=item.js.map