import { Component, Input, OnInit } from "@angular/core";
import { User, UserQuery, UserService } from "./user.model";
import { IPage } from "../../fwk/data.service";
import { Router, ActivatedRoute, NavigationExtras } from "@angular/router";
import { NavigationService } from "../../fwk/navigation.service";

@Component({
    selector: "user-browse",
    template: require("./user.browse.component.html"),
    providers: [UserService, NavigationService],
    styles: [require("./user.browse.component.css")]
})
export class UserBrowseComponent implements OnInit {

    public query: UserQuery = { name: "" };
    items: Array<User> = [];
    pageIndex: number = 1;
    pageSize: number = 10;
    pageCount: number = 0;
    rowCount: number = 0;

    selectedIndex: number;
    keyProperty: string = "id";

    get selectedId() {
        let item = this.items[this.selectedIndex];
        if (item)
            return item[this.keyProperty];
        else
            return null;
    }
    set selectedId(value: any) {
        this.selectedIndex = this.items.findIndex(e => e[this.keyProperty] == value);
    }

    constructor(private navService: NavigationService, private service: UserService) {
        
    }

    ngOnInit() {
        this.loadPage();
    }

    loadPage() {
        this.selectedIndex = null;
        this.service.getPage(this.pageIndex, this.query)
            .subscribe(r => {
                this.items = r.items;
                this.pageIndex = r.index;
                this.pageSize = r.size;
                this.pageCount = r.pageCount;
                this.rowCount = r.rowCount;
            },
            e => { });
    }
}

export class Item {

}