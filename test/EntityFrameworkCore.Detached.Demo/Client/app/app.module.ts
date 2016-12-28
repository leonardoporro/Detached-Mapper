// angular 2
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { HttpModule } from "@angular/http";
import { MaterialModule } from "@angular/material";
import { FlexLayoutModule } from "@angular/flex-layout"; 

// app
import { SharedModule } from "../shared/shared.module";
import { AppRoutingModule } from "./app-routing.module";
import { AppNavComponent } from "./app-nav.component";
import { AppComponent } from "./app.component";
// home
import { HomeComponent } from "./home/home.component";
// user
import { UserListComponent } from "./security/user-list.component";
import { UserEditComponent } from "./security/user-edit.component";

require("./material.scss");
require("./styles.scss");

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        HttpModule,
        FlexLayoutModule.forRoot(),
        MaterialModule.forRoot(),
        SharedModule, 
        AppRoutingModule,
    ],
    declarations: [
        AppComponent,
        AppNavComponent,
        HomeComponent,
        UserListComponent,
        UserEditComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }