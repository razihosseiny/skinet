export interface IProduct {
    id: number;
    name: string;
    productType: string;
    productBrand: string;
    desc: string;
    price: number;
    pictureUrl: string;
    regDT: Date;
    upDT: Date;
    isActive: boolean;
}
