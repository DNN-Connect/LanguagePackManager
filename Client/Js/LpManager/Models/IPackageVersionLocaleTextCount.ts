export interface IPackageVersionLocaleTextCount {
  PackageVersionId: number;
  LocaleId: number;
  NrTexts: number;
  LastChange: string;
  OriginalNr?: number;
  PackageId: number;
}

export class PackageVersionLocaleTextCount implements IPackageVersionLocaleTextCount {
  PackageVersionId: number;
  LocaleId: number;
  NrTexts: number;
  LastChange: string;
  OriginalNr?: number;
  PackageId: number;
    constructor() {
  this.PackageVersionId = -1;
  this.LocaleId = -1;
  this.NrTexts = -1;
  this.LastChange = "";
  this.PackageId = -1;
   }
}

