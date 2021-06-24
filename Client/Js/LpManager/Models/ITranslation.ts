export interface ITranslation {
  TextId: number;
  Locale: number;
  TextValue: string;
  CreatedByUserID: number;
  CreatedOnDate: Date;
  LastModifiedByUserID: number;
  LastModifiedOnDate: Date;
  PackageId: number;
  FilePath: string;
  TextKey: string;
  FirstInVersion: string;
  DeprecatedInVersion: string;
  CreatedByUser: string;
  ModifiedByUser: string;
}

export class Translation implements ITranslation {
  TextId: number;
  Locale: number;
  TextValue: string;
  CreatedByUserID: number;
  CreatedOnDate: Date;
  LastModifiedByUserID: number;
  LastModifiedOnDate: Date;
  PackageId: number;
  FilePath: string;
  TextKey: string;
  FirstInVersion: string;
  DeprecatedInVersion: string;
  CreatedByUser: string;
  ModifiedByUser: string;
    constructor() {
  this.TextId = -1;
  this.Locale = -1;
  this.CreatedByUserID = -1;
  this.CreatedOnDate = new Date();
  this.LastModifiedByUserID = -1;
  this.LastModifiedOnDate = new Date();
  this.PackageId = -1;
  this.FilePath = "";
  this.TextKey = "";
  this.FirstInVersion = "";
  this.DeprecatedInVersion = "";
   }
}

