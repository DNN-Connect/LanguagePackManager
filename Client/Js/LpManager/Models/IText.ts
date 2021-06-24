export interface IText {
  TextId: number;
  PackageVersionId: number;
  ResourceFileId: number;
  TextKey: string;
  OriginalValue: string;
  DeprecatedInVersionId?: number;
  FilePath: string;
  PackageId: number;
  FirstInVersion: string;
  DeprecatedInVersion: string;
}

export class Text implements IText {
  TextId: number;
  PackageVersionId: number;
  ResourceFileId: number;
  TextKey: string;
  OriginalValue: string;
  DeprecatedInVersionId?: number;
  FilePath: string;
  PackageId: number;
  FirstInVersion: string;
  DeprecatedInVersion: string;
    constructor() {
  this.TextId = -1;
  this.PackageVersionId = -1;
  this.ResourceFileId = -1;
  this.TextKey = "";
  this.FilePath = "";
  this.PackageId = -1;
  this.FirstInVersion = "";
  this.DeprecatedInVersion = "";
   }
}

