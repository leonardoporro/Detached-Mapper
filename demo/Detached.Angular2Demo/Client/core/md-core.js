"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
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
// directives.
var form_error_1 = require("./common/directives/form_error");
var resize_events_1 = require("./common/directives/resize_events");
var content_1 = require("./common/directives/content");
// components.
var item_1 = require("./common/components/item");
// material components.
var data_column_1 = require("./material/components/data_table/data_column");
var data_table_1 = require("./material/components/data_table/data_table");
var data_list_1 = require("./material/components/data_list/data_list");
var page_indicator_1 = require("./material/components/page_indicator/page_indicator");
var block_ui_1 = require("./material/components/block_ui/block_ui");
var message_box_1 = require("./material/components/message_box/message_box");
var message_box_2 = require("./material/services/message_box");
var confirmation_box_1 = require("./material/directives/confirmation_box");
var error_box_1 = require("./material/directives/error_box");
require("./md-core.css");
var MdCoreModule = MdCoreModule_1 = (function () {
    function MdCoreModule() {
    }
    MdCoreModule.forRoot = function () {
        return {
            ngModule: MdCoreModule_1,
            providers: []
        };
    };
    return MdCoreModule;
}());
MdCoreModule = MdCoreModule_1 = __decorate([
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
            form_error_1.FormErrorMessageDirective,
            resize_events_1.ResizeEventsDirective,
            content_1.ContentDirective,
            item_1.ItemComponent,
            confirmation_box_1.MdConfirmationBoxDirective,
            error_box_1.MdErrorBoxDirective,
            message_box_1.MdMessageBoxComponent,
            data_column_1.MdDataColumnComponent,
            data_table_1.MdDataTableComponent,
            data_list_1.MdDataListComponent,
            page_indicator_1.MdPageIndicatorComponent,
            block_ui_1.MdBlockUIComponent,
        ],
        declarations: [
            form_error_1.FormErrorMessageDirective,
            resize_events_1.ResizeEventsDirective,
            content_1.ContentDirective,
            item_1.ItemComponent,
            confirmation_box_1.MdConfirmationBoxDirective,
            error_box_1.MdErrorBoxDirective,
            data_column_1.MdDataColumnComponent,
            data_table_1.MdDataTableComponent,
            data_list_1.MdDataListComponent,
            page_indicator_1.MdPageIndicatorComponent,
            block_ui_1.MdBlockUIComponent,
            message_box_1.MdMessageBoxComponent
        ],
        entryComponents: [
            message_box_1.MdMessageBoxComponent
        ],
        providers: [
            message_box_2.MdMessageBoxService
        ]
    })
], MdCoreModule);
exports.MdCoreModule = MdCoreModule;
var message_box_3 = require("./material/services/message_box");
exports.MdMessageBoxService = message_box_3.MdMessageBoxService;
__export(require("./common/data/service"));
__export(require("./common/data/collection"));
__export(require("./common/data/model"));
__export(require("./common/data/selection"));
var MdCoreModule_1;
//# sourceMappingURL=md-core.js.map