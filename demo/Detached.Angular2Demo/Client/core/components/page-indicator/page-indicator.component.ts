import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ContentChild,
    ContentChildren,
    QueryList,
    TemplateRef,
} from '@angular/core';

import { ICollectionDataSource } from "../../datasources/collection.datasource";

@Component({
    selector: 'd-page-indicator',
    template: require("./page-indicator.component.html"),
    encapsulation: ViewEncapsulation.None
})
export class PageIndicatorComponent {
    _dataSource: ICollectionDataSource<any>;

    @Input()
    public set dataSource(value: ICollectionDataSource<any>) {
        this._dataSource = value;
        this._dataSource.pageIndexChange.subscribe(this.onPageIndexChange.bind(this));
        this._dataSource.pageCountChange.subscribe(this.onPageCountChange.bind(this));
        this._pageIndex = this._dataSource.pageIndex;
        this._pageCount = this._dataSource.pageCount;
    }

    _pageIndex: number;
    public get pageIndex(): number { return this._pageIndex; }
    public set pageIndex(value: number) {
        if (value !== this._pageIndex) {
            this._pageIndex = value;
            if (this._dataSource) {
                this._dataSource.pageIndex = value;
                this._dataSource.load();
            }
        }
    }

    _pageCount: number;
    public get pageCount(): number {
        return this._pageCount;
    }
    public set pageCount(value: number) {
        if (value !== this._pageCount) {
            this._pageCount = value;
            this.pages = [];
            let i: number = 1;
            while (i <= this._pageCount) {
                this.pages.push(i);
                i++;
            }
            this.pagesChange.emit(this.pages);
        }
    }

    public pages: Array<number> = [];
    public pagesChange = new EventEmitter();

    public onPageIndexChange() {
        this.pageIndex = this._dataSource.pageIndex;
    }

    public onPageCountChange() {
        this.pageCount = this._dataSource.pageCount; 
    }
}