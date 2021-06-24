export interface IPackage {
  PackageId: number;
  LinkId: number;
  PackageName: string;
  FriendlyName: string;
  PackageType: string;
  InstallPath: string;
  LastVersion: string;
  Name: string;
  LastChecked?: Date;
  ModuleId: number;
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
  LastChecked?: Date;
  ModuleId: number;
    constructor() {
  this.PackageId = -1;
  this.LinkId = -1;
  this.PackageName = "";
  this.FriendlyName = "";
  this.PackageType = "";
  this.Name = "";
  this.ModuleId = -1;
   }
}

