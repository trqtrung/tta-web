import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Product } from './shared/product.model';
import { ProductComponent} from './product/product.component';
import { ProductService } from './shared/product.service';
import { DataSource } from '../../../node_modules/@angular/cdk/table';
import { Observable } from '../../../node_modules/rxjs';

import {MatTableDataSource, MatSnackBar, MatPaginator, MatSort, MatDialog} from '@angular/material';

import 'rxjs/add/observable/merge';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/operator/distinctUntilChanged';
import 'rxjs/add/operator/debounceTime';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})

export class ProductsComponent implements OnInit {

  products: Product[];

  constructor(private productService: ProductService,
    public dialog: MatDialog) {

  }

  displayedColumns = ['id', 'name', 'sku', 'brand', 'description', 'price', 'buyingPrice'];
  public productDatabase: ProductDatabase | null;
    public dataSource: ProductDataSource | null;

    @ViewChild(MatPaginator) paginator: MatPaginator;
    @ViewChild(MatSort) sort: MatSort;
    @ViewChild('filter') filter: ElementRef;

  ngOnInit() {
    // this.getProducts();

    this.productDatabase = new ProductDatabase(this.productService);

    this.dataSource = new ProductDataSource(this.productDatabase, this.paginator, this.sort);

    Observable.fromEvent(this.filter.nativeElement, 'keyup')
        .debounceTime(150)
        .distinctUntilChanged()
        .subscribe(() => {
          if (!this.dataSource) { return; }
          this.dataSource.filter = this.filter.nativeElement.value;
        });
  }

  getProducts(): void {
    this.productService.getProducts().subscribe((data: Product[]) => {
      this.products = data;
      console.log('get products result: ' + data);
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(ProductComponent, {
      width: '800px',
      maxHeight: '1000px',
      data: 'Add Product'
    });
    dialogRef.componentInstance.event.subscribe((result) => {
      console.log('dialog data: ' + result.data );



        console.log('add new product');
        this.productService.addProduct(result.data).subscribe((res) => {
          console.log('res ' + res);
        }, (error) => {
          console.log('error ' + error);
          });
          // this.dataSource = new ProductDataSource(this.productDatabase, this.paginator, this.sort);
    });
  }

  selectProduct(row) {
    console.log('selected ' + row);
    const dialogRef = this.dialog.open(ProductComponent, {
      width: '800px',
      height: '85%',
      data: {
        title: 'Product Details',
        product : row
      }
    });
    dialogRef.componentInstance.event.subscribe((result) => {
      console.log('dialog data: ' + result.data );
      console.log('update product id: ' + result.data.id);
        this.productService.updateProduct(result.data).subscribe((res) => {
          console.log('res ' + res);
        }, (error) => {
          console.log('error ' + error);
          });
        });
  }
}

export class ProductDatabase {

  /** Stream that emits whenever the data has been modified. */
  public dataChange: BehaviorSubject<Product[]> = new BehaviorSubject<Product[]>([]);
  get data(): Product[] { return this.dataChange.value; }

  constructor(private productService: ProductService) {
      productService.getProducts().subscribe((data: Product[]) => {
        this.dataChange.next(data);
      });
  }
}


export class ProductDataSource extends DataSource<any> {
  _filterChange = new BehaviorSubject('');
  get filter(): string { return this._filterChange.value; }
  set filter(filter: string) { this._filterChange.next(filter); }

  filteredData: Product[] = [];
  renderedData: Product[] = [];

  constructor(private _exampleDatabase: ProductDatabase,
              private _paginator: MatPaginator,
              private _sort: MatSort) {
    super();
    this._filterChange.subscribe(() => this._paginator.pageIndex = 0);
  }

  /** Connect function called by the table to retrieve one stream containing the data to render. */
  connect(): Observable<Product[]> {
    // Listen for any changes in the base data, sorting, filtering, or pagination
    const displayDataChanges = [
      this._exampleDatabase.dataChange,
      this._sort.sortChange,
      this._filterChange,
      this._paginator.page,
    ];

    return Observable.merge(...displayDataChanges).map(() => {
      // Filter data
      this.filteredData = this._exampleDatabase.data.slice().filter((item: Product) => {
        const searchStr = (item.id + item.sku + item.name + item.brand + item.description).toLowerCase();
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

  disconnect() {}

  /** Returns a sorted copy of the database data. */
  sortData(data: Product[]): Product[] {
    if (!this._sort.active || this._sort.direction === '') { return data; }

    return data.sort((a, b) => {
      let propertyA: number|string = '';
      let propertyB: number|string = '';

      switch (this._sort.active) {
        case 'id': [propertyA, propertyB] = [a.id, b.id]; break;
        case 'sku' : [propertyA, propertyB] = [a.sku, b.sku]; break;
        case 'name': [propertyA, propertyB] = [a.name, b.name]; break;
        case 'desciption': [propertyA, propertyB] = [a.description, b.description]; break;
        case 'brand': [propertyA, propertyB] = [a.brand, b.brand]; break;
        // case 'price': [propertyA, propertyB] = [a.price, b.price]; break;
      }

      const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
      const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

      return (valueA < valueB ? -1 : 1) * (this._sort.direction === 'asc' ? 1 : -1);
    });
  }
}
