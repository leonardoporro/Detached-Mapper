import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Observable, ReplaySubject } from "rxjs";
import { HttpRestBaseDataSource } from "./base.datasource";

export enum SortDirection { Asc, Desc }

export interface ICollectionDataSource<TEntity> {
    searchText: string;
    sortBy: string;
    sortDirection: SortDirection;
    pageIndex: number;
    pageSize: number;
    noCount: boolean;
    pageCount: number;
    rowCount: number;
    items: TEntity[];
    itemsChange: EventEmitter<any>;
    load();
    toggleSort(propertyName: string);
}

export class DataPage<TEntity> {
    public items: TEntity[];
    public pageCount: number;
    public pageIndex: number;
    public pageSize: number;
    public rowCount: number;
}

export class HttpRestCollectionDataSource<TEntity, TSearchParams>
    extends HttpRestBaseDataSource
    implements ICollectionDataSource<TEntity> {

    constructor(http: Http, public baseUrl: string) {
        super(http);
    }

    public searchText: string;
    public sortBy: string;
    public sortDirection: SortDirection = SortDirection.Asc;
    public pageIndex: number = 1;
    public pageSize: number;
    public noCount: boolean;
    public pageCount: number;
    public rowCount: number;

    _items: TEntity[] = [];
    public get items(): TEntity[] {
        return this._items;
    }
    public set items(value: TEntity[]) {
        if (value != this._items) {
            this._items = value;
            this.itemsChange.emit(this._items);
        }
    }
    public itemsChange = new EventEmitter();

    public load(): ReplaySubject<TEntity[]> {
        let params = Object.assign({}, this.requestParams);
        params.searchText = this.searchText;
        params.sortBy = this.sortBy;
        params.sortDirection = this.sortDirection;
        params.pageIndex = this.pageIndex;
        params.pageSize = this.pageSize;
        params.noCount = this.noCount;
        
        let request = this.execute<TEntity[]>(this.baseUrl, this.requestParams, "get", null);
        request.subscribe(data => this.items = data,
                          error => { });
        return request;
    }

    public toggleSort(propertyName: string): Observable<TEntity[]> {
        if (this.sortBy != propertyName) {
            this.sortDirection = SortDirection.Asc;
        } else {
            switch (this.sortDirection) {
                case SortDirection.Desc:
                    this.sortDirection = SortDirection.Asc;
                    break;
                case SortDirection.Asc:
                    this.sortDirection = SortDirection.Desc;
                    break;
            }
        }
        this.sortBy = propertyName;
        return this.load();
    }
}