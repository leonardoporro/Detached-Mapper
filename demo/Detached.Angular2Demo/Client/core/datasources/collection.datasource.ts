import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Observable, ReplaySubject } from "rxjs";
import { HttpRestBaseDataSource } from "./base.datasource";

export enum SortDirection { Asc, Desc }

export interface ICollectionDataSource<TEntity> {
    searchText: string;
    orderBy: string;
    sortDirection: SortDirection;
    pageIndex: number;
    pageIndexChange: EventEmitter<any>;
    pageSize: number;
    noCount: boolean;
    pageCount: number;
    pageCountChange: EventEmitter<any>;
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
        this.pageBaseUrl = baseUrl + "/pages/:pageIndex";
    }

    protected pageBaseUrl: string;
    public searchText: string;
    public orderBy: string;
    public sortDirection: SortDirection = SortDirection.Asc;

    public pageSize: number;
    public noCount: boolean;

    _pageCount: number;
    public pageCountChange = new EventEmitter();
    public get pageCount(): number { return this._pageCount; }
    public set pageCount(value: number) {
        if (value !== this._pageCount) {
            this._pageCount = value;
            this.pageCountChange.emit(this._pageCount);
        }
    }

    _pageIndex: number = 1;
    public pageIndexChange = new EventEmitter();
    public get pageIndex(): number { return this._pageIndex; }
    public set pageIndex(value: number) {
        if (value !== this._pageIndex) {
            this._pageIndex = value;
            this.pageIndexChange.emit(this._pageIndex);
        }
    }

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

    public load(): ReplaySubject<any> {
        let params = Object.assign({}, this.requestParams);
        params.searchText = this.searchText;
        params.orderBy = this.getSortQueryParam();

        if (this.pageSize) {
            params.pageIndex = this.pageIndex;
            params.pageSize = this.pageSize;
            params.noCount = this.noCount;

            let request = this.execute<DataPage<TEntity>>(this.pageBaseUrl, params, "get", null);
            request.subscribe(data => {
                this.items = data.items;
                this.pageCount = data.pageCount;
            },
                error => {

                });
            return request;
        } else {
            let request = this.execute<TEntity[]>(this.baseUrl, params, "get", null);
            request.subscribe(data => {
                this.items = data;
            },
                error => {

                });

            return request;
        }
    }

    protected getSortQueryParam() {
        if (this.orderBy) {
            if (this.sortDirection == 1)
                return this.orderBy + "+desc";
            else
                return this.orderBy + "+asc";
        }
    }

    public toggleSort(propertyName: string): Observable<TEntity[]> {
        if (this.orderBy != propertyName) {
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
        this.orderBy = propertyName;
        return this.load();
    }
}