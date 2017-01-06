import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from "@angular/material";

import { ContentDirective } from "./directives/content.directive";
import { ColumnComponent } from "./components/table/column.component";
import { TableComponent } from "./components/table/table.component";

require("./material-icons.scss");
require("./material-theme.scss");

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        FlexLayoutModule,
        MaterialModule
    ],
    exports: [
        ContentDirective,
        ColumnComponent,
        TableComponent
    ],
    declarations: [
        ContentDirective,
        ColumnComponent,
        TableComponent
    ],
    providers: [
        /*MDL_SELECT_VALUE_ACCESSOR*/
    ]
})
export class SharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: []
        };
    }
}