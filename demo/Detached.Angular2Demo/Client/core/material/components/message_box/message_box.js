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
var material_1 = require("@angular/material");
var MdMessageBoxComponent = (function () {
    function MdMessageBoxComponent(_dialogRef) {
        this._dialogRef = _dialogRef;
        this.messages = [];
    }
    MdMessageBoxComponent.prototype.close = function (result) {
        this._dialogRef.close(result);
    };
    return MdMessageBoxComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdMessageBoxComponent.prototype, "title", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], MdMessageBoxComponent.prototype, "messages", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdMessageBoxComponent.prototype, "icon", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], MdMessageBoxComponent.prototype, "options", void 0);
MdMessageBoxComponent = __decorate([
    core_1.Component({
        selector: "md-message-box-host",
        template: require("./message_box.html")
    }),
    __metadata("design:paramtypes", [material_1.MdDialogRef])
], MdMessageBoxComponent);
exports.MdMessageBoxComponent = MdMessageBoxComponent;
var MdMessageBoxOption = (function () {
    function MdMessageBoxOption(label, value, isDefault) {
        if (isDefault === void 0) { isDefault = false; }
        this.label = label;
        this.value = value;
        this.isDefault = isDefault;
    }
    return MdMessageBoxOption;
}());
exports.MdMessageBoxOption = MdMessageBoxOption;
//# sourceMappingURL=message_box.js.map