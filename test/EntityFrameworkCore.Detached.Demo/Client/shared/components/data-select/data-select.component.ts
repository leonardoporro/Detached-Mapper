import {
    Component,
    Input,
    Output,
    EventEmitter, ElementRef,
    ViewEncapsulation,
    ContentChild,
    ContentChildren,
    QueryList,
    TemplateRef,
} from "@angular/core";
import { FormControl } from "@angular/forms";
import { ColumnDirective, ColumnOrder } from "../core/column.directive";
import { ItemsComponent, Item } from "../core/items.component";
import { PopoverComponent } from "../popover/popover.component";

@Component({
    selector: 'md-data-select',
    template: require("./data-select.component.html"),
    encapsulation: ViewEncapsulation.None,
    styles: [require("./data-select.component.scss")],
})
export class DataSelectComponent extends ItemsComponent {

    constructor(private elementRef: ElementRef) {
        super();
    }

    @Input()
    public placeholder: string;

    @Input()
    public text: string;
    @Output()
    public textChange = new EventEmitter();

    ngOnInit() {
        this.selectionChange.subscribe(this.onSelectionChange.bind(this));
        let input = this.elementRef.nativeElement.querySelector("input");
        input.readOnly = true;
    }

    public onSelectionChange() {
        this.text = "";
        for (let selected of this._selection) {
            if (this.text !== "")
                this.text += ", ";

            this.text += selected[this.displayProperty];
        }
        this.textChange.emit(this.text);
    }
}