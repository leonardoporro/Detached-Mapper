import {
    Directive,
    Input,
    Output,
    EventEmitter,
    ContentChild,
    TemplateRef,
    ViewContainerRef
} from "@angular/core";
import { SortDirection } from "../../datasources/collection.datasource";


@Directive({
    selector: "d-column"
})
export class ColumnComponent {

    constructor(private viewContainer: ViewContainerRef) {
    }

    @Input()
    public title: string;

    @Input()
    public type: string = "text";

    @Input()
    public property: string;

    @Input()
    public canSort: boolean = false;

    @Input()
    public width: string;

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;
}