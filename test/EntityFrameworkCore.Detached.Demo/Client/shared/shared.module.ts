import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from "@angular/material";

import { PopoverComponent } from "./components/popover/popover.component";
import { SelectComponent, MDL_SELECT_VALUE_ACCESSOR } from "./components/select/select.component";
import { OptionComponent } from "./components/select/option.component";

import { DataTableComponent } from "./components/data-table/data-table.component";
import { DataColumnComponent } from "./components/data-table/data-column.component";

import { PresenterDirective } from "./directives/presenter/presenter.directive";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        FlexLayoutModule,
        MaterialModule
    ],
    exports: [
        PresenterDirective,
        PopoverComponent,
        SelectComponent,
        OptionComponent,
        DataTableComponent,
        DataColumnComponent
    ],
    declarations: [
        PresenterDirective,
        PopoverComponent,
        SelectComponent,
        OptionComponent,
        DataTableComponent,
        DataColumnComponent
    ],
    providers: [
        MDL_SELECT_VALUE_ACCESSOR
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