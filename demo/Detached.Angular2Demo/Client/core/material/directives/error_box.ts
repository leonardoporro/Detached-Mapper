import { Directive, EventEmitter, Output, Input, OnDestroy } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import { MdDialogRef } from "@angular/material";
import { MdMessageBoxService } from "../services/message_box";
import { MdMessageBoxComponent } from "../components/message_box/message_box";
import { IError } from "../../common/data/error";

@Directive({
    selector: "md-error-box",
    exportAs: "mdErrorBox"
})
export class MdErrorBoxDirective implements OnDestroy {

    private _errorsSubscription: Subscription;
    private _errorDialog: MdDialogRef<MdMessageBoxComponent>;

    constructor(private _msgBoxService: MdMessageBoxService) {
    }

    @Input()
    public title: string;

    @Input()
    public message: string;

    @Input()
    public set errors(values: Observable<IError>[]) {
        if (this._errorsSubscription)
            this._errorsSubscription.unsubscribe();

        if (values) {
            this._errorsSubscription = Observable.merge(...values)
                .subscribe(this.onError.bind(this));
        }
    }

    @Output()
    public ok: EventEmitter<any> = new EventEmitter();

    public show() {
        this._errorDialog = this._msgBoxService.error(this.title, this.message);

        this._errorDialog.afterClosed()
            .subscribe(result => {
                this._errorDialog = null;
            });
    }

    private onError(error: IError) {
        if (!this._errorDialog)
            this.show();

        this._errorDialog.componentInstance.messages.push(error.message);
    }

    public ngOnDestroy() {
        if (this._errorsSubscription) {
            this._errorsSubscription.unsubscribe();
        }
    }
}