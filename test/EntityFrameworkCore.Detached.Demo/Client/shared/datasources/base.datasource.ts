import { EventEmitter } from "@angular/core";
import { Http, URLSearchParams } from "@angular/http";
import { Observable } from "rxjs/Observable";

export class HttpRestBaseDataSource {

    /**@desc replaces url params with query values, e.g.: /api/users/:id -> /api/users/1
      *@param baseUrl - the template url.
      *@param params - the replacement values.
      */
    protected resolveUrl(baseUrl: string, params: URLSearchParams): string {
        return baseUrl.replace(/:[a-zA-z0-9]+/, function (match: string, args: any[]) {
            let paramName = match.substr(1);
            let value = params.get(paramName);
            if (!value) //url params are important!
                throw new URIError("parameter " + match + " in url '" + this.url + "' is not defined.");

            params.delete(paramName); //don't send duplicated parameters.
            return value;
        }.bind(this));
    }
}