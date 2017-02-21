import { Component, Input } from "@angular/core";
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

        this.adjustSideNav();
        window.onresize = () => this.adjustSideNav();
    }

    adjustSideNav() {
        if (window.innerWidth > 800) {
            this.mode = "side";
            this.open = true;
        }
        else {
            this.mode = "over";
            this.open = false;
        }
    }

    @Input()
    public mode: string = "over";
    @Input()
    public open: boolean = false; 
}