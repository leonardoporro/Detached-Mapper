import { Component, Input, Output, EventEmitter } from "@angular/core";

@Component({
    selector: "page-selector",
    template: require("./pageSelector.html")
})
export class PageSelector {
    @Input()
    pageCount: number;

    _pageIndex: number;
    @Input()
    get pageIndex(): number {
        return this._pageIndex;
    }
    set pageIndex(value: number) {
        this._pageIndex = value;
        this.pageIndexChange.emit(this._pageIndex);
    }

    @Output()
    pageIndexChange: EventEmitter<number> = new EventEmitter<number>();

    pageItems(): Array<Number> {
        let items: number[] = [];
        if (this.pageCount) {
            for (var i = 1; i <= this.pageCount; i++) {
                items.push(i);
            }
        }
        return items;
    }
}
