import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { UniversalModule } from "angular2-universal";
import { FormsModule } from "@angular/forms"; 
// fwk components.
import { CheckListComponent } from "../fwk/controls/checklist";
import { PageSelector } from "../fwk/controls/pageSelector";
// app components.
import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./navmenu/navmenu.component";
import { HomeComponent } from "./home/home.component";
// user.
import { UserBrowseComponent } from "./user/user.browse.component";
import { UserEditComponent } from "./user/user.edit.component";

@NgModule({
    bootstrap: [AppComponent],
    declarations: [
        CheckListComponent,
        PageSelector,
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        UserBrowseComponent,
        UserEditComponent
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        RouterModule.forRoot([
            { path: "", redirectTo: "home", pathMatch: "full" },
            { path: "home", component: HomeComponent },
            {
                path: "user",
                children: [
                    { path: "", redirectTo: "list", pathMatch: "full" },
                    { path: "list", component: UserBrowseComponent },
                    { path: "edit/:id", component: UserEditComponent },
                    { path: "new", component: UserEditComponent }
                ]
            },
            { path: "**", redirectTo: "home" }
        ])
    ]
})
export class AppModule {
}