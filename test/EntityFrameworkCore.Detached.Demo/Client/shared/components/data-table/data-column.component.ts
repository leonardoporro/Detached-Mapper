import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ContentChild,
    TemplateRef,
    ViewContainerRef
} from "@angular/core";

@Component({
    selector: "md-data-column",
    template: "<span></span>",
    encapsulation: ViewEncapsulation.None,
})
export class DataColumnComponent {

    constructor(private viewContainer: ViewContainerRef) {
    }

    @Input()
    public title: string;

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;
}