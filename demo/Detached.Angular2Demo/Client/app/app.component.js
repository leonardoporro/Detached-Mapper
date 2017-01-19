"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var angular2localization_1 = require("angular2localization");
var AppComponent = (function (_super) {
    __extends(AppComponent, _super);
    function AppComponent(locale, localization) {
        _super.call(this, locale, localization);
        this.locale = locale;
        this.localization = localization;
        this.locale.addLanguages(["en", "es"]);
        this.locale.definePreferredLocale("en", "US", 30);
        this.locale.definePreferredCurrency("USD");
        this.localization.translationProvider("./lang/");
        this.localization.updateTranslation();
    }
    AppComponent = __decorate([
        core_1.Component({
            selector: "app",
            template: require("./app.component.html")
        }), 
        __metadata('design:paramtypes', [angular2localization_1.LocaleService, angular2localization_1.LocalizationService])
    ], AppComponent);
    return AppComponent;
}(angular2localization_1.Locale));
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map