import {
    Component,
    Input,
    Output,
    EventEmitter,
    TemplateRef,
} from '@angular/core';

export class Item {
    constructor(public model: any) {
    }

    private _selected: boolean = false;
    public get selected(): boolean {
        return this._selected;
    }
    public set selected(value: boolean) {
        if (this._selected !== value) {
            this._selected = value;
            this.selectedChange.emit(this);
        }
    }
    public selectedChange: EventEmitter<Item> = new EventEmitter();

    public updateSelected(value: boolean) {
        this._selected = value;
    }
}

export class ItemsComponent {

    protected _itemsSource: Array<any>;
    protected _items: Array<Item> = [];
    protected _selection: Array<any> = [];
    protected _allSelected: boolean;
    protected _suspendFlag: boolean = false;

    @Input()
    public valueProperty: string;
    @Input()
    public displayProperty: string;

    @Input()
    public get items(): Array<Item> {
        return this._items;
    }
    public set items(value: Array<Item>) {
        if (this._items !== value) {
            if (this._items) {
                for (let item of this._items) {
                    item.selectedChange.unsubscribe();
                }
            }
            this._items = value;
            if (this._items) {
                for (let item of this._items) {
                    item.selectedChange.subscribe(this.itemSelectedChanged.bind(this));
                }
            }
            this.syncSelection();
            this.itemsChange.emit(this._items);
        }
    }
    @Output()
    public itemsChange = new EventEmitter();

    @Input()
    public get itemsSource(): Array<any> {
        return this._items;
    }
    public set itemsSource(values: Array<any>) {
        if (this._itemsSource !== values) {
            this._itemsSource = values;
            this.items = values.map(v => new Item(v));
        }
    }

    @Input()
    public get selection(): Array<any> {
        return this._selection;
    }
    public set selection(value: Array<any>) {
        if (this._selection !== value) {
            this._selection = value;
            this.syncSelection();
            this.selectionChange.emit(this._selection);
        }
    }
    @Output()
    public selectionChange = new EventEmitter();

    @Input()
    public get allSelected(): boolean {
        return this._allSelected;
    }
    public set allSelected(value: boolean) {
        if (this._allSelected !== value) {
            this._allSelected = value;

            this._suspendFlag = true;
            this._selection = [];
            if (value) {
                for (let item of this._items) {
                    item.selected = true;
                    this._selection.push(item.model);
                }
            } else {
                for (let item of this._items) {
                    item.selected = false;
                }
            }
            this.selectionChange.emit(this._selection);
            this.allSelectedChange.emit(this._allSelected);

            this._suspendFlag = false;
        }
    }
    @Output()
    public allSelectedChange: EventEmitter<boolean> = new EventEmitter();

    protected itemSelectedChanged(item: Item) {
        if (!this._suspendFlag) {
            if (!this._selection) {
                this._selection = [];
            }

            let existing = this._selection.findIndex(s => s[this.valueProperty] == item.model[this.valueProperty])
            if (item.selected && existing < 0)
                this._selection.push(item.model);
            else if (!item.selected && existing >= 0)
                this._selection.splice(existing, 1);

            this.selectionChange.emit(this._selection);
            this.syncAllSelected();
        }
    }

    protected syncSelection() {
        this._suspendFlag = true;
        if (this._items && this._selection) {
            for (let item of this._items) {
                item.selected = this._selection.find(s => item.model[this.valueProperty] == s[this.valueProperty]) !== undefined;
            }
            this.syncAllSelected();
        }
        this._suspendFlag = false;
    }

    protected syncAllSelected() {
        let allSelected = this._items.length == this._selection.length;
        if (this._allSelected !== allSelected) {
            this._allSelected = allSelected;
            this.allSelectedChange.emit(allSelected);
        }
    }
}