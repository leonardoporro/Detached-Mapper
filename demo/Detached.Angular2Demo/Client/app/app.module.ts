// angular 2
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { HttpModule } from "@angular/http";
import { MaterialModule } from "@angular/material";
import { FlexLayoutModule } from "@angular/flex-layout"; 
import { LocaleModule, LocalizationModule } from "angular2localization";

// app
import { CoreModule } from "../core/core.module";
import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
// home
import { HomeComponent } from "./home/home.component";
// user
import { UserListComponent } from "./security/users/user-list.component";
import { UserEditComponent } from "./security/users/user-edit.component";

require("./app-styles.scss");

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        FlexLayoutModule.forRoot(),
        MaterialModule.forRoot(),
        LocaleModule.forRoot(),
        LocalizationModule.forRoot(),
        CoreModule, 
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