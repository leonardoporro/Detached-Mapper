import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ContentChild,
    ContentChildren,
    QueryList,
    TemplateRef,
} from '@angular/core';
import { CollectionComponent } from "../collection/collection.component";
import { SortDirection } from "../../datasources/collection.datasource";

@Component({
    selector: 'd-list',
    template: require("./list.component.html"),
    encapsulation: ViewEncapsulation.None,
    styles: [require("./list.component.scss")],
})
export class ListComponent extends CollectionComponent {

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;

    @Input()
    public placeholder: string;
}