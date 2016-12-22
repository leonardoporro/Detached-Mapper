import { Injectable } from "@angular/core";
import { Http, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/Rx';

@Injectable()
export class UserService {

    baseUrl: string = "/api/users";

    constructor(protected http: Http) {
    }

    public findById(id: any): Observable<User> {
        return this.http.get(this.baseUrl + "/" + id).map(r => r.json());
    }

    public get(query: any): Observable<Array<User>> {
       
        let params = new URLSearchParams();
        if (query) {
            for (let prop of query) {
                params.append(prop, query[prop]);
            }
        }
        return this.http.get(this.baseUrl, { search: params }).map(r => r.json());
    }

    public getPage(pageIndex: number, pageSize: number, query: any): Observable<IPage<User>> {
        let params = new URLSearchParams();
        params.append("pageIndex", pageIndex.toString());
        params.append("pageSize", pageSize.toString());
        for (let prop of query) {
            params.append(prop, query[prop]);
        }
        return this.http.get(this.baseUrl, { search: params }).map(r => r.json());
    }

    public save(entity: User): Observable<User> {
        return this.http.post(this.baseUrl, entity).map(r => r.json());
    } 

    public delete(id: number): Observable<any> {
        return this.http.delete(this.baseUrl + "/" + id);
    } 
}

export interface User {
    id: number;
    name: string;
}

export interface IPage<T> {
    pageIndex: number;
    pageSize: number;
    pageCount: number;
    rowCount: number;
    items: Array<T>;
}