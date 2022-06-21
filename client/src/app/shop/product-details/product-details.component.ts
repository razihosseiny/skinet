import { ActivatedRoute } from '@angular/router';
import { ShopService } from './../shop.service';
import { IProduct } from './../../shared/models/product';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product!: IProduct;

  constructor(private shopService: ShopService, private activateRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadProduct();
  }
  loadProduct() {
    let id = this.activateRoute.snapshot.paramMap.get('id');
    this.shopService.getProduct(((id) ? parseInt(id) : 0)).subscribe(product => {
      this.product = product;
    }, error => {
      console.log(error);
    })
  }
}
