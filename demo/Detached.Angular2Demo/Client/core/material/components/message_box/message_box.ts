import { Directive, Component, EmbeddedViewRef, TemplateRef, ViewContainerRef, Input, ContentChild, Injectable } from "@angular/core";
import { MdDialog, MdDialogRef } from "@angular/material";
import { Observable } from "rxjs/Observable";

@Component({
    selector: "md-message-box-host",
    template: require("./message_box.html")
})
export class MdMessageBoxComponent {

    constructor(private _dialogRef: MdDialogRef<MdMessageBoxComponent>) {
    }

    @Input()
    public title: string;

    @Input()
    public messages: string[] = [];

    @Input()
    public icon: string;

    @Input()
    public options: MdMessageBoxOption[];

    public close(result: any) {
        this._dialogRef.close(result);
    }
}

export class MdMessageBoxOption {
    constructor(
        public label: string,
        public value: any,
        public isDefault: boolean = false) {
    }
}