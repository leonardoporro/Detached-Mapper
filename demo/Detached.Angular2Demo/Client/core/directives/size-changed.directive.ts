import { Directive, ElementRef, Input, Output, EventEmitter } from "@angular/core";

@Directive({
    selector: "[sizeChanged]"
})
export class OnSizeChangedDirective {

    constructor(private element: ElementRef) {
        let elementResizeDetectorMaker = require("element-resize-detector");
        let erd = elementResizeDetectorMaker({
            strategy: "scroll"
        });
        erd.listenTo(element.nativeElement, this.onNativeSizeChanged.bind(this));
    }

    @Output()
    public onSizeChanged: EventEmitter<any> = new EventEmitter();

    onNativeSizeChanged(e) {
        this.onSizeChanged.emit(e);
    }
}