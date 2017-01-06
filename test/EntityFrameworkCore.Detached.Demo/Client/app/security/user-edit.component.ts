import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { User, UserModelDataSource } from "./user.service";

@Component({
    selector: "user-edit",
    template: require("./user-edit.component.html"),
    providers: [UserModelDataSource]
})
export class UserEditComponent implements OnInit {
    constructor(public userSource: UserModelDataSource, public activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
        let key = this.activatedRoute.snapshot.params["id"];
        this.userSource.load(key);
    }

    save() {
        this.userSource.save()
            .subscribe(e => window.history.back());
    }

    delete() {
        this.userSource.delete()
            .subscribe(e => window.history.back());
    }

    do(form: any) {

    }
}
