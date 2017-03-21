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
var users_services_1 = require("./users.services");
var md_core_1 = require("../../../core/md-core");
var UserListComponent = (function () {
    function UserListComponent(userCollection) {
        //this.userSelection.keys.subscribe(u => {
        this.userCollection = userCollection;
        this.userSelection = new md_core_1.Selection();
        //    let k = "keys=" + JSON.stringify(u);
        //    window.history.replaceState(null, "Selection Change", "?" + k);
        //    let s = window.location.origin + window.location.pathname + "?" + k;
        //    window.history.replaceState(null, "SC", s);
        //});
    }
    UserListComponent.prototype.ngOnInit = function () {
        this.userCollection.params.pageSize = 5;
        this.userCollection.load();
    };
    return UserListComponent;
}());
UserListComponent = __decorate([
    core_1.Component({
        selector: "user-list",
        template: require("./user-list.component.html"),
        providers: [users_services_1.UserCollection, users_services_1.UserService]
    }),
    __metadata("design:paramtypes", [users_services_1.UserCollection])
], UserListComponent);
exports.UserListComponent = UserListComponent;
//# sourceMappingURL=user-list.component.js.map