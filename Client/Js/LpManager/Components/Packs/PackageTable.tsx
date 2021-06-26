import * as React from "react";
import { IAppModule } from "../../Models/IAppModule";
import { IPackageVersion } from "../../Models/IPackageVersion";
import { IPackageVersionLocaleTextCount } from "../../Models/IPackageVersionLocaleTextCount";
import PackageVersionRow from "./PackageVersionRow";

interface IPackageTableProps {
  module: IAppModule;
  packageId: number;
  locale: string;
}

interface IPackageTableState {
  packageVersions: IPackageVersion[];
  textStats: IPackageVersionLocaleTextCount[];
}

export default class PackageTable extends React.Component<
  IPackageTableProps,
  IPackageTableState
> {
  constructor(props: IPackageTableProps) {
    super(props);
    this.state = {
      packageVersions: [],
      textStats: [],
    };
    this.refreshData();
  }

  private refreshData() {
    this.props.module.service.getPackVersions(this.props.packageId, (data) => {
      this.setState(
        {
          packageVersions: data,
        },
        () => {
          this.props.module.service.getTextStats(
            this.props.packageId,
            this.props.locale,
            (data) => {
              this.setState({
                textStats: data,
              });
            }
          );
        }
      );
    });
  }

  componentDidUpdate(
    prevProps: IPackageTableProps,
    prevState: IPackageTableState
  ) {
    if (
      prevProps.packageId != this.props.packageId ||
      prevProps.locale != this.props.locale
    ) {
      this.refreshData();
    }
  }

  public render(): JSX.Element {
    var rows = this.state.packageVersions.map((pv) => (
      <PackageVersionRow
        key={pv.PackageVersionId}
        contextLocale={this.props.module.locale}
        resources={this.props.module.resources}
        baseServicepath={this.props.module.service.baseServicepath}
        locale={this.props.locale}
        packageVersion={pv}
        textStat={this.state.textStats.filter(
          (t) => t.PackageVersionId === pv.PackageVersionId
        )}
      />
    ));
    return (
      <div>
        <table className="table">
          <thead>
            <tr>
              <th>{this.props.module.resources.Version}</th>
              <th>{this.props.module.resources.Stats}</th>
              <th>{this.props.module.resources.Released}</th>
              <th></th>
            </tr>
          </thead>
          <tbody>{rows}</tbody>
        </table>
      </div>
    );
  }
}
