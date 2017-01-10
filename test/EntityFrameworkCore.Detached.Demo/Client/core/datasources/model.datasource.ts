import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Observable, ReplaySubject } from "rxjs";
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
    public modelErrors: any = {};
    public modelErrorsChange = new EventEmitter();
    public params: any = {};

    public isNew: boolean = true;
    public isNewChange = new EventEmitter();

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

    public load(key: any): ReplaySubject<TEntity> {
        let result = new ReplaySubject<TEntity>();

        this.params[this.keyProperty] = key;
        let searchParams = this.buildParams();
        let resolvedUrl = this.resolveUrl(this.loadUrl, searchParams);
        searchParams.delete(this.keyProperty);

        this.http.get(resolvedUrl, { search: searchParams })
            .subscribe(response => {
                this.handleResponse(response);
                result.complete();
            },
            error => {
                this.handleError(error);
                result.error(error);
            });

        return result;
    }

    public validate(): boolean {
        return true;
    }

    public save(): ReplaySubject<TEntity> {
        let result = new ReplaySubject<TEntity>();

        if (!this.validate) {
            result.error("invalid.");
        }
        else {
            let searchParams = this.buildParams();
            let resolvedUrl = this.resolveUrl(this.saveUrl, searchParams);
            searchParams.delete(this.keyProperty);

            this.http.post(resolvedUrl, this.model, { search: searchParams })
                .subscribe(response => {
                    this.handleResponse(response);
                    result.next(this.model);
                },
                error => {
                    this.handleError(error);
                    result.error(error);
                }, () => result.complete());
        }
        return result;
    }

    public delete(): ReplaySubject<any> {
        let result = new ReplaySubject();
        let searchParams = this.buildParams();
        let resolvedUrl = this.resolveUrl(this.deleteUrl, searchParams);
        searchParams.delete(this.keyProperty);

        this.http.delete(resolvedUrl, { search: searchParams })
            .subscribe(response => {
                result.next(null);
            }, error => {
                this.handleError(error);
                result.error(error);
            },
            () => result.complete());

        return result;
    }

    protected handleResponse(response: Response) {
        this.model = response.json();
        this.modelChange.emit(this.model);

        this.isNew = false;
        this.isNewChange.emit(this.isNew);
    }

    public addModelError(property: string, code: string) {
        this.modelErrors[property] = code; 
    }
}