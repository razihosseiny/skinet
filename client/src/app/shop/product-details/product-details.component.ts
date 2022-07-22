import { BasketService } from './../../basket/basket.service';
import { ActivatedRoute } from '@angular/router';
import { ShopService } from './../shop.service';
import { IProduct } from './../../shared/models/product';
import { Component, OnInit } from '@angular/core';
import { BreadcrumbService } from 'xng-breadcrumb';
@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  product!: IProduct;
  quantity = 1;

  constructor(private shopService: ShopService,
    private activateRoute: ActivatedRoute
    , private bcService: BreadcrumbService
    , private basketService: BasketService) {
    this.bcService.set('@productDetails', ' ');
  }

  ngOnInit(): void {
    this.loadProduct();
  }
  addItemToBasket() {
    this.basketService.addItemToBasket(this.product, this.quantity);
  }
  inreamentQuantity() {

    this.quantity++;
  }
  decreamentQuantity() {
    if (this.quantity > 1)
      this.quantity--;
  }
  loadProduct() {
    let id = this.activateRoute.snapshot.paramMap.get('id');
    this.shopService.getProduct(((id) ? parseInt(id) : 0)).subscribe(product => {
      this.product = product;
      this.bcService.set('@productDetails', product.name);
    }, error => {
      console.log(error);
    })
  }
}
