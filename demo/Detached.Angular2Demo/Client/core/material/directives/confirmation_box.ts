import { Directive, EventEmitter, Output, Input } from "@angular/core";
import { MdDialog, MdDialogRef } from "@angular/material";
import { Observable } from "rxjs/Observable";

import { MdMessageBoxService } from "../services/message_box";

@Directive({
    selector: "md-confirmation-box",
    exportAs: "mdConfirmationBox"
})
export class MdConfirmationBoxDirective {

    constructor(private _msgBoxService: MdMessageBoxService) {
    }

    @Input()
    public title: string;

    @Input()
    public message: string;

    @Output()
    public ok: EventEmitter<any> = new EventEmitter();

    @Output()
    public cancel: EventEmitter<any> = new EventEmitter();

    public show(data?: any) {

        this._msgBoxService.confirm(this.title, this.message)
            .afterClosed()
            .subscribe(result => {
                if (result) {
                    this.ok.emit(data);
                } else {
                    this.cancel.emit(data);
                }
            });
    }
}