export interface IPackageVersion {
  PackageVersionId: number;
  PackageId: number;
  ContainedInPackageVersionId?: number;
  Version: string;
  ReleaseDate: Date;
  Downloaded: Date;
  NrTexts?: number;
  PackageName: string;
  FriendlyName: string;
  PackageType: string;
  PackageLinkName: string;
  LastChecked?: Date;
  ModuleId: number;
  PortalID?: number;
}

export class PackageVersion implements IPackageVersion {
  PackageVersionId: number;
  PackageId: number;
  ContainedInPackageVersionId?: number;
  Version: string;
  ReleaseDate: Date;
  Downloaded: Date;
  NrTexts?: number;
  PackageName: string;
  FriendlyName: string;
  PackageType: string;
  PackageLinkName: string;
  LastChecked?: Date;
  ModuleId: number;
  PortalID?: number;
    constructor() {
  this.PackageVersionId = -1;
  this.PackageId = -1;
  this.Version = "";
  this.ReleaseDate = new Date();
  this.Downloaded = new Date();
  this.PackageName = "";
  this.FriendlyName = "";
  this.PackageType = "";
  this.PackageLinkName = "";
  this.ModuleId = -1;
   }
}

