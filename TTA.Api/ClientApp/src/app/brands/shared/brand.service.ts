import { Injectable, Inject } from '@angular/core';
import { Http, Response } from '@angular/http';
import {HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError, tap  } from 'rxjs/operators';
import { Brand } from './brand.model';
import { of } from 'rxjs';
import {AppSettings} from '../../shared/app-settings';
import 'rxjs/add/operator/map';

const httpOptions = {
  headers: new HttpHeaders({ 'content-type': 'application/json'}) // , 'Authorization':`Bearer ${currentUser}` })
};

@Injectable()
export class BrandService {

  private apiURL = AppSettings.API_URL + 'Brands/';

  constructor(private http: HttpClient) {
  }

  getBrands() {
    console.log('get brands service');
    return this.http.get(this.apiURL, httpOptions);
  }

  getBrandID(id) {
      console.log('get brand by id');
      return this.http.get(`${this.apiURL}/${id}`, httpOptions);
  }

}