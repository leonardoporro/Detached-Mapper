import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { User, UserModelDataSource } from "./users.datasource";
import { Role, RoleCollectionDataSource } from "../roles/roles.datasource";
import { Locale, LocalizationService } from "angular2localization";
import { NgForm, FormGroup } from "@angular/forms";

@Component({
    selector: "user-edit",
    template: require("./user-edit.component.html"),
    providers: [UserModelDataSource, RoleCollectionDataSource]
})
export class UserEditComponent extends Locale implements OnInit {
    constructor(public userSource: UserModelDataSource,
        public rolesSource: RoleCollectionDataSource,
        public activatedRoute: ActivatedRoute,
        public localization: LocalizationService) {

        super(null, localization);
    }

    ngOnInit() {
        let key = this.activatedRoute.snapshot.params["id"];
        if (key !== "new") {
            this.userSource.load(key);
        }
        this.rolesSource.load();
    }

    save(frm : NgForm ) {
        if (frm.valid) {
            this.userSource.save()
                .subscribe(ok => this.close(),
                error => {
                    if (error.memberErrors) {
                        for (let member in error.memberErrors) {
                            let ctrl = frm.controls[member];
                            ctrl.setErrors({ server: error.memberErrors[member] });
                        }
                    }
                });
        }
    }

    delete() {
        this.userSource.delete()
            .subscribe(ok => this.close());
    }

    close() {
        window.history.back();
    }
}
