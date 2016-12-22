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
var PresenterDirective = (function () {
    function PresenterDirective(viewContainer, cdr) {
        this.viewContainer = viewContainer;
        this.cdr = cdr;
    }
    PresenterDirective.prototype.ngOnInit = function () {
        this.cdr.detach();
    };
    PresenterDirective.prototype.ngAfterViewInit = function () {
        var _this = this;
        var view = this.viewContainer.createEmbeddedView(this.template);
        view.context.item = this.item;
        setTimeout(function () { return _this.cdr.reattach(); });
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], PresenterDirective.prototype, "item", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', core_1.TemplateRef)
    ], PresenterDirective.prototype, "template", void 0);
    PresenterDirective = __decorate([
        core_1.Directive({
            selector: "presenter"
        }), 
        __metadata('design:paramtypes', [core_1.ViewContainerRef, core_1.ChangeDetectorRef])
    ], PresenterDirective);
    return PresenterDirective;
}());
exports.PresenterDirective = PresenterDirective;
//# sourceMappingURL=presenter.directive.js.map