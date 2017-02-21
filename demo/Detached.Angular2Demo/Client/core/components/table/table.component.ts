import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ElementRef,
    ContentChildren,
    QueryList
} from '@angular/core';
import { ColumnComponent, ColumnType } from "./column.component";
import { CollectionComponent } from "../collection/collection.component";
import { SortDirection } from "../../datasources/collection.datasource";

@Component({
    selector: 'd-table',
    template: require("./table.component.html"),
    encapsulation: ViewEncapsulation.None
})
export class TableComponent extends CollectionComponent {
    @ContentChildren(ColumnComponent)
    private set columnsQuery(value: QueryList<ColumnComponent>) {
        this.columns = value.toArray();
    }

    _columns: Array<ColumnComponent> = [];
    @Input()
    public get columns(): Array<ColumnComponent> {
        return this._columns;
    }
    public set columns(value: Array<ColumnComponent>) {
        this._columns = value;
        let index = 1;
        for (let column of this._columns) {
            // default for canSort
            if (column.canSort == undefined && column.property) {
                column.canSort = true;
            }
            // default for type
            if (column.type == undefined) {
                if (column.template)
                    column.type = ColumnType.Template;
                else
                    column.type = ColumnType.Text;
            }
            // default for title
            if (column.title == undefined && column.property) {
                column.title = column.property;
            }
            // default for size
            if (column.size == undefined) {
                switch (column.type) {
                    case ColumnType.Text:
                        column.size = "grow";
                        break;
                    default:
                        column.size = "stretch";
                        break;
                }
            }
            // default for minWidth
            if (column.minWidth == undefined) {
                switch (column.type) {
                    case ColumnType.Text:
                        column.minWidth = 300;
                        break;
                    case ColumnType.Number:
                        column.minWidth = 100;
                        break;
                    default:
                        column.minWidth = 100;
                        break;
                }
            }
            // default for priority
            if (column.priority == undefined) {
                column.priority = index;
            }

            if (column.visible == undefined) {
                column.visible = true;
            }
        }
        this.setColumnWidths();
    }

    toggleSort(column: ColumnComponent) {
        if (column.canSort && column.property) {
            this.dataSource.toggleSort(column.property);
        }
    }

    setColumnWidths() {
        let total: number = 100;
        let count: number = 0;

        for (let column of this._columns) {
            if (column.size == "grow") {
                count++;
            }
            else if (column.size.endsWith("%")) {
                let percentSize = parseInt(column.size.substr(0, column.size.length - 1));
                total -= percentSize;
            }
        }

        for (let column of this._columns) {
            switch (column.size) {
                case "grow":
                    column.clientSize = total / count + "%";
                    break;
                case "shrink":
                    column.clientSize = "auto";
                    break;
                default:
                    column.clientSize = column.size;
                    break;
            } 
        }
    }

    sizeChanged(e) {
        console.debug(e.clientWidth);
    }
}