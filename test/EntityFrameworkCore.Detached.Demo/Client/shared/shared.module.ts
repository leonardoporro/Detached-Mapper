import { ModuleWithProviders, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { MdlModule } from "angular2-mdl";

import { PopoverComponent } from "./components/popover/popover.component";
import { SelectComponent, MDL_SELECT_VALUE_ACCESSOR } from "./components/select/select.component";
import { OptionComponent } from "./components/select/option.component";

import { DataTableComponent } from "./components/datatable/datatable.component";
import { ColumnComponent } from "./components/datatable/column.component";

import { PresenterDirective } from "./directives/presenter/presenter.directive";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        MdlModule
    ],
    exports: [
        PresenterDirective,
        PopoverComponent,
        SelectComponent,
        OptionComponent,
        DataTableComponent,
        ColumnComponent
    ],
    declarations: [
        PresenterDirective,
        PopoverComponent,
        SelectComponent,
        OptionComponent,
        DataTableComponent,
        ColumnComponent],
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