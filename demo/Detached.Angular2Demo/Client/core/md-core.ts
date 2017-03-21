// angular.
import { NgModule, ModuleWithProviders } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FlexLayoutModule } from "@angular/flex-layout";
import { MaterialModule } from "@angular/material";
import { LocaleModule, LocalizationModule } from "angular2localization";

// directives.
import { FormErrorMessageDirective } from "./common/directives/form_error";
import { ResizeEventsDirective } from "./common/directives/resize_events";
import { ContentDirective } from "./common/directives/content";

// components.
import { ItemComponent } from "./common/components/item";
import { ItemsControlComponent } from "./common/components/items_control";

// material components.
import { MdDataColumnComponent } from "./material/components/data_table/data_column";
import { MdDataTableComponent } from "./material/components/data_table/data_table";
import { MdDataListComponent } from "./material/components/data_list/data_list";
import { MdPageIndicatorComponent } from "./material/components/page_indicator/page_indicator";
import { MdBlockUIComponent } from "./material/components/block_ui/block_ui";
import { MdMessageBoxComponent } from "./material/components/message_box/message_box";
import { MdMessageBoxService } from "./material/services/message_box";
import { MdConfirmationBoxDirective } from "./material/directives/confirmation_box";
import { MdErrorBoxDirective } from "./material/directives/error_box";

require("./md-core.css");

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
        ResizeEventsDirective,
        ContentDirective,
        ItemComponent, 

        MdConfirmationBoxDirective,
        MdErrorBoxDirective,
        MdMessageBoxComponent,
        MdDataColumnComponent,
        MdDataTableComponent,
        MdDataListComponent,
        MdPageIndicatorComponent,
        MdBlockUIComponent,
    ],
    declarations: [
        FormErrorMessageDirective,
        ResizeEventsDirective,
        ContentDirective,
        ItemComponent,

        MdConfirmationBoxDirective,
        MdErrorBoxDirective,
        MdDataColumnComponent,
        MdDataTableComponent,
        MdDataListComponent,
        MdPageIndicatorComponent,
        MdBlockUIComponent,
        MdMessageBoxComponent
    ],
    entryComponents: [
        MdMessageBoxComponent
    ],
    providers: [
        MdMessageBoxService
    ]
})
export class MdCoreModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: MdCoreModule,
            providers: []
        };
    }
}

export { MdMessageBoxService } from "./material/services/message_box";
export * from "./common/data/service";
export * from "./common/data/collection";
export * from "./common/data/model";
export * from "./common/data/selection";