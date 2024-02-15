import * as React from 'react';
import { IAppModule } from '../../Models/IAppModule';
import MenuItem from "@material-ui/core/MenuItem";
import FormHelperText from "@material-ui/core/FormHelperText";
import FormControl from "@material-ui/core/FormControl";
import Select from "@material-ui/core/Select";
import PackageTable from './PackageTable';
import styles from "./styles.module.css";

interface IPacksPageProps {
  module: IAppModule;
  packages: { Key: string; Value: string }[];
  locales: { Key: string; Value: string }[];
  genlocales: { Key: string; Value: string }[];
};

const PacksPage: React.FC<IPacksPageProps> = props => {
  const [selectedPackage, setSelectedPackage] = React.useState<number>(-1);
  const [selectedGenLocale, setSelectedGenLocale] = React.useState<string>("none");
  const [selectedLocale, setSelectedLocale] = React.useState<string>("");
  const [selectablePackages, setSelectablePackages] = React.useState<{ Key: string; Value: string }[]>([]);

  React.useEffect(() => {
    if (selectedGenLocale === "none") {
      setSelectedLocale("");
    } else {
      setSelectedLocale(findBestLocale(selectedGenLocale));
    }
  }, [selectedGenLocale]);

  React.useEffect(() => {
    if (selectedLocale === "") {
      setSelectablePackages([]);
    } else {
      props.module.service.getAvailableComponents(selectedLocale, (data: number[]) => {
        setSelectablePackages(
          props.packages.filter((p) => data.includes(parseInt(p.Key)))
        );
      });
    }
  }, [selectedLocale]);

  React.useEffect(() => {
    if (selectablePackages.length === 0) {
      setSelectedPackage(-1);
    } else if (selectablePackages.filter((p) => p.Value === "DNN Core").length > 0) {
      setSelectedPackage(parseInt(selectablePackages.filter((p) => p.Value === "DNN Core")[0].Key));
    } else {
      setSelectedPackage(parseInt(selectablePackages[0].Key));
    }
  }, [selectablePackages]);

  const findBestLocale = (genLocale: string): string => {
    var test = `${genLocale}-${genLocale.toUpperCase()}`;
    var res = props.locales.filter((l) => l.Key === test);
    if (res.length > 0) {
      return res[0].Key;
    }
    return props.locales.filter((l) =>
      l.Key.startsWith(props.genlocales[0].Key)
    )[0].Key;
  }

  var table = selectedPackage === -1 ?
    <div></div> :
    <PackageTable
      module={props.module}
      packageId={selectedPackage}
      locale={selectedLocale}
    />;

  return (
    <div>
      <div className={styles.page}>
        <div className={styles.dddiv}>
          <FormControl style={{ minWidth: "200px" }}>
            <Select
              value={selectedGenLocale}
              onChange={(e) =>
                setSelectedGenLocale(e.target.value as string)
              }
              style={{ paddingRight: "32px" }}
            >
              <MenuItem value="none" key="none">
                {props.module.resources.Select}
              </MenuItem>
              {props.genlocales.map((l) => (
                <MenuItem value={l.Key} key={l.Key}>
                  {l.Value}
                </MenuItem>
              ))}
            </Select>
            <FormHelperText>
              {props.module.resources.Language}
            </FormHelperText>
          </FormControl>
        </div>
        <div className={styles.dddiv}>
          <FormControl style={{ minWidth: "200px" }}>
            <Select
              value={selectedLocale}
              onChange={(e) =>
                setSelectedLocale(e.target.value as string)
              }
              style={{ paddingRight: "32px" }}
            >
              {props.locales
                .filter((l) => l.Key.startsWith(selectedGenLocale))
                .map((l) => (
                  <MenuItem value={l.Key} key={l.Key}>
                    {l.Value}
                  </MenuItem>
                ))}
            </Select>
            <FormHelperText>
              {props.module.resources.Region}
            </FormHelperText>
          </FormControl>
        </div>
        <div className={styles.dddiv}>
          <FormControl style={{ minWidth: "200px" }}>
            <Select
              value={selectedPackage}
              onChange={(e) =>
                setSelectedPackage(parseInt(e.target.value as string))
              }
              style={{ paddingRight: "32px" }}
            >
              {selectablePackages.map((p) => (
                <MenuItem value={p.Key} key={p.Key}>
                  {p.Value}
                </MenuItem>
              ))}
            </Select>
            <FormHelperText>
              {props.module.resources.Package}
            </FormHelperText>
          </FormControl>
        </div>
      </div>
      {table}
    </div>
  );
}

export default PacksPage;