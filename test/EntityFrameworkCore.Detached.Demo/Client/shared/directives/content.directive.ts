import { Directive, Input, TemplateRef, ViewContainerRef, ChangeDetectorRef } from '@angular/core';

@Directive({
    selector: "d-content"
})
export class ContentDirective {

    constructor(private _viewContainer: ViewContainerRef) {
    }

    private _hasView = false;

    @Input()
    public model: any;

    @Input()
    public index: number;

    @Input()
    public template: TemplateRef<any>;

    @Input()
    public set condition(value: boolean) {
        if (value)
            this.show();
        else
            this.hide();
    }

    show() {
        if (!this._hasView) {
            this._hasView = true;
            let view = this._viewContainer.createEmbeddedView(this.template);
            view.context.model = this.model;
            view.context.index = this.index;
        }
    }

    hide() {
        if (this._hasView) {
            this._hasView = false;
            this._viewContainer.clear();
        }
    }
}