import { Injectable } from "@angular/core";
import { Service } from "../../fwk/data.service";
import { Http } from "@angular/http";

export interface Role {
    id: number,
    name: string
}

@Injectable()
export class RoleService extends Service<Role> {
    constructor(http: Http) {
        super("api/role", http);
    }
}

export interface User {
    id: number;
    name: string;
    roles: Array<Role>
}

export interface UserQuery  {
    name: string;
}

@Injectable()
export class UserService extends Service<User> {
    constructor(http: Http) {
        super("api/user", http);
    }
}
