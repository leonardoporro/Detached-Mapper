import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { User, UserCollectionDataSource } from "./user.service";

@Component({
    selector: "user-list",
    template: require("./user-list.component.html"),
    providers: [UserCollectionDataSource]
})
export class UserListComponent implements OnInit {
    constructor(public users: UserCollectionDataSource) {
    }

    ngOnInit() {
        this.users.load();
    }
}
