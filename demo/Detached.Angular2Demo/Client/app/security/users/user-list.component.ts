import { Component, OnInit, Input } from "@angular/core";
import { User, UserService, UserCollection } from "./users.services";
import { ISelection, Selection } from "../../../core/md-core";

@Component({
    selector: "user-list",
    template: require("./user-list.component.html"),
    providers: [UserCollection, UserService]
})
export class UserListComponent implements OnInit {
    constructor(public userCollection: UserCollection) {
        //this.userSelection.keys.subscribe(u => {

        //    let k = "keys=" + JSON.stringify(u);

        //    window.history.replaceState(null, "Selection Change", "?" + k);

        //    let s = window.location.origin + window.location.pathname + "?" + k;
        //    window.history.replaceState(null, "SC", s);
        //});
    }

    public userSelection: ISelection<User> = new Selection();

    ngOnInit() {
        this.userCollection.params.pageSize = 5;
        this.userCollection.load();
    }
}