import { Component, Input, Output, EventEmitter, TemplateRef, ContentChild } from "@angular/core";
import { Selector } from "./selector";

@Component({
    selector: "checklist",
    template: require("./checklist.html"),
    inputs: ["source", "selection", "trackBy"],
    outputs: ["selectionChange"]
})
export class CheckListComponent extends Selector<any> {
    @Input()
    displayProperty: string = "name";

    t: TemplateRef<any>;
    @ContentChild(TemplateRef)
    public get itemTemplate(): TemplateRef<any> {
        return this.t;
    }
    public set itemTemplate(value: TemplateRef<any>) {
        this.t = value;
    }
}

