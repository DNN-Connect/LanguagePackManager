export interface IResourceFile {
  ResourceFileId: number;
  PackageId: number;
  FilePath: string;
}

export class ResourceFile implements IResourceFile {
  ResourceFileId: number;
  PackageId: number;
  FilePath: string;
    constructor() {
  this.ResourceFileId = -1;
  this.PackageId = -1;
  this.FilePath = "";
   }
}

