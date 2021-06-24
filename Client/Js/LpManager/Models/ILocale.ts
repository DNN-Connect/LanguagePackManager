export interface ILocale {
  LocaleId: number;
  Code: string;
  GenericLocaleId?: number;
}

export class Locale implements ILocale {
  LocaleId: number;
  Code: string;
  GenericLocaleId?: number;
    constructor() {
  this.LocaleId = -1;
  this.Code = "";
   }
}

