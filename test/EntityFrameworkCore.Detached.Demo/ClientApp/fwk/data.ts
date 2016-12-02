import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { Inject, Injectable } from '@angular/core';
import { Http, URLSearchParams } from '@angular/http';

@Injectable()
export class Model<T> {

    public model: T = <T>{};
    public error: any;

    constructor(private path: string, private http: Http) {
    }

    load(id: any): Observable<T> {
        let response = this.http.get(this.path + '/' + id).map(r => <T>r.json());
        response.subscribe(r => {
            this.model = r;
        },
            e => {
                this.error = e;
            });
        return response;
    }

    save(): Observable<T> {
        let response = this.http.post(this.path, this.model).map(r => <T>r.json());
        response.subscribe(r => {
            this.model = r;
        },
            e => {
                this.error = e;
            });

        return response;
    }
}

@Injectable()
export class Collection<TItem, TFilter> {

    public items: Array<TItem> = [];
    public filter: TFilter = <TFilter>{};
    public error: any;
    
    constructor(private path: string, private http: Http) {
    }

    load(): Observable<Array<TItem>> {

        let params: URLSearchParams = new URLSearchParams();
        for (let prop in this.filter) {
            params.append(prop, this.filter[prop]);
        }

        let response = this.http.get(this.path, { search: params }).map(r => <Array<TItem>>r.json());
        response.subscribe(r => {
            this.items = r;
        },
            e => {
                this.error = e;
            });
        return response;
    }
}