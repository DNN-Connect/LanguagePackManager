import * as $ from "jquery";
import { AppModule, IAppModule } from "./Models/IAppModule";
import { KeyedCollection } from "./Models/IKeyedCollection";
import DataService from "./Service";

export class AppManager {
  public static Modules = new KeyedCollection<IAppModule>();

  public static loadData(): void {
    $(".connectlpm").each(function(i, el) {
      var moduleId = $(el).data("moduleid");
      AppManager.Modules.Add(
        moduleId,
        new AppModule(
          moduleId,
          $(el).data("tabid"),
          $(el).data("locale"),
          $(el).data("resources"),
          $(el).data("common"),
          $(el).data("security"),
          new DataService(moduleId)
        )
      );
    });
  }
}
