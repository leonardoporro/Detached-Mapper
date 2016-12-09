import { Http, URLSearchParams } from "@angular/http";
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

export class Service<TEntity> implements ICollectionService<TEntity>, IModelService<TEntity> {

    constructor(private uriBase: string, private http: Http) {
    }

    findById(id: any): Observable<TEntity> {
        return this.http.get(this.getResourceUrl(id)).map(r => <TEntity>r.json());
    }

    get(query: any): Observable<Array<TEntity>> {
        return this.http.get(this.uriBase, { search: this.getUrlSearchParams(query) }).map(r => <Array<TEntity>>r.json());
    }

    getPage(pageIndex: number, query: any): Observable<IPage<TEntity>> {
        return this.http.get(this.getResourcePageUrl(pageIndex, query), { search: this.getUrlSearchParams(query) }).map(r => <IPage<TEntity>>r.json());
    }

    save(entity: TEntity): Observable<TEntity> {
        return this.http.post(this.uriBase, entity).map(r => <TEntity>r.json());
    }

    delete(id: any): Observable<any> {
        return this.http.delete(this.getResourceUrl(id));
    }

    protected getResourceUrl(id: any) {
        return this.uriBase + "/" + id;
    }

    protected getResourcePageUrl(pageIndex: number, query: any) {
        return this.uriBase + "/" + "/page/" + pageIndex;
    }

    protected getUrlSearchParams(query: any): URLSearchParams {
        let params: URLSearchParams = new URLSearchParams();
        for (let prop in query) {
            params.append(prop, query[prop]);
        }
        return params;
    }
}

export interface IModelService<TEntity> {
    findById(id: any): Observable<TEntity>;
    save(entity: TEntity): Observable<TEntity>; 
    delete(id: any): Observable<any>;
}

export interface ICollectionService<TEntity> {
    get(query: any): Observable<Array<TEntity>>;
    getPage(pageIndex: number, query: any): Observable<IPage<TEntity>>;
}

export interface IPage<TItem> {
    items: Array<TItem>;
    index: number;
    size: number;
    rowCount: number;
    pageCount: number;
}