import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MdlPopoverComponent } from "./components/popover/popover";
import { MdlSelectComponent, MDL_SELECT_VALUE_ACCESSOR } from "./components/select/select";
import { MdlOptionComponent } from "./components/select/option";

@NgModule({
    imports: [CommonModule],
    exports: [MdlPopoverComponent, MdlSelectComponent, MdlOptionComponent],
    declarations: [MdlPopoverComponent, MdlSelectComponent, MdlOptionComponent],
    providers: [
        MDL_SELECT_VALUE_ACCESSOR
    ]
})
export class MdlSharedModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: MdlSharedModule,
            providers: []
        };
    }
}