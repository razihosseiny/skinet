import { IProduct } from './../shared/models/product';
import { Basket, IBasketItem, IBasketTotals } from './../shared/models/basket';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IBasket } from '../shared/models/basket';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketSource = new BehaviorSubject<IBasket>({ id: "0", items: [] });
  basket$ = this.basketSource.asObservable();
  private basketTotalSource = new BehaviorSubject<IBasketTotals>({ shipping: 0, total: 0, subtotal: 0 })
  basketTotal$ = this.basketTotalSource.asObservable();

  constructor(private http: HttpClient) { }


  getBasket(id: string) {
    return this.http.get<IBasket>(this.baseUrl + 'basket?basketid=' + id)
      .pipe(
        map((basket: IBasket) => {
          this.basketSource.next(basket);
          this.calculateTotals();
        })
      );
  }

  setBasket(basket: IBasket) {
    return this.http.post<IBasket>(this.baseUrl + 'basket', basket)
      .subscribe((response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      }, error => {
        console.log(error);
      })
  }
  getCurrentBasketValue() {
    //console.log('GetCurrentBasketFromBasketService');
    if (this.basketSource.value.id == "0")
      return null;
    else
      return this.basketSource.value;
  }
  addItemToBasket(item: IProduct, quantity = 1) {
    //console.log('AddItemFromBasketService');
    const itemToAdd: IBasketItem = mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }
  increamentItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket) {
      const findItemIndex = basket.items.findIndex(x => x.id == item.id)
      if (findItemIndex != -1) {
        basket.items[findItemIndex].quantity++;
        this.setBasket(basket);
      }
    }
  }
  decreamentItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket) {
      const findItemIndex = basket.items.findIndex(x => x.id == item.id)
      if (findItemIndex != -1) {
        if (basket.items[findItemIndex].quantity > 1) {
          basket.items[findItemIndex].quantity--;
          this.setBasket(basket);
        }
        else
          this.removeItemFromBasket(item);
      }
    }
  }
  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    if (basket) {
      if (basket.items.some(x => x.id === item.id)) {
        basket.items = basket.items.filter(i => i.id !== item.id);
        if (basket.items.length > 0)
          this.setBasket(basket);
        else
          this.deleteBasket(basket);
      }
    }
  }
  deleteBasket(basket: IBasket) {
    return this.http.delete(this.baseUrl + 'basket?id=' + basket.id).subscribe(() => {
      this.basketSource.next({
        id: '',
        items: []
      });
      this.basketTotalSource.next({
        shipping: 0,
        total: 0,
        subtotal: 0
      });
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    })
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    console.log(items);
    const index = items.findIndex(i => i.id === itemToAdd.id);
    if (index === -1) {
      //Add item to basket
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }
  private createBasket(): IBasket {
    console.log('CreateBasket');
    const basket = new Basket();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }
  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    if (basket) {
      const shipping = 0;
      const subtotal = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
      const total = subtotal! + shipping;
      this.basketTotalSource.next({ shipping, total, subtotal });
    }
  }
}

function mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
  return {
    id: item.id,
    productName: item.name,
    brand: item.productBrand,
    type: item.productType,
    price: item.price,
    pictureUrl: item.pictureUrl,
    quantity
  };
}


