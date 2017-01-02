import {
    Component,
    Directive,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ContentChild,
    TemplateRef,
    ViewContainerRef
} from "@angular/core";

export enum ColumnOrder {
    None, Asc, Desc
}

@Directive({
    selector: "column"
})
export class ColumnDirective {

    constructor(private viewContainer: ViewContainerRef) {
    }

    @Input()
    public title: string;

    @Input()
    public type: string = "text";

    @Input()
    public property: string;

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;

    private _order: ColumnOrder = ColumnOrder.None;
    @Input()
    public get order(): ColumnOrder {
        return this._order;
    }
    public set order(value: ColumnOrder) {
        this._order = value;
        this.orderChanged.emit(this);
    }

    @Output()
    public orderChanged: EventEmitter<ColumnDirective> = new EventEmitter();

    public toggleOrder() {
        switch (this._order) {
            case ColumnOrder.None:
                this.order = ColumnOrder.Asc;
                break;
            case ColumnOrder.Asc:
                this.order = ColumnOrder.Desc;
                break;
            case ColumnOrder.Desc:
                this.order = ColumnOrder.Asc;
                break;
        }
    }
}