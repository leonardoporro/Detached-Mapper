import { Component } from "@angular/core";
import { Locale, LocaleService, LocalizationService } from "angular2localization";

@Component({
    selector: "app",
    template: require("./app.component.html")
})
export class AppComponent extends Locale {

    constructor(public locale: LocaleService, public localization: LocalizationService) {
        super(locale, localization);

        this.locale.addLanguages(["en", "es"]);
        this.locale.definePreferredLocale("en", "US", 30);
        this.locale.definePreferredCurrency("USD");
        this.localization.translationProvider("./lang/res_"); 
        this.localization.updateTranslation();
    }
}