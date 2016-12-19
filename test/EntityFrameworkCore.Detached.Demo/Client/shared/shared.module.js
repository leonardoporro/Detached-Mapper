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
var core_1 = require('@angular/core');
var common_1 = require('@angular/common');
var popover_1 = require("./components/popover/popover");
var select_1 = require("./components/select/select");
var option_1 = require("./components/select/option");
var MdlSharedModule = (function () {
    function MdlSharedModule() {
    }
    MdlSharedModule.forRoot = function () {
        return {
            ngModule: MdlSharedModule,
            providers: []
        };
    };
    MdlSharedModule = __decorate([
        core_1.NgModule({
            imports: [common_1.CommonModule],
            exports: [popover_1.MdlPopoverComponent, select_1.MdlSelectComponent, option_1.MdlOptionComponent],
            declarations: [popover_1.MdlPopoverComponent, select_1.MdlSelectComponent, option_1.MdlOptionComponent],
            providers: [
                select_1.MDL_SELECT_VALUE_ACCESSOR
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], MdlSharedModule);
    return MdlSharedModule;
}());
exports.MdlSharedModule = MdlSharedModule;
//# sourceMappingURL=shared.module.js.map