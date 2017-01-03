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
var items_component_1 = require("../core/items.component");
var DataSelectComponent = (function (_super) {
    __extends(DataSelectComponent, _super);
    function DataSelectComponent(elementRef) {
        _super.call(this);
        this.elementRef = elementRef;
        this.textChange = new core_1.EventEmitter();
    }
    DataSelectComponent.prototype.ngOnInit = function () {
        this.selectionChange.subscribe(this.onSelectionChange.bind(this));
        var input = this.elementRef.nativeElement.querySelector("input");
        input.readOnly = true;
    };
    DataSelectComponent.prototype.onSelectionChange = function () {
        this.text = "";
        for (var _i = 0, _a = this._selection; _i < _a.length; _i++) {
            var selected = _a[_i];
            if (this.text !== "")
                this.text += ", ";
            this.text += selected[this.displayProperty];
        }
        this.textChange.emit(this.text);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], DataSelectComponent.prototype, "placeholder", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], DataSelectComponent.prototype, "text", void 0);
    __decorate([
        core_1.Output(), 
        __metadata('design:type', Object)
    ], DataSelectComponent.prototype, "textChange", void 0);
    DataSelectComponent = __decorate([
        core_1.Component({
            selector: 'md-data-select',
            template: require("./data-select.component.html"),
            encapsulation: core_1.ViewEncapsulation.None,
            styles: [require("./data-select.component.scss")],
        }), 
        __metadata('design:paramtypes', [core_1.ElementRef])
    ], DataSelectComponent);
    return DataSelectComponent;
}(items_component_1.ItemsComponent));
exports.DataSelectComponent = DataSelectComponent;
//# sourceMappingURL=data-select.component.js.map