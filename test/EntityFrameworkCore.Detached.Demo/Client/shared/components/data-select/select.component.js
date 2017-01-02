"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var popover_component_1 = require("../popover/popover.component");
var option_component_1 = require("./option.component");
var uniq = function (array) { return Array.from(new Set(array)); };
function randomId() {
    var S4 = function () { return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1); };
    return (S4() + S4());
}
exports.MDL_SELECT_VALUE_ACCESSOR = {
    provide: forms_1.NG_VALUE_ACCESSOR,
    useExisting: core_1.forwardRef(function () { return SelectComponent; }),
    multi: true
};
var SearchableComponent = (function () {
    function SearchableComponent(searchTimeout) {
        if (searchTimeout === void 0) { searchTimeout = 300; }
        this.clearTimeout = null;
        this.query = "";
        this.searchTimeout = searchTimeout;
    }
    SearchableComponent.prototype.updateSearchQuery = function (event) {
        var _this = this;
        if (this.clearTimeout) {
            clearTimeout(this.clearTimeout);
        }
        this.clearTimeout = setTimeout(function () {
            _this.query = "";
        }, this.searchTimeout);
        this.query += String.fromCharCode(event.keyCode).toLowerCase();
    };
    SearchableComponent.prototype.getSearchQuery = function () {
        return this.query;
    };
    return SearchableComponent;
}());
exports.SearchableComponent = SearchableComponent;
var SelectComponent = (function (_super) {
    __extends(SelectComponent, _super);
    function SelectComponent(changeDetectionRef) {
        _super.call(this);
        this.changeDetectionRef = changeDetectionRef;
        this.disabled = false;
        this.placeholder = "";
        this.multiple = false;
        this.change = new core_1.EventEmitter(true);
        this.text = "";
        this.textByValue = {};
        this.onChange = Function.prototype;
        this.onTouched = Function.prototype;
        this.focused = false;
        this.textfieldId = "mdl-textfield-" + randomId();
    }
    SelectComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.bindOptions();
        this.renderValue(this.ngModel);
        this.optionComponents.changes.subscribe(function () { return _this.bindOptions(); });
    };
    SelectComponent.prototype.onKeydown = function ($event) {
        if (!this.disabled && this.popoverComponent.isVisible) {
            var closeKeys = ["Escape", "Tab", "Enter"];
            var closeKeyCodes = [13, 27, 9];
            if (closeKeyCodes.indexOf($event.keyCode) != -1 || ($event.key && closeKeys.indexOf($event.key) != -1)) {
                this.popoverComponent.hide();
            }
            else if (!this.multiple) {
                if ($event.keyCode == 38 || ($event.key && $event.key == "ArrowUp")) {
                    this.onArrowUp($event);
                }
                else if ($event.keyCode == 40 || ($event.key && $event.key == "ArrowDown")) {
                    this.onArrowDown($event);
                }
                else if ($event.keyCode >= 31 && $event.keyCode <= 90) {
                    this.onCharacterKeydown($event);
                }
            }
        }
    };
    SelectComponent.prototype.onCharacterKeydown = function ($event) {
        var _this = this;
        this.updateSearchQuery($event);
        var optionsList = this.optionComponents.toArray();
        var filteredOptions = optionsList.filter(function (option) {
            return option.text.toLowerCase().startsWith(_this.getSearchQuery());
        });
        var selectedOption = optionsList.find(function (option) { return option.selected; });
        if (filteredOptions.length > 0) {
            var selectedOptionInFiltered = filteredOptions.indexOf(selectedOption) != -1;
            if (!selectedOptionInFiltered && !filteredOptions[0].selected) {
                this.onSelect($event, filteredOptions[0].value);
            }
        }
        $event.preventDefault();
    };
    SelectComponent.prototype.onArrowUp = function ($event) {
        var arr = this.optionComponents.toArray();
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].selected) {
                if (i - 1 >= 0) {
                    this.onSelect($event, arr[i - 1].value);
                }
                break;
            }
        }
        $event.preventDefault();
    };
    SelectComponent.prototype.onArrowDown = function ($event) {
        var arr = this.optionComponents.toArray();
        var selectedOption = arr.find(function (option) { return option.selected; });
        if (selectedOption) {
            var selectedOptionIndex = arr.indexOf(selectedOption);
            if (selectedOptionIndex + 1 < arr.length) {
                this.onSelect($event, arr[selectedOptionIndex + 1].value);
            }
        }
        else {
            this.onSelect($event, arr[0].value);
        }
        $event.preventDefault();
    };
    SelectComponent.prototype.addFocus = function () {
        this.focused = true;
    };
    SelectComponent.prototype.removeFocus = function () {
        this.focused = false;
    };
    SelectComponent.prototype.isEmpty = function () {
        return this.multiple ? !this.ngModel.length : !this.ngModel;
    };
    // rebind options and reset value in connected select
    SelectComponent.prototype.reset = function (resetValue) {
        if (resetValue === void 0) { resetValue = true; }
        if (resetValue && !this.isEmpty()) {
            this.ngModel = this.multiple ? [] : "";
            this.onChange(this.ngModel);
            this.change.emit(this.ngModel);
            this.renderValue(this.ngModel);
        }
    };
    SelectComponent.prototype.bindOptions = function () {
        var _this = this;
        this.optionComponents.forEach(function (selectOptionComponent) {
            selectOptionComponent.setMultiple(_this.multiple);
            selectOptionComponent.onSelect = _this.onSelect.bind(_this);
            if (selectOptionComponent.value != null) {
                _this.textByValue[_this.stringifyValue(selectOptionComponent.value)] = selectOptionComponent.text;
            }
        });
    };
    SelectComponent.prototype.renderValue = function (value) {
        var _this = this;
        if (this.multiple) {
            this.text = value.map(function (value) { return _this.textByValue[_this.stringifyValue(value)]; }).join(", ");
        }
        else {
            this.text = this.textByValue[this.stringifyValue(value)] || "";
        }
        this.changeDetectionRef.detectChanges();
        if (this.optionComponents) {
            this.optionComponents.forEach(function (selectOptionComponent) {
                selectOptionComponent.updateSelected(value);
            });
        }
    };
    SelectComponent.prototype.stringifyValue = function (value) {
        switch (typeof value) {
            case "number": return String(value);
            case "object": return JSON.stringify(value);
            default: return (!!value) ? String(value) : "";
        }
    };
    SelectComponent.prototype.toggle = function ($event) {
        if (!this.disabled) {
            this.popoverComponent.toggle($event);
            $event.stopPropagation();
        }
    };
    SelectComponent.prototype.open = function ($event) {
        if (!this.disabled && !this.popoverComponent.isVisible) {
            this.popoverComponent.show($event);
        }
    };
    SelectComponent.prototype.close = function ($event) {
        if (!this.disabled && this.popoverComponent.isVisible) {
            this.popoverComponent.hide();
        }
    };
    SelectComponent.prototype.onSelect = function ($event, value) {
        if (this.multiple) {
            // prevent popup close on click inside popover when selecting multiple
            $event.stopPropagation();
        }
        else {
            var popover = this.popoverComponent.elementRef.nativeElement;
            var list = popover.querySelector(".mdl-list");
            var option_1 = null;
            this.optionComponents.forEach(function (o) {
                // not great for long lists because break is not available
                if (o.value == value) {
                    option_1 = o.contentWrapper.nativeElement;
                }
            });
            if (option_1) {
                if (option_1.offsetTop > popover.clientHeight) {
                    list.scrollTop += option_1.parentElement.clientHeight;
                }
                else if (option_1.offsetTop < list.scrollTop) {
                    list.scrollTop -= option_1.parentElement.clientHeight;
                }
            }
        }
        this.writeValue(value);
        this.change.emit(this.ngModel);
    };
    SelectComponent.prototype.writeValue = function (value) {
        if (this.multiple) {
            this.ngModel = this.ngModel || [];
            if (!value || this.ngModel === value) {
            }
            else if (Array.isArray(value)) {
                this.ngModel = uniq(this.ngModel.concat(value));
            }
            else if (this.ngModel.indexOf(value) != -1) {
                this.ngModel = this.ngModel.filter(function (v) { return v !== value; }).slice();
            }
            else if (!!value) {
                this.ngModel = this.ngModel.concat([value]);
            }
        }
        else {
            this.ngModel = value;
        }
        this.onChange(this.ngModel);
        this.renderValue(this.ngModel);
    };
    SelectComponent.prototype.registerOnChange = function (fn) {
        this.onChange = fn;
    };
    SelectComponent.prototype.registerOnTouched = function (fn) {
        this.onTouched = fn;
    };
    SelectComponent.prototype.getLabelVisibility = function () {
        return this.isFloatingLabel == null || (this.isFloatingLabel != null && this.text != null && this.text.length > 0) ? "block" : "none";
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], SelectComponent.prototype, "ngModel", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean)
    ], SelectComponent.prototype, "disabled", void 0);
    __decorate([
        core_1.Input("floating-label"), 
        __metadata('design:type', Object)
    ], SelectComponent.prototype, "isFloatingLabel", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], SelectComponent.prototype, "placeholder", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Boolean)
    ], SelectComponent.prototype, "multiple", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', core_1.EventEmitter)
    ], SelectComponent.prototype, "change", void 0);
    __decorate([
        core_1.ViewChild(popover_component_1.PopoverComponent), 
        __metadata('design:type', popover_component_1.PopoverComponent)
    ], SelectComponent.prototype, "popoverComponent", void 0);
    __decorate([
        core_1.ContentChildren(option_component_1.OptionComponent), 
        __metadata('design:type', core_1.QueryList)
    ], SelectComponent.prototype, "optionComponents", void 0);
    __decorate([
        core_1.HostListener("document:keydown", ["$event"]), 
        __metadata('design:type', Function), 
        __metadata('design:paramtypes', [KeyboardEvent]), 
        __metadata('design:returntype', void 0)
    ], SelectComponent.prototype, "onKeydown", null);
    SelectComponent = __decorate([
        core_1.Component({
            selector: "mdl-select",
            host: {
                "[class.mdl-select]": "true",
                "[class.mdl-select--floating-label]": "isFloatingLabel != null"
            },
            template: require("./select.component.html"),
            styles: [require("./select.component.scss")],
            encapsulation: core_1.ViewEncapsulation.None,
            providers: [exports.MDL_SELECT_VALUE_ACCESSOR]
        }), 
        __metadata('design:paramtypes', [core_1.ChangeDetectorRef])
    ], SelectComponent);
    return SelectComponent;
}(SearchableComponent));
exports.SelectComponent = SelectComponent;
//# sourceMappingURL=select.component.js.map