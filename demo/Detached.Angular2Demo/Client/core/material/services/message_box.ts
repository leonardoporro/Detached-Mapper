import { Injectable } from "@angular/core";
import { MdDialog, MdDialogRef } from "@angular/material";
import { Observable } from "rxjs/Observable";
import { MdMessageBoxComponent, MdMessageBoxOption } from "../components/message_box/message_box";

@Injectable()
export class MdMessageBoxService {
    constructor(private _dialog: MdDialog) {
    }

    public open(title: string, messages: string[], icon: string, options: MdMessageBoxOption[]): MdDialogRef<MdMessageBoxComponent> {
        let dialogRef: MdDialogRef<MdMessageBoxComponent> =
            this._dialog.open(MdMessageBoxComponent, { disableClose: true });

        dialogRef.componentInstance.title = title;
        dialogRef.componentInstance.messages = messages;
        dialogRef.componentInstance.icon = icon;
        dialogRef.componentInstance.options = options;

        return dialogRef;
    }

    public confirm(title: string, message: string): MdDialogRef<MdMessageBoxComponent> {
        return this.open(title, [message], null, [
                new MdMessageBoxOption("Yes", true),
                new MdMessageBoxOption("No", false)
            ]);
    }

    public error(title: string, message: string) {
        return this.open(title, [message], null, [
            new MdMessageBoxOption("OK", true)
        ]);
    }
}