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
var message_box_1 = require("../components/message_box/message_box");
var MdMessageBoxService = (function () {
    function MdMessageBoxService(_dialog) {
        this._dialog = _dialog;
    }
    MdMessageBoxService.prototype.open = function (title, messages, icon, options) {
        var dialogRef = this._dialog.open(message_box_1.MdMessageBoxComponent, { disableClose: true });
        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.messages = messages;
        dialogRef.componentInstance.icon = icon;
        dialogRef.componentInstance.options = options;
        return dialogRef;
    };
    MdMessageBoxService.prototype.confirm = function (title, message) {
        return this.open(title, [message], null, [
            new message_box_1.MdMessageBoxOption("Yes", true),
            new message_box_1.MdMessageBoxOption("No", false)
        ]);
    };
    MdMessageBoxService.prototype.error = function (title, message) {
        return this.open(title, [message], null, [
            new message_box_1.MdMessageBoxOption("OK", true)
        ]);
    };
    return MdMessageBoxService;
}());
MdMessageBoxService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [material_1.MdDialog])
], MdMessageBoxService);
exports.MdMessageBoxService = MdMessageBoxService;
//# sourceMappingURL=message_box.js.map