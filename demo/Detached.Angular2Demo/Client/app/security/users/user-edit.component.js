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
var router_1 = require("@angular/router");
var users_datasource_1 = require("./users.datasource");
var angular2localization_1 = require("angular2localization");
var UserEditComponent = (function (_super) {
    __extends(UserEditComponent, _super);
    function UserEditComponent(userSource, activatedRoute, localization) {
        _super.call(this, null, localization);
        this.userSource = userSource;
        this.activatedRoute = activatedRoute;
        this.localization = localization;
    }
    UserEditComponent.prototype.ngOnInit = function () {
        var key = this.activatedRoute.snapshot.params["id"];
        if (key !== "new") {
            this.userSource.load(key);
        }
    };
    UserEditComponent.prototype.save = function (frm) {
        if (frm.valid) {
            this.userSource.save()
                .subscribe(function (e) { return window.history.back(); });
        }
    };
    UserEditComponent.prototype.delete = function () {
        this.userSource.delete()
            .subscribe(function (e) { return window.history.back(); });
    };
    UserEditComponent = __decorate([
        core_1.Component({
            selector: "user-edit",
            template: require("./user-edit.component.html"),
            providers: [users_datasource_1.UserModelDataSource]
        }), 
        __metadata('design:paramtypes', [users_datasource_1.UserModelDataSource, router_1.ActivatedRoute, angular2localization_1.LocalizationService])
    ], UserEditComponent);
    return UserEditComponent;
}(angular2localization_1.Locale));
exports.UserEditComponent = UserEditComponent;
//# sourceMappingURL=user-edit.component.js.map