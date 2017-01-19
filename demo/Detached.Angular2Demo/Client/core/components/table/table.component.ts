import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ContentChild,
    ContentChildren,
    QueryList,
    TemplateRef,
} from '@angular/core';
import { ColumnComponent } from "./column.component";
import { CollectionComponent } from "../collection/collection.component";
import { SortDirection } from "../../datasources/collection.datasource";

@Component({
    selector: 'd-table',
    template: require("./table.component.html"),
    encapsulation: ViewEncapsulation.None,
    styles: [require("./table.component.scss")],
})
export class TableComponent extends CollectionComponent {

    @ContentChildren(ColumnComponent)
    private columns: Array<ColumnComponent> = [];

    toggleSort(canSort: boolean, propertyName: string) {
        if (canSort) {
            this.dataSource.toggleSort(propertyName);
        }
    }
}