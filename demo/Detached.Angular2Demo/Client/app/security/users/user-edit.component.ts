import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { User, UserModelDataSource } from "./users.datasource";
import { Locale, LocalizationService } from "angular2localization";

@Component({
    selector: "user-edit",
    template: require("./user-edit.component.html"),
    providers: [UserModelDataSource]
})
export class UserEditComponent extends Locale implements OnInit {
    constructor(public userSource: UserModelDataSource,
        public activatedRoute: ActivatedRoute,
        public localization: LocalizationService) {

        super(null, localization);
    }

    ngOnInit() {
        let key = this.activatedRoute.snapshot.params["id"];
        if (key !== "new") {
            this.userSource.load(key);
        }
    }

    save(frm) {
        if (frm.valid) {
            this.userSource.save()
                .subscribe(e => window.history.back());
        }
    }

    delete() {
        this.userSource.delete()
            .subscribe(e => window.history.back());
    }
}
