import { Directive,  Input, TemplateRef, ViewContainerRef, ChangeDetectorRef } from '@angular/core';
import { Item } from "./items.component";

@Directive({
    selector: "item-content"
})
export class ItemContentDirective {

    constructor(private viewContainer: ViewContainerRef,
                private cdr: ChangeDetectorRef) { }

    @Input()
    public item: Item;

    @Input()
    public index: number;

    @Input()
    public template: TemplateRef<any>;

    ngOnInit() {
        this.cdr.detach();
    }

    ngAfterViewInit() {
        let view = this.viewContainer.createEmbeddedView(this.template);
        view.context.item = this.item;
        view.context.model = this.item.model;
        view.context.index = this.index;
        setTimeout(() => this.cdr.reattach());
    }
}