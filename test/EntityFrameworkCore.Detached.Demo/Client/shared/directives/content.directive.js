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
var ContentDirective = (function () {
    function ContentDirective(_viewContainer) {
        this._viewContainer = _viewContainer;
        this._hasView = false;
    }
    Object.defineProperty(ContentDirective.prototype, "condition", {
        set: function (value) {
            if (value)
                this.show();
            else
                this.hide();
        },
        enumerable: true,
        configurable: true
    });
    ContentDirective.prototype.show = function () {
        if (!this._hasView) {
            this._hasView = true;
            var view = this._viewContainer.createEmbeddedView(this.template);
            view.context.model = this.model;
            view.context.index = this.index;
        }
    };
    ContentDirective.prototype.hide = function () {
        if (this._hasView) {
            this._hasView = false;
            this._viewContainer.clear();
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], ContentDirective.prototype, "model", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], ContentDirective.prototype, "index", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', core_1.TemplateRef)
    ], ContentDirective.prototype, "template", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean), 
        __metadata('design:paramtypes', [Boolean])
    ], ContentDirective.prototype, "condition", null);
    ContentDirective = __decorate([
        core_1.Directive({
            selector: "d-content"
        }), 
        __metadata('design:paramtypes', [core_1.ViewContainerRef])
    ], ContentDirective);
    return ContentDirective;
}());
exports.ContentDirective = ContentDirective;
//# sourceMappingURL=content.directive.js.map