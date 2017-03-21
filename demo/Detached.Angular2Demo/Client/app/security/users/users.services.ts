import { Injectable } from "@angular/core";
import { Http } from "@angular/http";
import { EntityService, Collection, Model, QueryParams } from "../../../core/md-core";
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
export class UserService extends EntityService<User> {
    constructor(http: Http) {
        super(http, "api/users");
    }
}

@Injectable()
export class UserCollection extends Collection<User, QueryParams> {
    constructor(userService: UserService) {
        super(userService);
    }
}

@Injectable()
export class UserModel extends Model<User> {
    constructor(userService: UserService) {
        super(userService);
    }
}