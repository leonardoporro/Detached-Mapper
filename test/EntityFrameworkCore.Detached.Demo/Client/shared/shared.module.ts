import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PopoverComponent } from "./components/popover/popover.component";
import { SelectComponent, MDL_SELECT_VALUE_ACCESSOR } from "./components/select/select.component";
import { OptionComponent } from "./components/select/option.component";

@NgModule({
    imports: [CommonModule],
    exports: [
        PopoverComponent,
        SelectComponent,
        OptionComponent
    ],
    declarations: [PopoverComponent, SelectComponent, OptionComponent],
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