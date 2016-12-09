import { Input, Output, EventEmitter } from "@angular/core";

export class Selector<TModel> {
    public items: Array<Item<TModel>>;

    _source: Array<TModel>
    _selection: Array<TModel>

    trackBy: string = "id";

    get source(): Array<TModel> {
        return this._source;
    }
    set source(value: Array<TModel>) {
        this._source = value;
        this.items = value.map(item => new Item(item, false));
    }

    get selection(): Array<TModel> {
        return this._selection;
    }
    set selection(value: Array<TModel>) {
        this._selection = value;
        this.syncSelection();
    }

    selectionChange = new EventEmitter();

    syncSelection() {
        if (this.items && this.selection) {
            for (let item of this.items) {
                let key = item.model[this.trackBy];
                item.checked = this._selection.findIndex(s => s[this.trackBy] === key) > -1;
            }
        }
    }

    syncItem(index: number) {
        let item = this.items[index];
        let key = item.model[this.trackBy];

        let selIndex: number = this.selection.findIndex(s => s[this.trackBy] === key);
        if (!item.checked && selIndex > -1) {
            this.selection.splice(selIndex, 1);
            this.selectionChange.emit(this.selection);
        }
        else if (item.checked && selIndex == -1) {
            this.selection.push(item.model);
            this.selectionChange.emit(this.selection);
        }
    }
}

export class Item<TModel> {
    constructor(public model: TModel, public checked: boolean) {
    }
}