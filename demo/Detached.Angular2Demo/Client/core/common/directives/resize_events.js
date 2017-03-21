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
var ResizeEventsDirective = (function () {
    function ResizeEventsDirective(element) {
        this.element = element;
        this.onResize = new core_1.EventEmitter();
        var elementResizeDetectorType = require("element-resize-detector");
        this.elementResizeDetector = elementResizeDetectorType({
            strategy: "scroll"
        });
    }
    ResizeEventsDirective.prototype.ngOnInit = function () {
        this.elementResizeDetector.listenTo(this.element.nativeElement, this.onNativeResize.bind(this));
    };
    ResizeEventsDirective.prototype.ngOnDestroy = function () {
        this.elementResizeDetector.removeListener(this.element.nativeElement, this.onNativeResize.bind(this));
    };
    ResizeEventsDirective.prototype.onNativeResize = function (e) {
        this.onResize.emit(e);
    };
    return ResizeEventsDirective;
}());
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], ResizeEventsDirective.prototype, "onResize", void 0);
ResizeEventsDirective = __decorate([
    core_1.Directive({
        selector: "[resizeEvents]"
    }),
    __metadata("design:paramtypes", [core_1.ElementRef])
], ResizeEventsDirective);
exports.ResizeEventsDirective = ResizeEventsDirective;
//# sourceMappingURL=resize_events.js.map