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
var core_1 = require("@angular/core");
var PopoverComponent = (function () {
    function PopoverComponent(changeDetectionRef, elementRef) {
        this.changeDetectionRef = changeDetectionRef;
        this.elementRef = elementRef;
        this.hideOnClick = false;
        this.isVisible = false;
        this.directionUp = false;
    }
    PopoverComponent.prototype.ngAfterViewInit = function () {
        // Add a hide listener to native element
        this.elementRef.nativeElement.addEventListener("hide", this.hide.bind(this));
    };
    PopoverComponent.prototype.onDocumentClick = function (event) {
        if (this.isVisible &&
            (this.hideOnClick || !this.elementRef.nativeElement.contains(event.target))) {
            this.hide();
        }
    };
    PopoverComponent.prototype.ngOnDestroy = function () {
        this.elementRef.nativeElement.removeEventListener("hide");
    };
    PopoverComponent.prototype.toggle = function (event) {
        if (this.isVisible) {
            this.hide();
        }
        else {
            this.hideAllPopovers();
            this.show(event);
        }
    };
    PopoverComponent.prototype.hide = function () {
        this.isVisible = false;
        this.changeDetectionRef.markForCheck();
    };
    PopoverComponent.prototype.hideAllPopovers = function () {
        var nodeList = document.querySelectorAll(".mdl-popover.is-visible");
        for (var i = 0; i < nodeList.length; ++i) {
            nodeList[i].dispatchEvent(new Event("hide"));
        }
    };
    PopoverComponent.prototype.show = function (event) {
        event.stopPropagation();
        this.isVisible = true;
        this.updateDirection(event);
    };
    PopoverComponent.prototype.updateDirection = function (event) {
        var _this = this;
        var nativeEl = this.elementRef.nativeElement;
        var targetRect = event.target.getBoundingClientRect();
        var viewHeight = window.innerHeight;
        setTimeout(function () {
            var height = nativeEl.offsetHeight;
            if (height) {
                var spaceAvailable = {
                    top: targetRect.top,
                    bottom: viewHeight - targetRect.bottom
                };
                _this.directionUp = spaceAvailable.bottom < height;
                _this.changeDetectionRef.markForCheck();
            }
        });
    };
    __decorate([
        core_1.Input("hide-on-click"), 
        __metadata('design:type', Boolean)
    ], PopoverComponent.prototype, "hideOnClick", void 0);
    __decorate([
        core_1.HostBinding("class.is-visible"), 
        __metadata('design:type', Boolean)
    ], PopoverComponent.prototype, "isVisible", void 0);
    __decorate([
        core_1.HostBinding("class.direction-up"), 
        __metadata('design:type', Boolean)
    ], PopoverComponent.prototype, "directionUp", void 0);
    __decorate([
        core_1.HostListener("document:click", ["$event"]), 
        __metadata('design:type', Function), 
        __metadata('design:paramtypes', [Event]), 
        __metadata('design:returntype', void 0)
    ], PopoverComponent.prototype, "onDocumentClick", null);
    PopoverComponent = __decorate([
        core_1.Component({
            selector: "md-popover",
            host: {
                "[class.md-popover]": "true"
            },
            template: require("./popover.component.html"),
            styles: [require("./popover.component.scss")],
            encapsulation: core_1.ViewEncapsulation.None,
        }), 
        __metadata('design:paramtypes', [core_1.ChangeDetectorRef, core_1.ElementRef])
    ], PopoverComponent);
    return PopoverComponent;
}());
exports.PopoverComponent = PopoverComponent;
//# sourceMappingURL=popover.component.js.map