import { Component, OnInit, OnDestroy } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Http } from "@angular/http";
import { User, Role } from "./user.model";
import { Model } from "../../fwk/data";

@Component({
    selector: "user-edit",
    template: require("./user.edit.component.html")
})
export class UserEditComponent {

    public u: Model<User>

    constructor(private route: ActivatedRoute, private http: Http) {
        this.u = new Model<User>("api/user", http);
    }

    ngOnInit() {
        let id = this.route.snapshot.params["id"];
        if (id) {
            this.u.load(id);
        }  
    }

    save() {
        this.u.save();
    }
}