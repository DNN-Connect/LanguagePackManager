import * as React from 'react';
import { IAppModule } from '../../Models/IAppModule';
import { IPackageVersion } from "../../Models/IPackageVersion";
import { IPackageVersionLocaleTextCount } from "../../Models/IPackageVersionLocaleTextCount";
import PackageVersionRow from "./PackageVersionRow";

interface IPackageTableProps {
  module: IAppModule;
  packageId: number;
  locale: string;
};

const PackageTable: React.FC<IPackageTableProps> = props => {
  const [packageVersions, setPackageVersions] = React.useState<IPackageVersion[]>([]);
  const [textStats, setTextStats] = React.useState<IPackageVersionLocaleTextCount[]>([]);

  React.useEffect(() => {
    props.module.service.getPackVersions(props.packageId, (data) => {
      setPackageVersions(data);
    });
  }, [props.packageId, props.locale]);

  React.useEffect(() => {
    props.module.service.getTextStats(
      props.packageId,
      props.locale,
      (data) => {
        setTextStats(data);
      }
    );
  }, [packageVersions]);

  const rows = packageVersions.map((pv) => (
    <PackageVersionRow
      key={pv.PackageVersionId}
      contextLocale={props.module.locale}
      resources={props.module.resources}
      baseServicepath={props.module.service.baseServicepath}
      locale={props.locale}
      packageVersion={pv}
      textStat={textStats.filter(
        (t) => t.PackageVersionId === pv.PackageVersionId
      )}
    />
  ));

  return (
    <div>
      <table className="table">
        <thead>
          <tr>
            <th>{props.module.resources.Version}</th>
            <th>
              {props.module.resources.Stats} {props.locale}
            </th>
            <th>{props.module.resources.Released}</th>
            <th></th>
          </tr>
        </thead>
        <tbody>{rows}</tbody>
      </table>
    </div>
  );
}

export default PackageTable;