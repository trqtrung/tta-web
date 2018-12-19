import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Order } from './shared/order.model';
import { OrderService } from './shared/order.service';
import { DataSource } from '../../../node_modules/@angular/cdk/table';
import { Observable } from '../../../node_modules/rxjs';

import {MAT_MOMENT_DATE_FORMATS, MomentDateAdapter} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';

import {MatTableDataSource, MatSnackBar, MatPaginator, MatSort, MatDialog} from '@angular/material';
import {MatDatepickerInputEvent} from '@angular/material/datepicker';

import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';
import { CollectionViewer } from '@angular/cdk/collections';
import { catchError } from 'rxjs/operators';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css'],
  providers: [
    // The locale would typically be provided on the root module of your application. We do it at
    // the component level here, due to limitations of our example generation script.
    {provide: MAT_DATE_LOCALE, useValue: 'en-GB'},

    // `MomentDateAdapter` and `MAT_MOMENT_DATE_FORMATS` can be automatically provided by importing
    // `MatMomentDateModule` in your applications root module. We provide it at the component level
    // here, due to limitations of our example generation script.
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS},
  ]
})
export class OrdersComponent implements OnInit {

  orders: Order[];

  minDateFrom = new Date(2000, 0, 1);
  maxDateFrom = new Date(2020, 0, 1);
  minDateTo = new Date(2000, 0, 1);
  maxDateTo = new Date(2020, 0, 1);

  displayedColumns = ['order_no', 'id',  'client', 'stage', 'customer_name', 'payment_method', 'receive_payment', 'order_date', 'total'];

  public orderDatabase: OrderDatabase | null;
    public dataSource: OrderDataSource | null;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    @ViewChild('filter') filter: ElementRef;

  constructor(private orderService: OrderService) { }

  ngOnInit() {
    this.orderDatabase = new OrderDatabase(this.orderService);

    this.dataSource = new OrderDataSource(this.orderDatabase, this.paginator, this.sort);

    Observable.fromEvent(this.filter.nativeElement, 'keyup')
        .debounceTime(150)
        .distinctUntilChanged()
        .subscribe(() => {
          if (!this.dataSource) { return; }
          this.dataSource.filter = this.filter.nativeElement.value;
        });
  }
  getTotal() {
    const completedOrders = this.orderDatabase.data.filter(t => t.stage === 'completed');
    return completedOrders.map(t => t.total).reduce((acc, value) => acc + value, 0);
  }
  addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
    // this.events.push(`${type}: ${event.value}`);
    console.log(event.value);
  }
}

export class OrderDatabase {

  /** Stream that emits whenever the data has been modified. */
  public dataChange: BehaviorSubject<Order[]> = new BehaviorSubject<Order[]>([]);
  get data(): Order[] { return this.dataChange.value; }

  constructor(private orderService: OrderService) {
    orderService.getOrders().subscribe((data: Order[]) => {
        this.dataChange.next(data);
        console.log('data: ' + data);
      });
  }
}


export class OrderDataSource extends DataSource<any> {
  _filterChange = new BehaviorSubject('');
  get filter(): string { return this._filterChange.value; }
  set filter(filter: string) { this._filterChange.next(filter); }

  filteredData: Order[] = [];
  renderedData: Order[] = [];

  // private ordersSubject = new BehaviorSubject<Order[]>([]);
  // private loadingOrder = new BehaviorSubject<boolean>(false);

  // public loading$ = this.loadingOrder.asObservable();

  constructor(private _exampleDatabase: OrderDatabase,
              private _paginator: MatPaginator,
              private _sort: MatSort,
              ) {
    super();
    this._filterChange.subscribe(() => this._paginator.pageIndex = 0);
  }

  /** Connect function called by the table to retrieve one stream containing the data to render. */
  connect(collectionViewer: CollectionViewer): Observable<Order[]> {
    // Listen for any changes in the base data, sorting, filtering, or pagination
    const displayDataChanges = [
      this._exampleDatabase.dataChange,
      this._sort.sortChange,
      this._filterChange,
      this._paginator.page,
    ];

    return Observable.merge(...displayDataChanges).map(() => {
      // Filter data
      this.filteredData = this._exampleDatabase.data.slice().filter((item: Order) => {
        const searchStr = (item.clientOrderId + item.orderNo + item.client + item.customerName + item.stage).toLowerCase();
        return searchStr.indexOf(this.filter.toLowerCase()) !== -1;
      });

      // Sort filtered data
      const sortedData = this.sortData(this.filteredData.slice());

      // Grab the page's slice of the filtered sorted data.
      const startIndex = this._paginator.pageIndex * this._paginator.pageSize;
      this.renderedData = sortedData.splice(startIndex, this._paginator.pageSize);
      return this.renderedData;
    });
  }

  disconnect(collectionViewer: CollectionViewer): void {
    // this.ordersSubject.complete();
    // this.loadingOrder.complete();
  }

  // loadOrders(from: Date, to: Date, stage: string, pageSize: number, pageIndex: number) {
  //   this.loadingOrder.next(true);
  //   this.orderService.getOrders(from, to, stage, pageSize, pageIndex).pipe(
  //     catchError(() => of([])),
  //     finalize(() => this.loadingOrder.next(false))
  //   ).subscribe(orders => this.ordersSubject.next(orders));
  // }

  /** Returns a sorted copy of the database data. */
  sortData(data: Order[]): Order[] {
    if (!this._sort.active || this._sort.direction === '') { return data; }

    return data.sort((a, b) => {
      let propertyA: number|string|Date = '';
      let propertyB: number|string|Date = '';

      switch (this._sort.active) {
        case 'id': [propertyA, propertyB] = [a.clientOrderId, b.clientOrderId]; break;
        case 'order_no': [propertyA, propertyB] = [a.orderNo, b.orderNo]; break;
        case 'customer_name': [propertyA, propertyB] = [a.customerName, b.customerName]; break;
        case 'client': [propertyA, propertyB] = [a.client, b.client]; break;
        case 'stage': [propertyA, propertyB] = [a.stage, b.stage]; break;
        case 'receive_payment': [propertyA, propertyB] = [new Date(a.receivePayment), new Date(b.receivePayment)]; break;
        case 'order_date': [propertyA, propertyB] = [new Date(a.orderDate), new Date(b.orderDate)]; break;
      }

      const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
      const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

      return (valueA < valueB ? -1 : 1) * (this._sort.direction === 'asc' ? 1 : -1);
    });
  }
}
