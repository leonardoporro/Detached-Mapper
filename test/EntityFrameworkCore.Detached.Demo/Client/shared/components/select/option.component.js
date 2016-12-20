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
var OptionComponent = (function () {
    function OptionComponent(changeDetectionRef) {
        this.changeDetectionRef = changeDetectionRef;
        this.multiple = false;
        this.selected = false;
        this.onSelect = Function.prototype;
    }
    OptionComponent.prototype.setMultiple = function (multiple) {
        this.multiple = multiple;
        this.changeDetectionRef.detectChanges();
    };
    OptionComponent.prototype.updateSelected = function (value) {
        var _this = this;
        if (this.multiple) {
            this.selected = (value.map(function (v) { return _this.stringifyValue(v); }).indexOf(this.stringValue) != -1);
        }
        else {
            this.selected = this.value == value;
        }
        this.changeDetectionRef.detectChanges();
    };
    OptionComponent.prototype.ngAfterViewInit = function () {
        this.text = this.contentWrapper.nativeElement.textContent.trim();
    };
    Object.defineProperty(OptionComponent.prototype, "stringValue", {
        get: function () {
            return this.stringifyValue(this.value);
        },
        enumerable: true,
        configurable: true
    });
    OptionComponent.prototype.stringifyValue = function (value) {
        switch (typeof value) {
            case "number": return String(value);
            case "object": return JSON.stringify(value);
            default: return (!!value) ? String(value) : "";
        }
    };
    __decorate([
        core_1.Input("value"), 
        __metadata('design:type', Object)
    ], OptionComponent.prototype, "value", void 0);
    __decorate([
        core_1.ViewChild("contentWrapper"), 
        __metadata('design:type', core_1.ElementRef)
    ], OptionComponent.prototype, "contentWrapper", void 0);
    OptionComponent = __decorate([
        core_1.Component({
            selector: "mdl-option",
            host: {
                "[class.mdl-option__container]": "true"
            },
            template: require("./option.component.html")
        }), 
        __metadata('design:paramtypes', [core_1.ChangeDetectorRef])
    ], OptionComponent);
    return OptionComponent;
}());
exports.OptionComponent = OptionComponent;
//# sourceMappingURL=option.component.js.map