import {
    Directive,
    Input,
    ContentChild,
    TemplateRef
} from "@angular/core";

export enum ColumnType { Text = 1, Number = 2, Template = 3 }

@Directive({
    selector: "md-data-column"
})
export class MdDataColumnComponent {

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

    public index: number;

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;

    public onResize(nativeElement: any): void {
        this.actualSize = nativeElement.clientWidth;
    }

    public setup() {
        // default for canSort
        if (this.canSort == undefined && this.property) {
            this.canSort = true;
        }
        // default for type
        if (this.type == undefined) {
            if (this.template)
                this.type = ColumnType.Template;
            else
                this.type = ColumnType.Text;
        }
        // default for title
        if (this.title == undefined && this.property) {
            this.title = this.property;
        }
        // default for size
        if (this.size == undefined) {
            switch (this.type) {
                case ColumnType.Text:
                    this.size = "grow";
                    break;
                default:
                    this.size = "stretch";
                    break;
            }
        }
        // default for minWidth
        if (this.minWidth == undefined) {
            switch (this.type) {
                case ColumnType.Text:
                    this.minWidth = 125;
                    break;
                case ColumnType.Number:
                    this.minWidth = 50;
                    break;
                default:
                    this.minWidth = 50;
                    break;
            }
        }
        // default for priority
        if (this.priority == undefined) {
            if (this.type == ColumnType.Template)
                this.priority = 0;
            else
                this.priority = this.index;
        }
        // default for visible
        if (this.visible == undefined) {
            this.visible = true;
        }
    }
}