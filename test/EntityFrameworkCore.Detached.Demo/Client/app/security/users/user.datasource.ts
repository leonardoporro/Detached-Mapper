import { Injectable } from "@angular/core";
import { Http, URLSearchParams } from "@angular/http";
import { HttpRestCollectionDataSource } from "../../../core/core.module";
import { HttpRestModelDataSource } from "../../../core/core.module";

export interface User {
    id: number;
    name: string;
}

export class UserQuery {
    dateOfBirthFrom: Date;
    dateOfBirthTo: Date;
    isActive: boolean;
}

@Injectable()
export class UserCollectionDataSource extends HttpRestCollectionDataSource<User, UserQuery> {
    constructor(http: Http) {
        super(http, "/api/users/pages/:pageIndex");
        this.sortBy = "name";
    }
}

@Injectable()
export class UserModelDataSource extends HttpRestModelDataSource<User> {
    constructor(http: Http) {
        super(http, "api/users/:id");
    }
}
