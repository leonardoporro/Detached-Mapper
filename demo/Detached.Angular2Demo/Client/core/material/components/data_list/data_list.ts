import {
    Component,
    Input,
    ViewEncapsulation,
    ContentChild,
    TemplateRef,
    EventEmitter
} from '@angular/core';

@Component({
    selector: 'md-data-list',
    template: require("./data_list.html"),
    encapsulation: ViewEncapsulation.None
})
export class MdDataListComponent {

    @ContentChild(TemplateRef)
    public template: TemplateRef<any>;

    @Input()
    public placeholder: string;
}