import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Subscription } from "rxjs/Subscription";

@Component({
    selector: "item",
    template: "<ng-content></ng-content>",
    exportAs: "listItem"
})
export class ItemComponent {
    private _selected: boolean = false;
    private _model: any;

    @Input()
    public get selected(): boolean {
        return this._selected;
    }
    public set selected(value: boolean) {
        if (this._selected != value) {
            this._selected = value;
            this.selectedChange.emit(this);
        }
    }
    @Output()
    public selectedChange: EventEmitter<ItemComponent> = new EventEmitter();

    @Input()
    public get model(): boolean {
        return this._model;
    }
    public set model(value: boolean) {
        if (this._model != value) {
            this._model = value;
            this.modelChange.emit(this);
        }
    }
    @Output()
    public modelChange: EventEmitter<ItemComponent> = new EventEmitter();
}