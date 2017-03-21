import {
    Component, Directive, ChangeDetectorRef,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    AfterViewInit,
    ElementRef,
    ContentChildren, ViewChildren, OnChanges, SimpleChange, SimpleChanges, OnInit, OnDestroy,
    QueryList
} from '@angular/core';

import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import { ICollection, SortOrder } from "../../../common/data/collection";
import { ISelection } from "../../../common/data/selection";
import { MdDataColumnComponent, ColumnType } from "./data_column";

import { ItemsControlComponent } from "../../../common/components/items_control";
import { ItemComponent } from "../../../common/components/item";


@Component({
    selector: 'md-data-table',
    template: require("./data_table.html"),
    encapsulation: ViewEncapsulation.None
})
export class MdDataTableComponent extends ItemsControlComponent implements AfterViewInit {

    @ContentChildren(MdDataColumnComponent)
    public columns: QueryList<MdDataColumnComponent>

    public ngAfterViewInit() {
        super.ngAfterViewInit();

        this.columns.forEach((col, index) => {
            col.index = index + 1;
            col.setup()
        });
        this.updateColumnWidths();
    }

    toggleSort(column: MdDataColumnComponent) {
        if (this.collection) {
            this.collection.params.sortBy = column.property;
            if (this.collection.params.sortOrder == SortOrder.Desc)
                this.collection.params.sortOrder = SortOrder.Asc;
            else
                this.collection.params.sortOrder = SortOrder.Desc;
            this.collection.load();
        }
    }

    updateColumnWidths() {
        let total: number = 100;
        let count: number = 0;

        this.columns.forEach((column) => {
            if (column.visible) {
                if (column.size == "grow") {
                    count++;
                }
                else if (column.size.endsWith("%")) {
                    let percentSize = parseInt(column.size.substr(0, column.size.length - 1));
                    total -= percentSize;
                }
            }
        });

        this.columns.forEach((column) => {
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
        });
    }

    onResize(nativeElement: any) {
        if (this.columns) {

            let minWidthAll: number = 0;
            let pVisibleColumn: MdDataColumnComponent = null;
            let pHiddenColumn: MdDataColumnComponent = null;

            this.columns.forEach((column) => {
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
            });

            let delta = nativeElement.clientWidth - minWidthAll;

            if (delta < 0 && pVisibleColumn) {
                pVisibleColumn.visible = false;
                this.updateColumnWidths();
            }

            if (pHiddenColumn && delta > pHiddenColumn.minWidth) {
                pHiddenColumn.visible = true;
                this.updateColumnWidths();
            }
        }
    }
} 