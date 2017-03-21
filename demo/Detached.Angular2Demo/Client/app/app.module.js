"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
// angular 2
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var http_1 = require("@angular/http");
var material_1 = require("@angular/material");
var flex_layout_1 = require("@angular/flex-layout");
var angular2localization_1 = require("angular2localization");
// app
var md_core_1 = require("../core/md-core");
var app_routing_module_1 = require("./app-routing.module");
var app_component_1 = require("./app.component");
// home
var home_component_1 = require("./home/home.component");
// user
var user_list_component_1 = require("./security/users/user-list.component");
var user_edit_component_1 = require("./security/users/user-edit.component");
require("./app-styles.css");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
            http_1.HttpModule,
            flex_layout_1.FlexLayoutModule.forRoot(),
            material_1.MaterialModule.forRoot(),
            angular2localization_1.LocaleModule.forRoot(),
            angular2localization_1.LocalizationModule.forRoot(),
            md_core_1.MdCoreModule,
            app_routing_module_1.AppRoutingModule,
        ],
        declarations: [
            app_component_1.AppComponent,
            home_component_1.HomeComponent,
            user_list_component_1.UserListComponent,
            user_edit_component_1.UserEditComponent
        ],
        bootstrap: [app_component_1.AppComponent]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map