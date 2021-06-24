import * as React from "react";
import { IAppModule } from "../../Models/IAppModule";
import { IPackageVersion } from "../../Models/IPackageVersion";
import { IPackageVersionLocaleTextCount } from "../../Models/IPackageVersionLocaleTextCount";

interface IPacksPageProps {
  module: IAppModule;
  packages: { Key: string; Value: string }[];
  locales: { Key: string; Value: string }[];
  genlocales: { Key: string; Value: string }[];
}

interface IPacksPageState {
  selectedPackage: number;
  selectedGenLocale: string;
  selectedLocale: string;
  packageVersions: IPackageVersion[];
  textStats: IPackageVersionLocaleTextCount[];
}

export default class PacksPage extends React.Component<
  IPacksPageProps,
  IPacksPageState
> {
  constructor(props: IPacksPageProps) {
    super(props);
    this.state = {
      selectedGenLocale:
        props.genlocales.length === 0 ? "" : props.genlocales[0].Key,
      selectedLocale:
        props.locales.length === 0
          ? ""
          : this.findBestLocale(props.genlocales[0].Key),
      selectedPackage:
        props.packages.length === 0 ? -1 : parseInt(props.packages[0].Key),
      packageVersions: [],
      textStats: [],
    };
    this.packageChanged();
  }

  private findBestLocale(genLocale: string): string {
    var test = `${genLocale}-${genLocale.toUpperCase()}`;
    var res = this.props.locales.filter((l) => l.Key === test);
    if (res.length > 0) {
      return res[0].Key;
    }
    return this.props.locales.filter((l) =>
      l.Key.startsWith(this.props.genlocales[0].Key)
    )[0].Key;
  }

  private packageChanged(): void {
    this.props.module.service.getPackVersions(
      this.state.selectedPackage,
      (data) => {
        this.setState(
          {
            packageVersions: data,
          },
          () => {
            this.localeChanged();
          }
        );
      }
    );
  }

  private localeChanged(): void {
    this.props.module.service.getTextStats(
      this.state.selectedPackage,
      this.state.selectedLocale,
      (data) => {
        this.setState({
          textStats: data,
        });
      }
    );
  }

  public render(): JSX.Element {
    return (
      <div>
        <div>
          <select
            value={this.state.selectedGenLocale}
            onChange={(e) =>
              this.setState(
                {
                  selectedGenLocale: e.target.value,
                  selectedLocale: this.findBestLocale(e.target.value),
                },
                () => this.localeChanged()
              )
            }
          >
            {this.props.genlocales.map((l) => (
              <option value={l.Key} key={l.Key}>
                {l.Value}
              </option>
            ))}
          </select>
        </div>
        <div>
          <select
            value={this.state.selectedLocale}
            onChange={(e) =>
              this.setState({ selectedLocale: e.target.value }, () =>
                this.localeChanged()
              )
            }
          >
            {this.props.locales
              .filter((l) => l.Key.startsWith(this.state.selectedGenLocale))
              .map((l) => (
                <option value={l.Key} key={l.Key}>
                  {l.Value}
                </option>
              ))}
          </select>
        </div>
        <div>
          <select
            value={this.state.selectedPackage}
            onChange={(e) =>
              this.setState({ selectedPackage: parseInt(e.target.value) }, () =>
                this.packageChanged()
              )
            }
          >
            {this.props.packages.map((p) => (
              <option value={p.Key} key={p.Key}>
                {p.Value}
              </option>
            ))}
          </select>
        </div>
      </div>
    );
  }
}
