import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { HttpRestCollectionDataSource } from "../../../core/core.module";
import { HttpRestModelDataSource } from "../../../core/core.module";

export class Role {
    id: number;
    name: string;
}

export class RoleQuery {

}

@Injectable()
export class RoleCollectionDataSource extends HttpRestCollectionDataSource<Role, RoleQuery> {
    constructor(http: Http) {
        super(http, "/api/roles");
        this.sortBy = "name";
    }
}
