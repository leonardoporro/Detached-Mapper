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
function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
// angular.
var core_1 = require("@angular/core");
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var flex_layout_1 = require("@angular/flex-layout");
var material_1 = require("@angular/material");
var angular2localization_1 = require("angular2localization");
// components.
var form_errormsg_directive_1 = require("./directives/form-errormsg.directive");
var size_changed_directive_1 = require("./directives/size-changed.directive");
var content_directive_1 = require("./directives/content.directive");
var column_component_1 = require("./components/table/column.component");
var table_component_1 = require("./components/table/table.component");
var list_component_1 = require("./components/list/list.component");
var CoreModule = (function () {
    function CoreModule() {
    }
    CoreModule.forRoot = function () {
        return {
            ngModule: CoreModule,
            providers: []
        };
    };
    CoreModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule,
                forms_1.FormsModule,
                flex_layout_1.FlexLayoutModule,
                material_1.MaterialModule,
                angular2localization_1.LocaleModule,
                angular2localization_1.LocalizationModule
            ],
            exports: [
                form_errormsg_directive_1.FormErrorMessageDirective,
                size_changed_directive_1.OnSizeChangedDirective,
                content_directive_1.ContentDirective,
                column_component_1.ColumnComponent,
                table_component_1.TableComponent,
                list_component_1.ListComponent
            ],
            declarations: [
                form_errormsg_directive_1.FormErrorMessageDirective,
                size_changed_directive_1.OnSizeChangedDirective,
                content_directive_1.ContentDirective,
                column_component_1.ColumnComponent,
                table_component_1.TableComponent,
                list_component_1.ListComponent
            ],
            providers: []
        }), 
        __metadata('design:paramtypes', [])
    ], CoreModule);
    return CoreModule;
}());
exports.CoreModule = CoreModule;
__export(require("./datasources/collection.datasource"));
__export(require("./datasources/model.datasource"));
//# sourceMappingURL=core.module.js.map