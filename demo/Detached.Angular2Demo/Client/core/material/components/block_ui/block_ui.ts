import {
    Component,
    Input,
    ViewEncapsulation
} from '@angular/core';

@Component({
    selector: 'md-block-ui',
    template: require("./block_ui.html"),
    encapsulation: ViewEncapsulation.None
})
export class MdBlockUIComponent {

    @Input()
    public visible: boolean = false;
}