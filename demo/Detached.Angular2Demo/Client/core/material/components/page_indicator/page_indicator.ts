import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation
} from '@angular/core';

@Component({
    selector: 'md-page-indicator',
    template: require("./page_indicator.html"),
    encapsulation: ViewEncapsulation.None
})
export class MdPageIndicatorComponent {

    _pageIndex: number;
    @Input()
    public get pageIndex(): number { return this._pageIndex; }
    public set pageIndex(value: number) {
        if (value !== this._pageIndex) {
            this._pageIndex = value;
            this.pageIndexChange.emit(value);
        }
    }
    @Output()
    public pageIndexChange = new EventEmitter();

    _pageCount: number;
    @Input()
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
            this.pageCountChange.emit(this._pageCount);
        }
    }
    @Output()
    public pageCountChange = new EventEmitter();

    public pages: Array<number> = [];
    @Output()
    public pagesChange = new EventEmitter();
}