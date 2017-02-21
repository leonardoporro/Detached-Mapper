import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { HttpRestCollectionDataSource } from "../../../core/core.module";
import { HttpRestModelDataSource } from "../../../core/core.module";
import { Role } from "../roles/roles.datasource";

export class User {
    id: number;
    name: string;
    firstName: string;
    lastName: string;
    dateOfBith: Date;
    email: string;
    address: string;
    city: string;
    roles: Role[];
}

export class UserQuery {
    dateOfBirthFrom: Date;
    dateOfBirthTo: Date;
    isActive: boolean;
}

@Injectable()
export class UserCollectionDataSource extends HttpRestCollectionDataSource<User, UserQuery> {
    constructor(http: Http) {
        super(http, "/api/users");
        this.orderBy = "name";
    }
}

@Injectable()
export class UserModelDataSource extends HttpRestModelDataSource<User> {
    constructor(http: Http) {
        super(http, "api/users");
        this.requestParams.culture = "es-AR";
    }
}
