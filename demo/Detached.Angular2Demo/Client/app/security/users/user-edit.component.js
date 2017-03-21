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
var router_1 = require("@angular/router");
var users_services_1 = require("./users.services");
var md_core_1 = require("../../../core/md-core");
var UserEditComponent = (function () {
    function UserEditComponent(userModel, activatedRoute, messageBoxService) {
        this.userModel = userModel;
        this.activatedRoute = activatedRoute;
        this.messageBoxService = messageBoxService;
    }
    UserEditComponent.prototype.ngOnInit = function () {
        var key = this.activatedRoute.snapshot.params["id"];
        if (key !== "new") {
            this.userModel.load(key);
        }
        //this.rolesSource.load();
    };
    UserEditComponent.prototype.save = function (frm) {
        var _this = this;
        if (frm.valid) {
            this.userModel.save()
                .subscribe(function (ok) { return _this.close(); }, function (error) {
                if (error.memberErrors) {
                    for (var member in error.memberErrors) {
                        var ctrl = frm.controls[member];
                        ctrl.setErrors({ server: error.memberErrors[member] });
                    }
                }
            });
        }
    };
    UserEditComponent.prototype.delete = function () {
        //this.userSource.delete()
        //    .subscribe(ok => this.close());
    };
    UserEditComponent.prototype.close = function () {
        window.history.back();
    };
    return UserEditComponent;
}());
UserEditComponent = __decorate([
    core_1.Component({
        selector: "user-edit",
        template: require("./user-edit.component.html"),
        providers: [users_services_1.UserModel, users_services_1.UserService, md_core_1.MdMessageBoxService]
    }),
    __metadata("design:paramtypes", [users_services_1.UserModel,
        router_1.ActivatedRoute,
        md_core_1.MdMessageBoxService])
], UserEditComponent);
exports.UserEditComponent = UserEditComponent;
//# sourceMappingURL=user-edit.component.js.map