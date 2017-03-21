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
var angular2localization_1 = require("angular2localization");
var AppComponent = (function (_super) {
    __extends(AppComponent, _super);
    function AppComponent(locale, localization) {
        var _this = _super.call(this, locale, localization) || this;
        _this.locale = locale;
        _this.localization = localization;
        _this.mode = "over";
        _this.open = false;
        _this.locale.addLanguages(["en", "es"]);
        _this.locale.definePreferredLocale("en", "US", 30);
        _this.locale.definePreferredCurrency("USD");
        _this.localization.translationProvider("./lang/res_");
        _this.localization.updateTranslation();
        _this.adjustSideNav();
        window.onresize = function () { return _this.adjustSideNav(); };
        return _this;
    }
    AppComponent.prototype.adjustSideNav = function () {
        if (window.innerWidth > 800) {
            this.mode = "side";
            this.open = true;
        }
        else {
            this.mode = "over";
            this.open = false;
        }
    };
    return AppComponent;
}(angular2localization_1.Locale));
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], AppComponent.prototype, "mode", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], AppComponent.prototype, "open", void 0);
AppComponent = __decorate([
    core_1.Component({
        selector: "app",
        template: require("./app.component.html")
    }),
    __metadata("design:paramtypes", [angular2localization_1.LocaleService, angular2localization_1.LocalizationService])
], AppComponent);
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map