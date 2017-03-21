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
var message_box_1 = require("../services/message_box");
var MdConfirmationBoxDirective = (function () {
    function MdConfirmationBoxDirective(_msgBoxService) {
        this._msgBoxService = _msgBoxService;
        this.ok = new core_1.EventEmitter();
        this.cancel = new core_1.EventEmitter();
    }
    MdConfirmationBoxDirective.prototype.show = function (data) {
        var _this = this;
        this._msgBoxService.confirm(this.title, this.message)
            .afterClosed()
            .subscribe(function (result) {
            if (result) {
                _this.ok.emit(data);
            }
            else {
                _this.cancel.emit(data);
            }
        });
    };
    return MdConfirmationBoxDirective;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdConfirmationBoxDirective.prototype, "title", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdConfirmationBoxDirective.prototype, "message", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], MdConfirmationBoxDirective.prototype, "ok", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], MdConfirmationBoxDirective.prototype, "cancel", void 0);
MdConfirmationBoxDirective = __decorate([
    core_1.Directive({
        selector: "md-confirmation-box",
        exportAs: "mdConfirmationBox"
    }),
    __metadata("design:paramtypes", [message_box_1.MdMessageBoxService])
], MdConfirmationBoxDirective);
exports.MdConfirmationBoxDirective = MdConfirmationBoxDirective;
//# sourceMappingURL=confirmation_box.js.map