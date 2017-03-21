import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Subject } from "rxjs/Subject";
import { IEntityService, IQueryParams, IQueryResult } from "./service";
import { ISelection, Selection } from "./selection";
import { ApiError, ApiErrorCodes } from "./error";

export interface ICollection<TModel, TParams extends IQueryParams> {
    values: Observable<TModel[]>;
    length: number;
    params: TParams;
    load();
    getValues(): TModel[];
}

export class Collection<TModel, TParams extends IQueryParams> implements ICollection<TModel, TParams> {
    private _service: IEntityService<TModel>;
    private _values: BehaviorSubject<TModel[]> = new BehaviorSubject<TModel[]>(new Array());
    private _busy: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
    private _error: Subject<ApiError> = new Subject<ApiError>();

    constructor(service: IEntityService<TModel>) {
        this._service = service;
    }

    public params: TParams = <TParams>{ pageIndex: 1, pageSize: 0 };

    public values: Observable<TModel[]> = this._values.asObservable();

    public get length(): number {
        return this._values.getValue().length;
    }

    public busy: Observable<boolean> = this._busy.asObservable();

    public error: Observable<ApiError> = this._error.asObservable();

    public pageCount: number;

    public rowCount: number;

    public load() {
        this._busy.next(true);
        this._service.query(this.params)
            .subscribe(result => {
                this._values.next(result.items);
                this.pageCount = result.pageCount;
                this.rowCount = result.pageCount;
                this._busy.next(false); 
            }, error => {
                this._error.next(error);
                this._busy.next(false);
            });
    }

    public getValues(): TModel[] {
        return this._values.getValue();
    }

    public getKey(model: TModel) {
        return model["id"];
    }
}

export { IQueryParams, IQueryResult, SortOrder } from "./service";