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
    selector: "mdl-column",
    template: "<span></span>",
    encapsulation: ViewEncapsulation.None,
})
export class ColumnComponent {

    constructor(private viewContainer: ViewContainerRef) {
        
    }

    @Input()
    public title: string;

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;
}