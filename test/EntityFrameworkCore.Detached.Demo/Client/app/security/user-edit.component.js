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
var router_1 = require("@angular/router");
var user_service_1 = require("./user.service");
var UserEditComponent = (function () {
    function UserEditComponent(userSource, activatedRoute) {
        this.userSource = userSource;
        this.activatedRoute = activatedRoute;
    }
    UserEditComponent.prototype.ngOnInit = function () {
        var key = this.activatedRoute.snapshot.params["id"];
        this.userSource.load(key);
    };
    UserEditComponent.prototype.save = function () {
        this.userSource.save()
            .subscribe(function (e) { return window.history.back(); });
    };
    UserEditComponent.prototype.delete = function () {
        this.userSource.delete()
            .subscribe(function (e) { return window.history.back(); });
    };
    UserEditComponent.prototype.do = function (form) {
    };
    UserEditComponent = __decorate([
        core_1.Component({
            selector: "user-edit",
            template: require("./user-edit.component.html"),
            providers: [user_service_1.UserModelDataSource]
        }), 
        __metadata('design:paramtypes', [user_service_1.UserModelDataSource, router_1.ActivatedRoute])
    ], UserEditComponent);
    return UserEditComponent;
}());
exports.UserEditComponent = UserEditComponent;
//# sourceMappingURL=user-edit.component.js.map