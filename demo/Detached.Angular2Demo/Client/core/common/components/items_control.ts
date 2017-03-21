import {
    Component, Directive, ChangeDetectorRef,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    AfterViewInit,
    ElementRef,
    ContentChildren, ViewChildren, OnChanges, SimpleChange, SimpleChanges, OnInit, OnDestroy,
    QueryList
} from '@angular/core';

import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import { ICollection, IQueryParams } from "../data/collection";
import { ISelection, Selection } from "../data/selection";

import { ItemComponent } from "./item";

export class ItemsControlComponent implements AfterViewInit, OnDestroy {

    private _itemSelectedSubscription: Subscription;
    private _modelSelectedSubscription: Subscription;
    private _selectionSubscription: Subscription;
    private _allSelectedSubscription: Subscription;

    @ViewChildren(ItemComponent)
    public items: QueryList<ItemComponent>;

    private _collection: ICollection<any, IQueryParams>;
    @Input()
    public get collection(): ICollection<any, IQueryParams> {
        return this._collection;
    }
    public set collection(value: ICollection<any, IQueryParams>) {
        this._collection = value;
        this._subscribeAllSelectedChange();
    }

    private _selection: ISelection<any>;
    @Input()
    public get selection(): ISelection<any> {
        return this._selection;
    }
    public set selection(value: ISelection<any>) {
        this._selection = value;
        this._subscribeAllSelectedChange();
        this._subscribeSelection();
    }

    private _allSelected: boolean = false;
    @Input()
    public get allSelected(): boolean {
        return this._allSelected;
    }
    public set allSelected(value: boolean) {
        if (this._allSelected != value) {
            if (this._selection && this._collection) {
                this._allSelected = value;
                if (value) {
                    this._selection.replace(this._collection.getValues());
                } else {
                    this._selection.clear();
                }
            }
        }
    }

    private _selectedItems: any[];
    @Input()
    public get selectedItems() {
        return this._selectedItems;
    }
    public set selectedItems(value: any[]) {
        if (value != this._selectedItems) {
            if (this._selection) {
                this._selectedItems = value;
                this._selection.replace(value);
                this.selectedItemsChange.emit(value);
            }
        }
    }

    @Output()
    public selectedItemsChange: EventEmitter<any[]> = new EventEmitter();

    public ngAfterViewInit() {

        if (!this.selection) {
            this.selection = new Selection();
        }

        this._subscribeItemChanges();
    }

    private _subscribeAllSelectedChange() {

        if (this._allSelectedSubscription)
            this._allSelectedSubscription.unsubscribe();

        if (this._collection && this._selection) {
            this._allSelectedSubscription = Observable.combineLatest(this._collection.values, this._selection.values)
                .subscribe((sources: Array<Array<any>>) => this._onAllSelectedChange(sources[0], sources[1]));
        }
    }

    private _onAllSelectedChange(collectionValues: any[], selectionValues: any[]) {
        let result = false;
        if (collectionValues && selectionValues) {
            if (collectionValues.length > 0 && collectionValues.length >= selectionValues.length) {
                result = true;
                for (let item of collectionValues) {
                    if (!this.selection.has(item)) {
                        result = false;
                        break;
                    }
                }
            }
        }
        this._allSelected = result;
    }

    private _subscribeItemChanges() {
        if (this.items) {
            this.items.changes.startWith([])
                .subscribe(items => {
                    this._subscribeItemsModelChange(items);
                    this._subscribeItemsSelectedChange(items);
                });
        }
    }

    private _subscribeItemsSelectedChange(items: ItemComponent[]) {

        if (this._itemSelectedSubscription) {
            this._itemSelectedSubscription.unsubscribe();
        }

        this._itemSelectedSubscription = Observable.merge(...items.map(r => r.selectedChange))
            .subscribe(this._onItemSelectedChange.bind(this));
    }

    private _onItemSelectedChange(item: ItemComponent) {
        if (item.selected) {
            this.selection.add(item.model);
        } else {
            this.selection.remove(item.model);
        }
    }

    private _subscribeItemsModelChange(items: ItemComponent[]) {
        if (this._modelSelectedSubscription) {
            this._modelSelectedSubscription.unsubscribe();
        }

        this._modelSelectedSubscription = Observable.merge(...items.map(r => r.modelChange))
            .subscribe(this._onItemModelChange.bind(this));
    }

    private _onItemModelChange(item: ItemComponent) {
        item.selected = this._selection.has(item.model);
    }



    private _subscribeSelection() {
        this._selectionSubscription = this.selection.values
            .subscribe(this._onSelectionChange.bind(this));
    }

    private _onSelectionChange(values: any[]) {
        if (this.items) {
            this.items.forEach(row => {
                row.selected = this.selection.has(row.model);
            });
        }
        this._selectedItems = values;
        this.selectedItemsChange.emit(values);
    }

    public ngOnDestroy() {

        if (this._itemSelectedSubscription) {
            this._itemSelectedSubscription.unsubscribe();
        }

        if (this._modelSelectedSubscription) {
            this._modelSelectedSubscription.unsubscribe();
        }

        if (this._selectionSubscription) {
            this._selectionSubscription.unsubscribe();
        }

        if (this._allSelectedSubscription) {
            this._allSelectedSubscription.unsubscribe();
        }
    }
} 