import { Component, OnInit, OnDestroy } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { Http } from "@angular/http";

import { User, UserQuery, UserService, Role, RoleService } from "./user.model";



@Component({
    selector: "user-edit",
    template: require("./user.edit.component.html"),
    providers: [UserService]
})
export class UserEditComponent {

    public model: User = <User>{};
    public errors: any = {};
    public id: any;

    constructor(private route: ActivatedRoute, private service: UserService) {
        this.id = route.snapshot.params["id"];
        this.load();

        this.errors.name = "the field is sarlanga.";
    }

    load() {
        this.service.findById(this.id)
            .subscribe(r => this.model = r,
            e => { });
    }

    save() {
        this.service.save(this.model)
            .subscribe(r => this.model = r,
            e => { });
    }
}