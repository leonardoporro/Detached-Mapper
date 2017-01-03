import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from "@angular/material";

import { PopoverComponent } from "./components/popover/popover.component";
import { ItemContentDirective } from "./components/core/item-content.directive";
import { ColumnDirective } from "./components/core/column.directive";

import { DataSelectComponent/*, MDL_SELECT_VALUE_ACCESSOR*/ } from "./components/data-select/data-select.component";
import { DataTableComponent } from "./components/data-table/data-table.component";



@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        FlexLayoutModule,
        MaterialModule
    ],
    exports: [
        ItemContentDirective,
        ColumnDirective,
        PopoverComponent,
        DataSelectComponent,
        DataTableComponent
    ],
    declarations: [
        ItemContentDirective,
        ColumnDirective,
        PopoverComponent,
        DataSelectComponent,
        DataTableComponent
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