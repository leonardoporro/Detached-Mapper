import { Component, OnInit, Input } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NgForm, FormGroup } from "@angular/forms";
import { User, UserService, UserModel } from "./users.services";
import { ISelection, Selection } from "../../../core/md-core";
import { MdMessageBoxService } from "../../../core/md-core";

@Component({
    selector: "user-edit",
    template: require("./user-edit.component.html"),
    providers: [UserModel, UserService, MdMessageBoxService]
})
export class UserEditComponent implements OnInit {

    constructor(private userModel: UserModel,
        private activatedRoute: ActivatedRoute,
        private messageBoxService: MdMessageBoxService) {
    }

    ngOnInit() {
        let key = this.activatedRoute.snapshot.params["id"];
        if (key !== "new") {
            this.userModel.load(key);
        }
        //this.rolesSource.load();
    } 

    save(frm: NgForm) {

        if (frm.valid) {
            this.userModel.save()
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
        //this.userSource.delete()
        //    .subscribe(ok => this.close());
    }

    close() {
        window.history.back();
    }
}
