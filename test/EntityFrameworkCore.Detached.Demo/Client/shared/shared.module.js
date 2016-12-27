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
var common_1 = require("@angular/common");
var forms_1 = require("@angular/forms");
var flex_layout_1 = require("@angular/flex-layout");
var material_1 = require("@angular/material");
var popover_component_1 = require("./components/popover/popover.component");
var select_component_1 = require("./components/select/select.component");
var option_component_1 = require("./components/select/option.component");
var data_table_component_1 = require("./components/data-table/data-table.component");
var data_column_component_1 = require("./components/data-table/data-column.component");
var presenter_directive_1 = require("./directives/presenter/presenter.directive");
var SharedModule = (function () {
    function SharedModule() {
    }
    SharedModule.forRoot = function () {
        return {
            ngModule: SharedModule,
            providers: []
        };
    };
    SharedModule = __decorate([
        core_1.NgModule({
            imports: [
                common_1.CommonModule,
                forms_1.FormsModule,
                flex_layout_1.FlexLayoutModule,
                material_1.MaterialModule
            ],
            exports: [
                presenter_directive_1.PresenterDirective,
                popover_component_1.PopoverComponent,
                select_component_1.SelectComponent,
                option_component_1.OptionComponent,
                data_table_component_1.DataTableComponent,
                data_column_component_1.DataColumnComponent
            ],
            declarations: [
                presenter_directive_1.PresenterDirective,
                popover_component_1.PopoverComponent,
                select_component_1.SelectComponent,
                option_component_1.OptionComponent,
                data_table_component_1.DataTableComponent,
                data_column_component_1.DataColumnComponent
            ],
            providers: [
                select_component_1.MDL_SELECT_VALUE_ACCESSOR
            ]
        }), 
        __metadata('design:paramtypes', [])
    ], SharedModule);
    return SharedModule;
}());
exports.SharedModule = SharedModule;
//# sourceMappingURL=shared.module.js.map