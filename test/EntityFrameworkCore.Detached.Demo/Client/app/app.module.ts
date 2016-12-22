// angular 2 modules.
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { HttpModule } from "@angular/http";
import { MdlModule } from "angular2-mdl";

// app modules.
import { SharedModule } from "../shared/shared.module";
import { AppComponent } from "./app.component";
import { AppRoutingModule } from "./app-routing.module";

import { HomeComponent } from "./home/home.component";

import { UserListComponent } from "./security/user-list.component";
import { UserEditComponent } from "./security/user-edit.component";

require("./styles.scss");

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        MdlModule,
        SharedModule, 
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