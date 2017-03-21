import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { ICollection } from "./collection";
import "rxjs";

export interface ISelection<TModel> {
    values: Observable<TModel[]>;
    keys: Observable<TModel[]>;
    length: number;
    add(model: TModel);
    remove(model: TModel);
    has(model: TModel): boolean;
    toggle(model: TModel, selected?: boolean);
    clear();
    replace(model: TModel[]);
}

export class Selection<TModel> implements ISelection<TModel> {
    private _values: BehaviorSubject<TModel[]> = new BehaviorSubject([]);
    private _keys: BehaviorSubject<any[]> = new BehaviorSubject([]);
    private _map: Map<any, TModel> = new Map();

    public values: Observable<TModel[]> = this._values.asObservable();

    public keys: Observable<any[]> = this._keys.asObservable();

    public allSelected: Observable<boolean>;

    public get length(): number {
        return this._map.size;
    }

    public has(model: TModel): boolean {
        return this._map.has(this.getKey(model));
    }

    public add(model: TModel) {
        let key: any = this.getKey(model);
        if (!this._map.has(key)) {
            this._map.set(key, model);
            this._nextValues();
        }
    }

    public remove(model: TModel) {
        if (this._map.delete(this.getKey(model))) {
            this._nextValues();
        }
    }

    public toggle(model: TModel, selected?: boolean) {
        if (selected == undefined) {
            selected = this.has(model);
        }

        if (selected)
            this.add(model);
        else
            this.remove(model);
    }

    public clear() {
        if (this._map.size > 0) {
            this._map.clear();
            this._nextValues();
        }
    }

    public replace(models: TModel[]) {
        if (models && models.length) {
            this._map.clear();
            for (let model of models) {
                this._map.set(this.getKey(model), model);
            }
            this._nextValues();
        }
    }

    public getKey(model: TModel): any {
        return model["id"];
    }

    private _nextValues() {
        this._values.next(Array.from(this._map.values()));
        this._keys.next(Array.from(this._map.keys()));
    }
}