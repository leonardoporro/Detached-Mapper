// angular.
import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from "@angular/material";
import { LocaleModule, LocalizationModule } from "angular2localization";

// components.
import { FormErrorMessageDirective } from "./directives/form-errormsg.directive";
import { OnSizeChangedDirective } from "./directives/size-changed.directive";
import { ContentDirective } from "./directives/content.directive";
import { ColumnComponent } from "./components/table/column.component";
import { TableComponent } from "./components/table/table.component";
import { ListComponent } from "./components/list/list.component";

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
        OnSizeChangedDirective,
        ContentDirective,
        ColumnComponent,
        TableComponent,
        ListComponent
    ],
    declarations: [
        FormErrorMessageDirective,
        OnSizeChangedDirective,
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