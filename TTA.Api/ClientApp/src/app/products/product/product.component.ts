import { Component, EventEmitter, Inject, OnInit } from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {ProductService} from '../shared/product.service';
import {OptionListService} from '../../option-list/shared/option-list.service';
import {BrandService} from '../../brands/shared/brand.service';
import {Product} from '../shared/product.model';
import {OptionList} from '../../option-list/shared/option-list.model';
import {Brand} from '../../brands/shared/brand.model';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {

  product = new Product;

  brands: Brand[] = [];

  types: OptionList[] = [];

  public event: EventEmitter<any> = new EventEmitter();

  constructor(
    private productService: ProductService,
    private optionListService: OptionListService,
    private brandService: BrandService,
    public dialogRef: MatDialogRef<ProductComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
    ) {
      this.brandService.getBrands().subscribe(t => {
        this.brands = t as Brand[];
      });

      this.optionListService.getOptionLists('product-type-sub').subscribe(a => {
        this.types = a as OptionList[];
      });

    }

  ngOnInit() {
    if  (this.data.product) {
      console.log('product dialog ' + this.data);
      this.product = this.data.product;
      this.data = this.data.title;
    }
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    // this.product.position = this.productService.getProducts.dataLength();
    // pass post data to products component

    this.product.brandid = this.brands.find(x => x.name === this.product.brand).id;

    console.log('submit product ' + this.product);

      // add
    this.event.emit({data: this.product});

    this.dialogRef.close();
  }

  // categories = this.dataService.getCategories();

}
