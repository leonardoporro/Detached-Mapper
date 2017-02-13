import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams, Response } from "@angular/http";
import { Observable } from "rxjs/Observable";
import { ReplaySubject } from "rxjs/ReplaySubject";

export class HttpRestBaseDataSource {

    public keyProperty: string = "id";
    public requestParams: any = {};
    public isBusy: boolean;

    constructor(private http: Http) {

    }

    protected execute<TResult>(baseUrl: string, params: any, verb: string, data: any): ReplaySubject<TResult> {
        let result = new ReplaySubject<TResult>();
        this.isBusy = true;

        let searchParams = new URLSearchParams();
        for (let paramProp in params) {
            searchParams.append(paramProp, params[paramProp]);
        }

        let url = baseUrl.replace(/:[a-zA-z0-9]+/, function (match: string, args: any[]) {
            let paramName = match.substr(1);
            let value = searchParams.get(paramName);
            if (!value) //url params are important!
                throw new URIError("parameter " + match + " in url '" + this.url + "' is not defined.");

            searchParams.delete(paramName); //don't send duplicated parameters.
            return value;
        }.bind(this));

        let request: Observable<Response>;
        switch (verb) {
            case "get":
                request = this.http.get(url, { search: searchParams })
                break;
            case "post":
                request = this.http.post(url, data, { search: searchParams });
                break;
            case "put":
                request = this.http.put(url, data, { search: searchParams });
                break;
            case "delete":
                request = this.http.delete(url, { search: searchParams });
                break;
        }

        request.subscribe(data => result.next(this.handleData<TResult>(data)),
                          error => result.error(this.handleError(error)),
                          () => {
                              result.complete();
                              this.isBusy = false;
                          });
        return result;
    }

    protected handleData<TResult>(data: Response): TResult {
        return <TResult>data.json();
    }

    protected handleError(error) {
        return <ApiError>error.json();
    }
}

export class ApiError {
    errorCode: string;
    errorMessage: string;
    memberErrors: any;
}