import * as React from "react";
import { IPackageVersion } from "../../Models/IPackageVersion";
import { IPackageVersionLocaleTextCount } from "../../Models/IPackageVersionLocaleTextCount";

interface IPackageVersionRowProps {
  baseServicepath: string;
  locale: string;
  packageVersion: IPackageVersion;
  textStat: IPackageVersionLocaleTextCount[];
}

const PackageVersionRow: React.FC<IPackageVersionRowProps> = (props) => {
  var textStat =
    props.textStat.length > 0 ? (
      <span>
        {props.textStat[0].NrTexts}/{props.packageVersion.NrTexts} (
        {(
          (100 * props.textStat[0].NrTexts) /
          (props.packageVersion.NrTexts as number)
        ).toFixed(0)}{" "}
        %)
      </span>
    ) : null;
  return (
    <tr>
      <td>{props.packageVersion.Version}</td>
      <td>{textStat}</td>
      <td>
        <a
          href={`${props.baseServicepath}Packs/Get?packageName=${props.packageVersion.PackageName}&version=${props.packageVersion.Version}&locale=${props.locale}`}
        >
          link
        </a>
      </td>
    </tr>
  );
};

export default PackageVersionRow;
