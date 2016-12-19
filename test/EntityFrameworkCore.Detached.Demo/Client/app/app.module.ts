// angular 2 modules.
import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { MdlModule } from "angular2-mdl";

// app modules.
//import { MdlSharedModule } from "../shared/shared.module";
import { AppComponent } from "./app.component";
import { AppRoutingModule } from "./app-routing.module";
import { HomeComponent } from "./home/home.component";

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        MdlModule,
       // MdlSharedModule,
        AppRoutingModule,
    ],
    declarations: [
        AppComponent,
        HomeComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }