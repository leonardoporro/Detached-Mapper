import { Component } from "@angular/core";
import { Http } from "@angular/http";
import { User, Role } from "./user.model";
import { Collection } from "../../fwk/data";

@Component({
    selector: "user-browse",
    template: require("./user.browse.component.html")
})
export class UserBrowseComponent {
    public users: Collection<User, any>;

    constructor(http: Http) {
        this.users = new Collection<User, any>("api/user", http);
        this.users.filter.Text = "hola";
        this.users.load();
    }
}

