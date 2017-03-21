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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var MdPageIndicatorComponent = (function () {
    function MdPageIndicatorComponent() {
        this.pageIndexChange = new core_1.EventEmitter();
        this.pageCountChange = new core_1.EventEmitter();
        this.pages = [];
        this.pagesChange = new core_1.EventEmitter();
    }
    Object.defineProperty(MdPageIndicatorComponent.prototype, "pageIndex", {
        get: function () { return this._pageIndex; },
        set: function (value) {
            if (value !== this._pageIndex) {
                this._pageIndex = value;
                this.pageIndexChange.emit(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(MdPageIndicatorComponent.prototype, "pageCount", {
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
                this.pageCountChange.emit(this._pageCount);
            }
        },
        enumerable: true,
        configurable: true
    });
    return MdPageIndicatorComponent;
}());
__decorate([
    core_1.Input(),
    __metadata("design:type", Number),
    __metadata("design:paramtypes", [Number])
], MdPageIndicatorComponent.prototype, "pageIndex", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], MdPageIndicatorComponent.prototype, "pageIndexChange", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number),
    __metadata("design:paramtypes", [Number])
], MdPageIndicatorComponent.prototype, "pageCount", null);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], MdPageIndicatorComponent.prototype, "pageCountChange", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", Object)
], MdPageIndicatorComponent.prototype, "pagesChange", void 0);
MdPageIndicatorComponent = __decorate([
    core_1.Component({
        selector: 'md-page-indicator',
        template: require("./page_indicator.html"),
        encapsulation: core_1.ViewEncapsulation.None
    })
], MdPageIndicatorComponent);
exports.MdPageIndicatorComponent = MdPageIndicatorComponent;
//# sourceMappingURL=page_indicator.js.map