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
var http_1 = require("@angular/http");
var md_core_1 = require("../../../core/md-core");
var User = (function () {
    function User() {
    }
    return User;
}());
exports.User = User;
var UserQuery = (function () {
    function UserQuery() {
    }
    return UserQuery;
}());
exports.UserQuery = UserQuery;
var UserService = (function (_super) {
    __extends(UserService, _super);
    function UserService(http) {
        return _super.call(this, http, "api/users") || this;
    }
    return UserService;
}(md_core_1.EntityService));
UserService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], UserService);
exports.UserService = UserService;
var UserCollection = (function (_super) {
    __extends(UserCollection, _super);
    function UserCollection(userService) {
        return _super.call(this, userService) || this;
    }
    return UserCollection;
}(md_core_1.Collection));
UserCollection = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [UserService])
], UserCollection);
exports.UserCollection = UserCollection;
var UserModel = (function (_super) {
    __extends(UserModel, _super);
    function UserModel(userService) {
        return _super.call(this, userService) || this;
    }
    return UserModel;
}(md_core_1.Model));
UserModel = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [UserService])
], UserModel);
exports.UserModel = UserModel;
//# sourceMappingURL=users.services.js.map