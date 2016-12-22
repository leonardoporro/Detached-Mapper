import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { HomeComponent } from "./home/home.component";

import { UserListComponent } from "./security/user-list.component";
import { UserEditComponent } from "./security/user-edit.component";


const appRoutes: Routes = [
    { path: "", redirectTo: "home", pathMatch: "full" },
    { path: "home", component: HomeComponent },
    {
        path: "security/users",
        children: [
            { path: "", pathMatch: "full", redirectTo: "list" },
            { path: "list", component: UserListComponent },
            { path: "edit/:id", component: UserEditComponent }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forRoot(appRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }