import { v4 as uuidv4 } from 'uuid';

export interface IBasketItem {
    id: number;
    productName: string;
    price: number;
    quantity: number;
    pictureUrl: string;
    type: string;
    brand: string;
}

export interface IBasket {
    id: string;
    items: IBasketItem[];
    // RegDT: string;
}
export class Basket implements IBasket {
    id = uuidv4();
    items: IBasketItem[] = [];
    // RegDT = "";
}
export interface IBasketTotals {
    shipping: number;
    total: number;
    subtotal: number;
}


