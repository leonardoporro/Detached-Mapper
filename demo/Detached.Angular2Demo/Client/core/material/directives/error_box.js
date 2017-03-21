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
var Observable_1 = require("rxjs/Observable");
var message_box_1 = require("../services/message_box");
var MdErrorBoxDirective = (function () {
    function MdErrorBoxDirective(_msgBoxService) {
        this._msgBoxService = _msgBoxService;
        this.ok = new core_1.EventEmitter();
    }
    Object.defineProperty(MdErrorBoxDirective.prototype, "errors", {
        set: function (values) {
            if (this._errorsSubscription)
                this._errorsSubscription.unsubscribe();
            if (values) {
                this._errorsSubscription = Observable_1.Observable.merge.apply(Observable_1.Observable, values).subscribe(this.onError.bind(this));
            }
        },
        enumerable: true,
        configurable: true
    });
    MdErrorBoxDirective.prototype.show = function () {
        var _this = this;
        this._errorDialog = this._msgBoxService.error(this.title, this.message);
        this._errorDialog.afterClosed()
            .subscribe(function (result) {
            _this._errorDialog = null;
        });
    };
    MdErrorBoxDirective.prototype.onError = function (error) {
        if (!this._errorDialog)
            this.show();
        this._errorDialog.componentInstance.messages.push(error.message);
    };
    MdErrorBoxDirective.prototype.ngOnDestroy = function () {
        if (this._errorsSubscription) {
            this._errorsSubscription.unsubscribe();
        }
    };
    return MdErrorBoxDirective;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdErrorBoxDirective.prototype, "title", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MdErrorBoxDirective.prototype, "message", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array),
    __metadata("design:paramtypes", [Array])
], MdErrorBoxDirective.prototype, "errors", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], MdErrorBoxDirective.prototype, "ok", void 0);
MdErrorBoxDirective = __decorate([
    core_1.Directive({
        selector: "md-error-box",
        exportAs: "mdErrorBox"
    }),
    __metadata("design:paramtypes", [message_box_1.MdMessageBoxService])
], MdErrorBoxDirective);
exports.MdErrorBoxDirective = MdErrorBoxDirective;
//# sourceMappingURL=error_box.js.map