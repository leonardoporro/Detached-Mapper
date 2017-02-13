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
var http_1 = require("@angular/http");
var core_module_1 = require("../../../core/core.module");
var core_module_2 = require("../../../core/core.module");
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
var UserCollectionDataSource = (function (_super) {
    __extends(UserCollectionDataSource, _super);
    function UserCollectionDataSource(http) {
        _super.call(this, http, "/api/users");
        this.sortBy = "name";
    }
    UserCollectionDataSource = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], UserCollectionDataSource);
    return UserCollectionDataSource;
}(core_module_1.HttpRestCollectionDataSource));
exports.UserCollectionDataSource = UserCollectionDataSource;
var UserModelDataSource = (function (_super) {
    __extends(UserModelDataSource, _super);
    function UserModelDataSource(http) {
        _super.call(this, http, "api/users/:id");
        this.requestParams.culture = "es-AR";
    }
    UserModelDataSource = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], UserModelDataSource);
    return UserModelDataSource;
}(core_module_2.HttpRestModelDataSource));
exports.UserModelDataSource = UserModelDataSource;
//# sourceMappingURL=users.datasource.js.map