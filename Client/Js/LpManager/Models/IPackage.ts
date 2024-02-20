export interface IPackage {
  PackageId: number;
  LinkId: number;
  PackageName: string;
  FriendlyName: string;
  PackageType: string;
  InstallPath: string;
  LastVersion: string;
  Name: string;
  IsResourcesRepo: boolean;
  LastChecked?: Date;
  ModuleId: number;
  PortalID?: number;
}

export class Package implements IPackage {
  PackageId: number;
  LinkId: number;
  PackageName: string;
  FriendlyName: string;
  PackageType: string;
  InstallPath: string;
  LastVersion: string;
  Name: string;
  IsResourcesRepo: boolean;
  LastChecked?: Date;
  ModuleId: number;
  PortalID?: number;
    constructor() {
  this.PackageId = -1;
  this.LinkId = -1;
  this.PackageName = "";
  this.FriendlyName = "";
  this.PackageType = "";
  this.Name = "";
  this.IsResourcesRepo = false;
  this.ModuleId = -1;
   }
}

