import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError, tap  } from 'rxjs/operators';
import { OptionList } from './option-list.model';
import { of } from 'rxjs';
import {AppSettings} from '../../shared/app-settings';
import 'rxjs/add/operator/map';

const httpOptions = {
  headers: new HttpHeaders({ 'content-type': 'application/json'}) // , 'Authorization':`Bearer ${currentUser}` })
};

@Injectable()
export class OptionListService {

  private apiURL = AppSettings.API_URL + 'OptionLists/';

  constructor(private http: HttpClient) {
  }

  searchOptionList(filter = '', pageNumber = 0, pageSize = 25): Observable<OptionList[]> {
    const httpGetOptions = {
      headers: new HttpHeaders({ 'content-type': 'application/json'}), // , 'Authorization':`Bearer ${currentUser}` })
      params: new HttpParams()
        .set('filter', filter)
        .set('pageNumber', pageNumber.toString())
        .set('pageSize', pageSize.toString())
    };

    return this.http.get(this.apiURL, httpGetOptions).pipe(map(res => (res as OptionList[])));
    // .pipe(
    //   map(
    //   res => res['payload'])
    // );
  }

  getOptionLists(key) {
    console.log('get ' + key + ' options');
    return this.http.get(`${this.apiURL}${key}`, httpOptions);
  }

  getOptionListByID(id) {
      console.log('get option by id');
      return this.http.get(`${this.apiURL}${id}`, httpOptions);
  }
}