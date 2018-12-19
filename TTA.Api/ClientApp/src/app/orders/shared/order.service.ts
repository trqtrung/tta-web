import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError, tap  } from 'rxjs/operators';
import { Order } from './order.model';
import { of } from 'rxjs';
import {AppSettings} from '../../shared/app-settings';
import 'rxjs/add/operator/map';

const httpOptions = {
  headers: new HttpHeaders({ 'content-type': 'application/json'}) // , 'Authorization':`Bearer ${currentUser}` })
};

@Injectable()
export class OrderService {

  private apiURL = AppSettings.API_URL + 'Orders/';

  constructor(private http: HttpClient) {
  }
// from: Date, to: Date, stage: string, pageSize = 25, pageNumber = 0
  getOrders() {
    console.log('get all orders service');

    // const httpGetOptions = {
    //   headers: new HttpHeaders({ 'content-type': 'application/json'}),
    //   params: new HttpParams()
    //     .set('from', from.toDateString())
    //     .set('to', to.toDateString())
    //     .set('stage', stage)
    //     .set('pageSize', pageSize.toString())
    //     .set('pageNumber', pageNumber.toString())
    // };

    return this.http.get(this.apiURL, httpOptions);
  }

  getOrderByID(id) {
    console.log('get order by id service');
    return this.http.get(`${this.apiURL}/${id}`, httpOptions).pipe(
      catchError(this.handleError('error while get order'))
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