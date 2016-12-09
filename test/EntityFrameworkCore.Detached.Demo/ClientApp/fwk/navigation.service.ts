import { Injectable } from "@angular/core";
import { Router, ActivatedRoute, NavigationExtras } from "@angular/router";

@Injectable()
export class NavigationService {

    constructor(private activatedRoute: ActivatedRoute, private router: Router) {
    }

    public getParams() {
        let queryParams = this.activatedRoute.snapshot.queryParams;
        let queryObj = {};

        for (let paramName in queryParams) {
            queryObj[paramName] = queryParams[paramName];
        }
        return queryObj;
    }

    public setParam(key: string, value: any) {
        let queryParams = this.getParams();
        queryParams[key] = value;
        this.setParams(queryParams);
    }

    public setParams(queryParams: any) {
        let routeParts: Array<string> = [];
        let snapshot = this.activatedRoute.snapshot;
        while (snapshot.parent != null) {
            routeParts.splice(0, 0, snapshot.url[0].path);
            snapshot = snapshot.parent;
        }
        this.router.navigate(routeParts, {
            queryParams: queryParams,
            replaceUrl: true
        });
    }
}