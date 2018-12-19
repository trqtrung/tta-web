import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import {HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError, tap  } from 'rxjs/operators';
import { Product } from './product.model';
import { of } from 'rxjs';
import {AppSettings} from '../../shared/app-settings';
import 'rxjs/add/operator/map';

const httpOptions = {
  headers: new HttpHeaders({ 'content-type': 'application/json'}) // , 'Authorization':`Bearer ${currentUser}` })
};

const httpPostOptions = {
  headers: new HttpHeaders({
    'Content-Type' : 'application/x-www-form-urlencoded'
  })
};

@Injectable()
export class ProductService {

  private myAppUrl = AppSettings.API_URL + 'Products/';

  constructor(private http: HttpClient) {
  }

  // getProducts(): Observable<Product[]> {
  //   console.log('get all products service');
  //   // return this._http.get<Product[]>(this.myAppUrl + 'api/Products/').pipe(map(res => (res as Product[])));

  //     return this.http.get<Product[]>(this.myAppUrl + 'api/Products/').map(res => (res as Product[]))
  //       .pipe(
  //         catchError(this.handleError<Product[]>(`get Products error`))
  //     );
  // }

  getProducts() {
    console.log('get all products service');
    return this.http.get(this.myAppUrl, httpOptions);
  }

  getProductByID(id) {
    console.log('get product by id service');
    return this.http.get(`${this.myAppUrl}${id}`, httpOptions).pipe(
      catchError(this.handleError('error while adding product'))
    );
  }

  addProduct(product: Product): Observable<Product> {
    console.log('add product service');

    // const data: FormData = new FormData();
    // data.append('name', product.name);
    // data.append('description', product.description);

    const data = { Name: product.name, Description: product.description,  };

    console.log(data);

    return this.http.post<Product>(this.myAppUrl, product, httpOptions).pipe(
      catchError(this.handleError<Product>('error while adding product'))
    );
    // .subcribe(
    //   tap((p: Product) => console.log(`${p.name} has been added`)),
    //   catchError(this.handleError<Product>('error while adding product'))
    // );
  }

  updateProduct(product: Product): Observable<Product> {
    console.log('update product service');

    return this.http.put<Product>(`${this.myAppUrl}${product.id}`, product, httpOptions).pipe(
      catchError(this.handleError<Product>('error while updating product'))
    );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.log(error); // log to console instead
      // TODO: better job of transforming error for user consumption
      // this.log(`${operation} failed: ${error.message}`);
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
