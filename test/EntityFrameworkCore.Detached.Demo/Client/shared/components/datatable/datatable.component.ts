import {
  Component,
  Input,
  Output,
  EventEmitter,
  ViewEncapsulation,
  ContentChild,
  ContentChildren,
  TemplateRef,
} from '@angular/core';
import { ColumnComponent } from "./column.component";


class Item {
    private _selected: boolean = false;

    constructor(public model: any, public parent: ItemsComponent) {
    }

    public get selected(): boolean {
        return this._selected;
    }
    public set selected(value: boolean) {
        this._selected = value;
        this.parent.updateSelection(this);
    }
}

class ItemsComponent {

    items: Array<Item> = [];

    selection: Array<any> = [];

    selectionChanged: EventEmitter<any> = new EventEmitter();

    allSelected: boolean;

    public set itemsSource(values: Array<any>) {
        this.items = values.map(v => new Item(v, this));
    }

    public updateSelection(item: Item) {
        if (!this.selection) {
            this.selection = [];
        }

        let existing = this.selection.findIndex(v => v === item.model);
        if (item.selected && existing < 0)
            this.selection.push(item.model);
        else if (!item.selected && existing >= 0)
            this.selection.splice(existing, 1);

        this.selectionChanged.emit(this.selection);
        this.allSelected = this.items.length == this.selection.length;
    }

    public selectAll() {
        let sel = this.allSelected;
        for (let item of this.items) {
            item.selected = sel;
        }
    }
}


@Component({
    selector: 'mdl-datatable',
    template: require("./datatable.component.html"),
    encapsulation: ViewEncapsulation.None,
    inputs: ["itemsSource", "selection"]
})
export class DataTableComponent extends ItemsComponent{
    

    @ContentChildren(ColumnComponent)
    columns: Array<ColumnComponent>;
}
