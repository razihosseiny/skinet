import { IBasketItem } from './../shared/models/basket';
import { Observable } from 'rxjs';
import { BasketService } from './basket.service';
import { Component, OnInit } from '@angular/core';
import { IBasket } from '../shared/models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
  basket$!: Observable<IBasket>;
  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }
  increamentItemQuantity(item: IBasketItem) {
    console.log('increament');
    this.basketService.increamentItemQuantity(item);
  }
  decreamentItemQantity(item: IBasketItem) {
    console.log('decreament');
    this.basketService.decreamentItemQuantity(item);
  }
  removeBasketItem(item: IBasketItem) {
    this.basketService.removeItemFromBasket(item);
  }

}
