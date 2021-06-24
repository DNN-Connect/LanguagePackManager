import * as React from "react";
import * as ReactDOM from "react-dom";
import * as $ from "jquery";

import { AppManager } from "./AppManager";
import PacksPage from "./Components/Packs/PacksPage";

export class ComponentLoader {
  public static load(): void {
    $(".packs").each(function (i, el) {
      var moduleId = $(el).data("moduleid");
      ReactDOM.render(
        <PacksPage
          module={AppManager.Modules.Item(moduleId.toString())}
          packages={$(el).data("packages")}
          locales={$(el).data("locales")}
          genlocales={$(el).data("genlocales")}
        />,
        el
      );
    });
  }
}
