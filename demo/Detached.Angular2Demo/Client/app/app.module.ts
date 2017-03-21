// angular 2
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { HttpModule } from "@angular/http";
import { MaterialModule } from "@angular/material";
import { FlexLayoutModule } from "@angular/flex-layout";
import { LocaleModule, LocalizationModule } from "angular2localization";

// app
import { MdCoreModule } from "../core/md-core";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
// home
import { HomeComponent } from "./home/home.component";
// user
import { UserListComponent } from "./security/users/user-list.component";
import { UserEditComponent } from "./security/users/user-edit.component";

require("./app-styles.css");

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        FlexLayoutModule.forRoot(),
        MaterialModule.forRoot(),
        LocaleModule.forRoot(),
        LocalizationModule.forRoot(),
        MdCoreModule,
        AppRoutingModule,
    ],
    declarations: [
        AppComponent,
        HomeComponent,
        UserListComponent,
        UserEditComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }