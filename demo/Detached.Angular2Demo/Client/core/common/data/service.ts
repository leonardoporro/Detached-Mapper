import { Http, URLSearchParams, Response, ResponseType, RequestMethod, Headers } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { ApiError, ApiErrorCodes } from "./error";

export class Service {

    constructor(private http: Http) {
    }

    protected request<TResult>(baseUrl: string, params: any, method: RequestMethod, data: any): Observable<TResult> {

        let searchParams: URLSearchParams = new URLSearchParams();
        for (let paramProp in params) {
            let value: any = params[paramProp];
            if (value !== undefined) {
                searchParams.append(paramProp, value);
            }
        }

        let url = baseUrl.replace(/:[a-zA-z0-9]+/, (match: string, args: any[]) => {
            let paramName: string = match.substr(1);
            let value = searchParams.get(paramName);
            if (!value) { // url params are important!
                throw new URIError("parameter " + match + " in url '" + url + "' is not defined.");
            }
            searchParams.delete(paramName); // don't send duplicated parameters.
            return value;
        });

        let headers = new Headers();
        headers.append("Content-Type", "application/json");

        return this.http.request(url, {
            search: searchParams,
            method: method,
            headers: headers,
            body: data
        }).catch(this._mapException.bind(this))
            .map(this._mapResult.bind(this));
    }

    protected _mapException(response: Response, observable: Observable<any>) {
        let errorValue: ApiError;

        if (response.type == ResponseType.Default) {
            errorValue = response.json();
        } else {
            errorValue = new ApiError();
            errorValue.code = ApiErrorCodes.NoInternetConnection;
            errorValue.message = "No internet connection";
        }

        throw errorValue;
    }

    protected _mapResult(response: Response) {
        return response.json();
    }
}

export class EntityService<TEntity> extends Service implements IEntityService<TEntity> {
    protected _queryUrl: string;
    protected _pagedQueryUrl: string;
    protected _getUrl: string;
    protected _postUrl: string;
    protected _deleteUrl: string;

    constructor(http: Http, baseUrl: string) {
        super(http);

        this._queryUrl = baseUrl;
        this._pagedQueryUrl = baseUrl + "/pages/:pageIndex";
        this._postUrl = baseUrl;
        this._getUrl = baseUrl + "/:key";
        this._deleteUrl = baseUrl + "/:key";
    }

    public query(params: IQueryParams): Observable<IQueryResult<TEntity>> {
        if (params.pageSize) {
            return super.request<IQueryResult<TEntity>>(this._pagedQueryUrl, params, RequestMethod.Get, null);
        } else {
            return super.request<TEntity[]>(this._queryUrl, params, RequestMethod.Get, null)
                .map(data => {
                    return <IQueryResult<TEntity>>{
                        items: data,
                        pageCount: 1,
                        rowCount: data.length
                    };
                });
        }
    }

    public get(key: any): Observable<TEntity> {
        return super.request<TEntity>(this._getUrl, { key: key }, RequestMethod.Get, null);
    }

    public post(entity: TEntity): Observable<TEntity> {
        return super.request(this._postUrl, {}, RequestMethod.Post, entity);
    }

    public delete(key: any) {
        return super.request(this._deleteUrl, { key: key }, RequestMethod.Delete, null);
    }
}

export class IServiceError {
    errorCode: string;
    errorMessage: string;
}

export enum SortOrder {
    Asc = 1,
    Desc = 2
}

export interface IQueryParams {
    pageIndex: number;
    pageSize: number;
    searchText: string;
    sortBy: string;
    sortOrder: SortOrder;
    noCount: boolean;
}

export class QueryParams implements IQueryParams {
    pageIndex: number;
    pageSize: number;
    searchText: string;
    sortBy: string;
    sortOrder: SortOrder;
    noCount: boolean;
}

export interface IQueryResult<TEntity> {
    items: TEntity[];
    pageCount: number;
    rowCount: number;
}

export interface IEntityService<TEntity> {
    query(params: IQueryParams): Observable<IQueryResult<TEntity>>;
    get(key: any): Observable<TEntity>;
    post(entity: TEntity): Observable<TEntity>;
    delete(key: any);
}

