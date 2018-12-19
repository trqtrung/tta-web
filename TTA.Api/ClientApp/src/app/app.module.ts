import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';

// material
import {LayoutModule} from '@angular/cdk/layout';
import {FormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MaterialModule} from './shared/material.module';
import { FlexLayoutModule } from '@angular/flex-layout';

// routing
import {AppRoutingModule} from './shared/app-routing.module';

// declare Product
import { ProductService } from './products/shared/product.service';
import { ProductsComponent } from './products/products.component';
import { ProductComponent } from './products/product/product.component';
// order
import { OrderService } from './orders/shared/order.service';
import { OrdersComponent } from './orders/orders.component';

// options
import {OptionListService} from './option-list/shared/option-list.service';
import { OptionListComponent } from './option-list/option-list.component';

// brands
import {BrandService} from './brands/shared/brand.service';
import { BrandsComponent } from './brands/brands.component';

// testing
import {AboutUsComponent} from './test/aboutus.component';
import { CustomersComponent } from './customers/customers.component';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductsComponent,
    AboutUsComponent,
    ProductComponent,
    OrdersComponent,
    OptionListComponent,
    BrandsComponent,
    CustomersComponent,
    HomeComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    HttpClientModule,
    AppRoutingModule,
    LayoutModule,
    BrowserAnimationsModule,
    MaterialModule,
    FlexLayoutModule
  ],
  entryComponents: [
    ProductComponent
  ],
  providers: [ProductService, OrderService, OptionListService, BrandService],
  bootstrap: [AppComponent]
})
export class AppModule { }
