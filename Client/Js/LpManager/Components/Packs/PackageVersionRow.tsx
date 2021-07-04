import * as React from "react";
import { IPackageVersion } from "../../Models/IPackageVersion";
import { IPackageVersionLocaleTextCount } from "../../Models/IPackageVersionLocaleTextCount";

interface IPackageVersionRowProps {
  contextLocale: string;
  resources: any;
  baseServicepath: string;
  locale: string;
  packageVersion: IPackageVersion;
  textStat: IPackageVersionLocaleTextCount[];
}

const PackageVersionRow: React.FC<IPackageVersionRowProps> = (props) => {
  let textStat = <span></span>;
  let hasTexts = false;
  if (props.textStat.length > 0) {
    textStat = (
      <span>
        {props.textStat[0].NrTexts}/{props.packageVersion.NrTexts} (
        {(
          (100 * props.textStat[0].NrTexts) /
          (props.packageVersion.NrTexts as number)
        ).toFixed(0)}{" "}
        %)
      </span>
    );
    if (props.textStat[0].NrTexts > 0) {
      hasTexts = true;
    }
  }
  const date = new Date(props.packageVersion.ReleaseDate);
  return (
    <tr>
      <td>{props.packageVersion.Version}</td>
      <td>{textStat}</td>
      <td>{new Intl.DateTimeFormat(props.contextLocale).format(date)}</td>
      <td>
        {hasTexts ? (
          <a
            href={`${props.baseServicepath}Packs/Get?packageName=${props.packageVersion.PackageName}&version=${props.packageVersion.Version}&locale=${props.locale}`}
          >
            {props.resources.Download}
          </a>
        ) : null}
      </td>
    </tr>
  );
};

export default PackageVersionRow;
