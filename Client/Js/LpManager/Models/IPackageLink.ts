export interface IPackageLink {
  PackageLinkId: number;
  ModuleId: number;
  Name: string;
  OrgName: string;
  RepoName: string;
  AssetRegex: string;
  LastChecked?: Date;
  LastDownloadedVersion: string;
  IsResourcesRepo: boolean;
  CreatedByUserID: number;
  CreatedOnDate: Date;
  LastModifiedByUserID: number;
  LastModifiedOnDate: Date;
  PortalID?: number;
  CreatedByUser: string;
  ModifiedByUser: string;
}

export class PackageLink implements IPackageLink {
  PackageLinkId: number;
  ModuleId: number;
  Name: string;
  OrgName: string;
  RepoName: string;
  AssetRegex: string;
  LastChecked?: Date;
  LastDownloadedVersion: string;
  IsResourcesRepo: boolean;
  CreatedByUserID: number;
  CreatedOnDate: Date;
  LastModifiedByUserID: number;
  LastModifiedOnDate: Date;
  PortalID?: number;
  CreatedByUser: string;
  ModifiedByUser: string;
    constructor() {
  this.PackageLinkId = -1;
  this.ModuleId = -1;
  this.Name = "";
  this.OrgName = "";
  this.RepoName = "";
  this.AssetRegex = "";
  this.IsResourcesRepo = false;
  this.CreatedByUserID = -1;
  this.CreatedOnDate = new Date();
  this.LastModifiedByUserID = -1;
  this.LastModifiedOnDate = new Date();
   }
}

