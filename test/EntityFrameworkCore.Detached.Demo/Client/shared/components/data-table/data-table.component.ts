import {
    Component,
    Input,
    Output,
    EventEmitter,
    ViewEncapsulation,
    ContentChild,
    ContentChildren,
    Query, QueryList,
    TemplateRef,
} from '@angular/core';
import { ColumnDirective, ColumnOrder } from "../core/column.directive";
import { ItemsComponent, Item } from "../core/items.component";

@Component({
    selector: 'md-data-table',
    template: require("./data-table.component.html"),
    encapsulation: ViewEncapsulation.None,
    styles: [require("./data-table.component.scss")],
})
export class DataTableComponent extends ItemsComponent {

    @ContentChildren(ColumnDirective)
    private _columnQuery: QueryList<ColumnDirective>;
    private _columns: Array<ColumnDirective> = [];
    private _orderBy: string;

    @Input()
    public get columns(): Array<ColumnDirective> {
        return this._columns;
    }
    public set columns(value: Array<ColumnDirective>) {
        if (this._columns) {
            for (let column of this._columns) {
                column.orderChanged.unsubscribe();
            }
        }
        this._columns = value;
        if (this._columns) {
            for (let column of this._columns) {
                column.orderChanged.subscribe(this.columnOrderChanged.bind(this));
            }
        }
        this.syncOrderBy();
    }

    @Input()
    public get orderBy(): string {
        return this._orderBy;
    }
    public set orderBy(value: string) {
        if (this._orderBy !== value) {
            this._orderBy = value;
            this.syncOrderBy();
            this.orderByChanged.emit(value);
        }
    }

    @Output()
    public orderByChanged = new EventEmitter();

    ngAfterContentInit() {
        if (this._columnQuery)
            this.columns = this._columnQuery.toArray();
    }

    columnOrderChanged(sortedColumn: ColumnDirective) {
        if (sortedColumn.order !== ColumnOrder.None) {
            for (let column of this._columns) {
                if (sortedColumn !== column) {
                    column.order = ColumnOrder.None;
                }
            }
            this._orderBy = (sortedColumn.order == ColumnOrder.Asc ? "+" : "-") + sortedColumn.property;
            this.orderByChanged.emit(this.orderBy);
        }
    }

    syncOrderBy() {
        if (this._columns && this._orderBy) {
            let order = this._orderBy.substr(0, 1);
            let property = this._orderBy.substr(1);

            for (let column of this._columns) {
                if (column.property == property) {
                    if (order == "+")
                        column.order = ColumnOrder.Asc;
                    else
                        column.order = ColumnOrder.Desc;

                    return;
                }
            }
        }
    }
}