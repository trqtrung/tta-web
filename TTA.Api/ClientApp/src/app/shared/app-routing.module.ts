import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {ProductsComponent} from '../products/products.component';
import {AboutUsComponent} from '../test/aboutus.component';
import {OrdersComponent} from '../orders/orders.component';
import {OptionListComponent} from '../option-list/option-list.component';
import {CustomersComponent} from '../customers/customers.component';
import {BrandsComponent} from '../brands/brands.component';
import {HomeComponent} from '../home/home.component';

const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'products', component: ProductsComponent},
    {path: 'aboutus', component: AboutUsComponent},
    {path: 'orders', component: OrdersComponent},
    {path: 'option-list', component: OptionListComponent},
    {path: 'brands', component: BrandsComponent},
    {path: 'customers', component: CustomersComponent}
];

@NgModule({
    exports: [RouterModule],
    imports: [RouterModule.forRoot(routes)]
})

export class AppRoutingModule {
}