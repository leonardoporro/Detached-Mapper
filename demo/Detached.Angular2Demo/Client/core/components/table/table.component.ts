import {
    Component,
    Input,
    Output,
    EventEmitter,
    HostListener,
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
        this.setDefaults();
        this.setColumnWidths();
        this.visibleColumnsChange.emit(null);
    }

    public visibleColumnsChange = new EventEmitter();
    public get visibleColumns(): Array<ColumnComponent> {
        let visible: Array<ColumnComponent> = [];

        for (let c of this._columns) {
            if (c.visible)
                visible.push(c);
        }

        return visible;
    } 

    toggleSort(column: ColumnComponent) {
        if (column.canSort && column.property) {
            this.dataSource.toggleSort(column.property);
        }
    }

    setDefaults() {
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
                        column.minWidth = 125;
                        break;
                    case ColumnType.Number:
                        column.minWidth = 50;
                        break;
                    default:
                        column.minWidth = 50;
                        break;
                }
            }
            // default for priority
            if (column.priority == undefined) {
                if (column.type == ColumnType.Template)
                    column.priority = 0;
                else
                    column.priority = index;
            }
            // default for visible
            if (column.visible == undefined) {
                column.visible = true;
            }

            index++;
        }
    }

    setColumnWidths() {
        let total: number = 100;
        let count: number = 0;

        for (let column of this._columns) {
            if (column.visible) {
                if (column.size == "grow") {
                    count++;
                }
                else if (column.size.endsWith("%")) {
                    let percentSize = parseInt(column.size.substr(0, column.size.length - 1));
                    total -= percentSize;
                }
            }
        }

        for (let column of this._columns) {
            if (column.visible) {
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
    }

    sizeChanged(nativeElement: any) {
        if (this._columns) {

            let minWidthAll: number = 0;
            let pVisibleColumn: ColumnComponent = null;
            let pHiddenColumn: ColumnComponent = null;

            for (let column of this._columns) {
                if (column.visible) {
                    minWidthAll += column.minWidth;
                    if (column.priority > 0 && (pVisibleColumn == null || column.priority > pVisibleColumn.priority)) {
                        pVisibleColumn = column;
                    }

                } else {
                    if (column.priority > 0 && (pHiddenColumn == null || column.priority < pHiddenColumn.priority)) {
                        pHiddenColumn = column;
                    }
                }
            }

            let delta = nativeElement.clientWidth - minWidthAll;

            if (delta < 0 && pVisibleColumn) {
                pVisibleColumn.visible = false;
                this.visibleColumnsChange.emit(null);
                this.setColumnWidths();
            }

            if (pHiddenColumn && delta > pHiddenColumn.minWidth) {
                pHiddenColumn.visible = true;
                this.visibleColumnsChange.emit(null);
                this.setColumnWidths();
            }
        }
    }
}
