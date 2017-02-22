import {
    Directive,
    Input,
    ContentChild,
    TemplateRef
} from "@angular/core";

export enum ColumnType { Text = 1, Number = 2, Template = 3}

@Directive({
    selector: "d-column"
})
export class ColumnComponent {

    @Input()
    public div: any;

    @Input()
    public title: string;

    @Input()
    public type: ColumnType;

    @Input()
    public property: string;

    @Input()
    public canSort: boolean;

    @Input()
    public size: string;

    @Input()
    public minWidth: number;

    public clientSize: string;

    public actualSize: number;

    @Input()
    public priority: number;

    @Input()
    public visible: boolean;

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;

    public sizeChanged(nativeElement: any): void {
        this.actualSize = nativeElement.clientWidth;
    }
}