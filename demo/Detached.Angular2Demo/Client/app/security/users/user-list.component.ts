import { Component, OnInit, Input, Output, EventEmitter, ContentChild } from "@angular/core";
import { User, UserCollectionDataSource } from "./users.datasource";
import { MdSidenav } from "@angular/material";

@Component({
    selector: "user-list",
    template: require("./user-list.component.html"),
    providers: [UserCollectionDataSource]
})
export class UserListComponent implements OnInit {
    constructor(public usersSource: UserCollectionDataSource) {
    }

    ngOnInit() {
        this.usersSource.pageSize = 3;
        this.usersSource.load();
    }
}
