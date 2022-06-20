import { IProductTypes } from './../shared/models/productTypes';
import { IProductBrands } from './../shared/models/produtBrands';
import { ShopService } from './shop.service';
import { IProduct } from '../shared/models/product';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ShopParams } from '../shared/models/ShopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  @ViewChild('search', { static: false }) searchTerm!: ElementRef;
  products: IProduct[] = [];
  productBrands: IProductBrands[] = [];
  productTypes: IProductTypes[] = [];
  shopParams = new ShopParams();
  totalCount!: number;
  sortOptions = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price:Low to High', value: 'priceAsc' },
    { name: 'Price:High to Low', value: 'priceDesc' }
  ]

  constructor(private shopSevice: ShopService) { }

  ngOnInit() {
    this.getProducts();
    this.getProductBrands();
    this.getProductTypes();
  }
  getProducts() {
    this.shopSevice.getProducts(this.shopParams)
      .subscribe(response => {
        if (response?.data) {
          this.products = response.data;
          this.shopParams.pageNumber = response.pageIndex;
          this.shopParams.pageSize = response.pageSize;
          this.totalCount = response.count;
        }
      }, error => {
        console.log(error);
      });
  }

  getProductBrands() {
    this.shopSevice.getProductBrands().subscribe(response => {
      this.productBrands = [{ id: 0, name: 'All', isActive: true }, ...response];
    }, error => {
      console.log(error);
    })
  }

  getProductTypes() {
    this.shopSevice.getProductTypes().subscribe(response => {
      this.productTypes = [{ id: 0, name: 'All', isActive: true, parentID: 0 }, ...response];
    }, error => {
      console.log(error);
    })
  }

  OnBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  OnTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  OnSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }
  OnPageChanged(event: any) {
    if (this.shopParams.pageNumber !== event) {
      this.shopParams.pageNumber = event;
      this.getProducts();
    }
  }
  OnSearch() {
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts()
  }

  OnReset() {
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts()

  }
}
