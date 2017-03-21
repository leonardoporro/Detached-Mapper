import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { IEntityService } from "./service";

export class Model<TModel> {
    private _service: IEntityService<TModel>;
    private _busy: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    constructor(service: IEntityService<TModel>) {
        this._service = service;
    }

    public model: TModel = <TModel>{};

    public busy: Observable<boolean> = this._busy.asObservable();

    public load(key: any) {
        this._busy.next(true);
        this._service.get(key)
            .subscribe(data => {
                this.model = data;

                this._busy.next(false);
            },
            error => {

                this._busy.next(false);
            });
    }

    public save(): Observable<TModel> {
        let result = new Subject<TModel>();
        this._busy.next(true);
        this._service.post(this.model)
            .subscribe(model => {
                this.model = model;
                result.next(model);

                this._busy.next(false);
            },
            error => {
                result.error(error);

                this._busy.next(false);
            });
        return result;
    }

    public delete(key: any) {
        this._service.delete(key)
            .subscribe(result => {

            },
            error => {

            });
    }
}