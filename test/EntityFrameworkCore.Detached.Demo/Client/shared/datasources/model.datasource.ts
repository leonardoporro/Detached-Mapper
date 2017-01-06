import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { HttpRestBaseDataSource } from "./base.datasource";

export interface IModelDataSource<TEntity> {
    keyProperty: string;
    model: TEntity;
    params: any;
    load(key: any);
    save();
}

export class HttpRestModelDataSource<TEntity> extends HttpRestBaseDataSource implements IModelDataSource<TEntity> {

    constructor(protected http: Http, protected baseUrl: string) {
        super();
        this.loadUrl = baseUrl;
        this.saveUrl = baseUrl.replace("/:" + this.keyProperty, "");
        this.deleteUrl = baseUrl;
    }

    public keyProperty: string = "id";
    public model: TEntity = <TEntity>{};
    public modelChange = new EventEmitter();
    public params: any = {};
    public get isNew(): boolean {
        return this.model && this.model[this.keyProperty];
    }

    protected loadUrl: string;
    protected saveUrl: string;
    protected deleteUrl: string;

    protected buildParams(): URLSearchParams {
        let searchParams = new URLSearchParams();
        for (let paramProp in this.params) {
            searchParams.append(paramProp, this.params[paramProp]);
        }

        return searchParams;
    }

    public load(key: any): Observable<TEntity> {
        this.params[this.keyProperty] = key;
        let searchParams = this.buildParams();
        let resolvedUrl = this.resolveUrl(this.loadUrl, searchParams);
        searchParams.delete(this.keyProperty);

        let result = this.http.get(resolvedUrl, { search: searchParams })
            .map(r => r.json());

        result.subscribe(this.handleResponse.bind(this),
            this.handleError.bind(this));

        return result;
    }

    public validate(): boolean {
        return true;
    }

    public save(): Observable<TEntity> {
        if (!this.validate) {
            return Observable.throw("invalid.");
        }
        else {
            let searchParams = this.buildParams();
            let resolvedUrl = this.resolveUrl(this.saveUrl, searchParams);
            searchParams.delete(this.keyProperty);

            let result = this.http.post(resolvedUrl, this.model, { search: searchParams })
                .map(r => r.json());

            result.subscribe(this.handleResponse.bind(this),
                this.handleError.bind(this));

            return result;
        }
    }

    public delete(): Observable<any> {
        let searchParams = this.buildParams();
        let resolvedUrl = this.resolveUrl(this.deleteUrl, searchParams);
        searchParams.delete(this.keyProperty);

        let result = this.http.delete(resolvedUrl, { search: searchParams });
        result.subscribe(this.handleResponse.bind(this), this.handleError.bind(this));

        return result;
    }

    protected handleResponse(response) {
        this.model = response;
        this.modelChange.emit(this.model);
    }

    protected handleError(error) {

    }
}