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

    constructor(private http: Http, public baseUrl: string) {
        super();
    }

    public keyProperty: string = "id";
    public searchParams: TSearchParams = <TSearchParams>{};
    public searchText: string;
    public sortBy: string;
    public sortDirection: SortDirection = SortDirection.Asc;
    public pageIndex: number = 1;
    public pageSize: number;
    public noCount: boolean;
    public pageCount: number;
    public rowCount: number;
    public items: TEntity[] = [];
    public itemsChange = new EventEmitter();

    /** builds a URLSearchParams instance from searchParams and collection parameters. */
    protected buildParameters(): URLSearchParams {
        let searchParams = new URLSearchParams();
        for (let queryProp in this.searchParams) {
            let value = this.searchParams[queryProp];
            if (value) {
                searchParams.append(queryProp, value);
            }
        }
        if (this.searchText) {
            searchParams.append("searchText", this.searchText);
        }
        if (this.sortBy) {
            searchParams.append("orderBy", this.sortBy + (this.sortDirection == SortDirection.Asc ? "+asc" : "+desc"));
        }
        if (this.pageIndex) {
            searchParams.append("pageIndex", this.pageIndex.toString());
        }
        if (this.pageSize) {
            searchParams.append("pageSize", this.pageSize.toString());
        }
        if (this.noCount !== undefined) {
            searchParams.append("noCount", this.noCount ? "true" : "false");
        }
        return searchParams;
    }

    public load(): ReplaySubject<TEntity[]> {
        let result = new ReplaySubject();

        let searchParams = this.buildParameters();
        let resolvedUrl = this.resolveUrl(this.baseUrl, searchParams);

        this.http.get(resolvedUrl, { search: searchParams })
            .subscribe(response => {
                this.handleResponse(response);
                result.next(this.items);
            },
            error => {
                this.handleError(error);
                result.error(error);
            },
            () => result.complete());

        return result;
    }

    protected handleResponse(response: Response) {
        let result = response.json();
        if (result instanceof Array) {
            this.handleArray(result);
        } else {
            this.handlePage(result);
        }
    }

    protected handleArray(array: TEntity[]) {
        this.items = array;
        this.itemsChange.emit(this.items);
    }

    protected handlePage(page: DataPage<TEntity>) {
        this.items = page.items;
        this.pageCount = page.pageCount;
        this.rowCount = page.rowCount;
        this.itemsChange.emit(this.items);
    }

    protected handleError(error) {

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

    public loadFirstPage() {
    }

    public loadPreviousPage() {
    }

    public loadNextPage() {
    }

    public loadLastPage() {
    }
}