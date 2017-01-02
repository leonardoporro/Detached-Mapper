import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";
import { User, UserService } from "./user.service";

@Component({
    selector: "user-list",
    template: require("./user-list.component.html"),
    providers: [UserService]
})
export class UserListComponent implements OnInit {
    constructor(private userService: UserService) {

    }

    public items: Array<User> = [];

    public sel: Array<any> = [];

    ngOnInit() {
        this.userService.get(null)
            .subscribe(r => this.items = r);

        this.sel.push({
            id: 2,
            name: "User 2"
        });
    }
}
