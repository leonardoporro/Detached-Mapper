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
var http_1 = require("@angular/http");
require('rxjs/Rx');
var UserService = (function () {
    function UserService(http) {
        this.http = http;
        this.baseUrl = "/api/users";
    }
    UserService.prototype.findById = function (id) {
        return this.http.get(this.baseUrl + "/" + id).map(function (r) { return r.json(); });
    };
    UserService.prototype.get = function (query) {
        var params = new http_1.URLSearchParams();
        if (query) {
            for (var _i = 0, query_1 = query; _i < query_1.length; _i++) {
                var prop = query_1[_i];
                params.append(prop, query[prop]);
            }
        }
        return this.http.get(this.baseUrl, { search: params }).map(function (r) { return r.json(); });
    };
    UserService.prototype.getPage = function (pageIndex, pageSize, query) {
        var params = new http_1.URLSearchParams();
        params.append("pageIndex", pageIndex.toString());
        params.append("pageSize", pageSize.toString());
        for (var _i = 0, query_2 = query; _i < query_2.length; _i++) {
            var prop = query_2[_i];
            params.append(prop, query[prop]);
        }
        return this.http.get(this.baseUrl, { search: params }).map(function (r) { return r.json(); });
    };
    UserService.prototype.save = function (entity) {
        return this.http.post(this.baseUrl, entity).map(function (r) { return r.json(); });
    };
    UserService.prototype.delete = function (id) {
        return this.http.delete(this.baseUrl + "/" + id);
    };
    UserService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http])
    ], UserService);
    return UserService;
}());
exports.UserService = UserService;
//# sourceMappingURL=user.service.js.map