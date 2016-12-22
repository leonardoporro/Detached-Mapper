import {
    Directive,
    Input,
    TemplateRef,
    ViewContainerRef,
    ChangeDetectorRef
} from '@angular/core';

@Directive({
    selector: "presenter"
})
export class PresenterDirective {

    constructor(private viewContainer: ViewContainerRef,
                private cdr: ChangeDetectorRef) { }

    @Input()
    public item: any;

    @Input()
    public template: TemplateRef<any>

    ngOnInit() {
        this.cdr.detach();
    }

    ngAfterViewInit() {
        let view = this.viewContainer.createEmbeddedView(this.template);
        view.context.item = this.item;
        setTimeout(() => this.cdr.reattach());
    }
}