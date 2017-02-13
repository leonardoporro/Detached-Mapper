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
var forms_1 = require("@angular/forms");
var angular2localization_1 = require("angular2localization");
var FormErrorMessageDirective = (function () {
    function FormErrorMessageDirective(element, localizationService) {
        this.element = element;
        this.localizationService = localizationService;
    }
    Object.defineProperty(FormErrorMessageDirective.prototype, "errormsg", {
        get: function () {
            return this._formControl;
        },
        set: function (value) {
            this._formControl = value;
            this._formControl.statusChanges.subscribe(this.onStatusChange.bind(this));
        },
        enumerable: true,
        configurable: true
    });
    FormErrorMessageDirective.prototype.onStatusChange = function () {
        if (!(this._formControl.pristine || this._formControl.valid)) {
            for (var errorName in this._formControl.errors) {
                var errorValue = this._formControl.errors[errorName];
                if (errorValue) {
                    if (errorName == "server") {
                        this.element.nativeElement.innerHTML = errorValue;
                    }
                    else {
                        var args = {
                            field: this.fieldName
                        };
                        var errorKey = "core.validation." + errorName;
                        var errorText = this.localizationService.translate(errorKey, args);
                        this.element.nativeElement.innerHTML = errorText;
                    }
                }
            }
        }
        else {
            this.element.nativeElement.innerHTML = null;
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', forms_1.FormControl)
    ], FormErrorMessageDirective.prototype, "errormsg", null);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], FormErrorMessageDirective.prototype, "fieldName", void 0);
    FormErrorMessageDirective = __decorate([
        core_1.Directive({
            selector: "[errormsg]"
        }), 
        __metadata('design:paramtypes', [core_1.ElementRef, angular2localization_1.LocalizationService])
    ], FormErrorMessageDirective);
    return FormErrorMessageDirective;
}());
exports.FormErrorMessageDirective = FormErrorMessageDirective;
//# sourceMappingURL=form-errormsg.directive.js.map