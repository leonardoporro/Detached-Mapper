import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Observable, ReplaySubject } from "rxjs";
import { HttpRestBaseDataSource } from "./base.datasource";

export interface IModelDataSource<TEntity> {
    keyProperty: string;
    model: TEntity;
    requestParams: any;
    load(key: any);
    save();
}

export class HttpRestModelDataSource<TEntity> extends HttpRestBaseDataSource implements IModelDataSource<TEntity> {

    constructor(http: Http, protected baseUrl: string) {
        super(http);
        this.loadUrl = baseUrl;
        this.saveUrl = baseUrl + "/:" + this.keyProperty;
        this.deleteUrl = this.saveUrl;
    }
   
    public model: TEntity = <TEntity>{};
    public modelChange = new EventEmitter();
    public modelErrors: any = {};
    public modelErrorsChange = new EventEmitter();
    public isNew: boolean = true;
    public isNewChange = new EventEmitter();
    public key: any;
    protected loadUrl: string;
    protected saveUrl: string;
    protected deleteUrl: string;

    public load(key: any): ReplaySubject<TEntity> {
        let params = Object.assign({}, this.requestParams);
        params[this.keyProperty] = key;
        let request = this.execute<TEntity>(this.loadUrl, params, "get", null);
        request.subscribe(data => { 
            this.model = data;
        }, error => { });
        return request;
    }

    public save(): ReplaySubject<TEntity> {
        let request = this.execute<TEntity>(this.saveUrl, this.requestParams, "post", this.model);
        request.subscribe(data => {
            this.model = data;
        }, error => { });
        return request;
    }

    public delete(): ReplaySubject<any> {
        return null;
    }
}