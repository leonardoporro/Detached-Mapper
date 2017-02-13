// angular.
import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from "@angular/material";
import { LocaleModule, LocalizationModule } from "angular2localization";

// components.
import { FormErrorMessageDirective } from "./directives/form-errormsg.directive";
import { ContentDirective } from "./directives/content.directive";
import { ColumnComponent } from "./components/table/column.component";
import { TableComponent } from "./components/table/table.component";
import { ListComponent } from "./components/list/list.component";

require("./core-styles.scss");

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        FlexLayoutModule,
        MaterialModule,
        LocaleModule,
        LocalizationModule
    ],
    exports: [
        FormErrorMessageDirective,
        ContentDirective,
        ColumnComponent,
        TableComponent,
        ListComponent
    ],
    declarations: [
        FormErrorMessageDirective,
        ContentDirective,
        ColumnComponent,
        TableComponent,
        ListComponent
    ],
    providers: [
        /*MDL_SELECT_VALUE_ACCESSOR*/
    ]
})
export class CoreModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: CoreModule,
            providers: []
        };
    }
}

export * from "./datasources/collection.datasource";
export * from "./datasources/model.datasource";