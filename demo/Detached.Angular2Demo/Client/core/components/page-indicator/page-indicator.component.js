"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var PageIndicatorComponent = (function () {
    function PageIndicatorComponent() {
        this.pages = [];
        this.pagesChange = new core_1.EventEmitter();
    }
    Object.defineProperty(PageIndicatorComponent.prototype, "dataSource", {
        set: function (value) {
            this._dataSource = value;
            this._dataSource.pageIndexChange.subscribe(this.onPageIndexChange.bind(this));
            this._dataSource.pageCountChange.subscribe(this.onPageCountChange.bind(this));
            this._pageIndex = this._dataSource.pageIndex;
            this._pageCount = this._dataSource.pageCount;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageIndicatorComponent.prototype, "pageIndex", {
        get: function () { return this._pageIndex; },
        set: function (value) {
            if (value !== this._pageIndex) {
                this._pageIndex = value;
                if (this._dataSource) {
                    this._dataSource.pageIndex = value;
                    this._dataSource.load();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PageIndicatorComponent.prototype, "pageCount", {
        get: function () {
            return this._pageCount;
        },
        set: function (value) {
            if (value !== this._pageCount) {
                this._pageCount = value;
                this.pages = [];
                var i = 1;
                while (i <= this._pageCount) {
                    this.pages.push(i);
                    i++;
                }
                this.pagesChange.emit(this.pages);
            }
        },
        enumerable: true,
        configurable: true
    });
    PageIndicatorComponent.prototype.onPageIndexChange = function () {
        this.pageIndex = this._dataSource.pageIndex;
    };
    PageIndicatorComponent.prototype.onPageCountChange = function () {
        this.pageCount = this._dataSource.pageCount;
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object), 
        __metadata('design:paramtypes', [Object])
    ], PageIndicatorComponent.prototype, "dataSource", null);
    PageIndicatorComponent = __decorate([
        core_1.Component({
            selector: 'd-page-indicator',
            template: require("./page-indicator.component.html"),
            encapsulation: core_1.ViewEncapsulation.None
        }), 
        __metadata('design:paramtypes', [])
    ], PageIndicatorComponent);
    return PageIndicatorComponent;
}());
exports.PageIndicatorComponent = PageIndicatorComponent;
//# sourceMappingURL=page-indicator.component.js.map