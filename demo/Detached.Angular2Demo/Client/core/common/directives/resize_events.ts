import { Directive, ElementRef, Input, Output, EventEmitter, OnDestroy, OnInit } from "@angular/core";

@Directive({
    selector: "[resizeEvents]"
})
export class ResizeEventsDirective implements OnInit, OnDestroy {

    elementResizeDetector: any;

    constructor(private element: ElementRef) {
        let elementResizeDetectorType = require("element-resize-detector");
        this.elementResizeDetector = elementResizeDetectorType({
            strategy: "scroll"
        });
    }

    @Output()
    public onResize: EventEmitter<any> = new EventEmitter();

    ngOnInit() {
        this.elementResizeDetector.listenTo(this.element.nativeElement, this.onNativeResize.bind(this));
    }

    ngOnDestroy() {
        this.elementResizeDetector.removeListener(this.element.nativeElement, this.onNativeResize.bind(this));
    }

    onNativeResize(e) {
        this.onResize.emit(e); 
    }
}