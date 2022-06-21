import { IProduct } from './../shared/models/product';
import { IProductBrands } from './../shared/models/produtBrands';
import { IPagination } from '../shared/models/pagination';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IProductTypes } from '../shared/models/productTypes';
import { map } from 'rxjs/operators';
import { ShopParams } from '../shared/models/ShopParams';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:7268/api/';

  constructor(private http: HttpClient) { }

  getProducts(shopParams: ShopParams) {
    let params = new HttpParams();
    if (shopParams.brandId !== 0) {
      params = params.append('brandId', shopParams.brandId.toString());
    }
    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString());
    }
    if (shopParams.search) {
      params = params.append('search', shopParams.search);
    }
    params = params.append('sort', shopParams.sort);
    params = params.append('pageSize', shopParams.pageSize.toString());
    params = params.append('pageIndex', shopParams.pageNumber.toString());

    return this.http.get<IPagination>(this.baseUrl + 'product',
      { observe: 'response', params })
      .pipe(
        map(response => { return response.body; })
      );
  }
  getProduct(id: number) {
    return this.http.get<IProduct>(this.baseUrl + "product/" + id);
  }
  getProductBrands() {
    return this.http.get<IProductBrands[]>(this.baseUrl + 'product/productbrands')
  }
  getProductTypes() {
    return this.http.get<IProductTypes[]>(this.baseUrl + 'product/ProductTypes')
  }
}
