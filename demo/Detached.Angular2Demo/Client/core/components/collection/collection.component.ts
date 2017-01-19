import { Component, Input, Output, EventEmitter, TemplateRef } from '@angular/core';
import { ICollectionDataSource, SortDirection } from "../../datasources/collection.datasource";

export class CollectionItem {
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

    public selectedChange: EventEmitter<CollectionItem> = new EventEmitter();
}

export class CollectionComponent {
    protected _suspendFlag: boolean = false;

    protected _dataSource: ICollectionDataSource<any>;
    protected _items: Array<CollectionItem> = [];
    protected _selection: Array<any> = [];
    protected _allSelected: boolean;
    protected _orderBy: string;
    protected _sortDirection: SortDirection;

    @Input()
    public keyProperty: string = "id";

    @Input()
    public displayProperty: string = "name";

    //items.
    @Input()
    public get items(): Array<CollectionItem> {
        return this._items;
    }
    public set items(value: Array<CollectionItem>) {
        if (this._items !== value) {
            if (this._items) {
                for (let item of this._items) {
                    item.selectedChange.unsubscribe();
                }
            }
            this._items = value;
            if (this._items) {
                for (let item of this._items) {
                    item.selectedChange.subscribe(this.onItemSelectedChange.bind(this));
                }
            }
            this.syncSelection();
            this.itemsChange.emit(this._items);
        }
    }
    @Output()
    public itemsChange = new EventEmitter();

    //item source.
    @Input()
    public get dataSource(): ICollectionDataSource<any> {
        return this._dataSource;
    }
    public set dataSource(value: ICollectionDataSource<any>) {
        if (this._dataSource != value) {
            if (this._dataSource) {
                this._dataSource.itemsChange.unsubscribe();
            }
            this._dataSource = value;
            if (this._dataSource) {
                this._dataSource.itemsChange.subscribe(this.onDataSourceItemsChange.bind(this));
                this.onDataSourceItemsChange();
            }
            this.dataSourceChanged.emit(this.dataSource);
        }
    }
    @Output()
    public dataSourceChanged = new EventEmitter();

    protected onDataSourceItemsChange() {
        if (this._dataSource && this._dataSource.items) {
            this.items = this._dataSource.items.map(i => new CollectionItem(i));
        }
    }

    // selection.
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

    protected syncSelection() {
        this._suspendFlag = true;
        if (this._items && this._selection) {
            for (let item of this._items) {
                item.selected = this._selection.find(s => item.model[this.keyProperty] == s[this.keyProperty]) !== undefined;
            }
            this.syncAllSelected();
        }
        this._suspendFlag = false;
    }

    protected onItemSelectedChange(item: CollectionItem) {
        if (!this._suspendFlag) {
            if (!this._selection) {
                this._selection = [];
            }

            let existing = this._selection.findIndex(s => s[this.keyProperty] == item.model[this.keyProperty])
            if (item.selected && existing < 0)
                this._selection.push(item.model);
            else if (!item.selected && existing >= 0)
                this._selection.splice(existing, 1);

            this.selectionChange.emit(this._selection);
            this.syncAllSelected();
        }
    }

    // all selected.
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

    protected syncAllSelected() {
        let allSelected = this._items.length == this._selection.length && this._items.length > 0;
        if (this._allSelected !== allSelected) {
            this._allSelected = allSelected;
            this.allSelectedChange.emit(allSelected);
        }
    }
}