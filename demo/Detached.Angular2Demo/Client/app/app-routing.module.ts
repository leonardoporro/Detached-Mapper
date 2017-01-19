import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./home/home.component";
import { UserListComponent } from "./security/users/user-list.component";
import { UserEditComponent } from "./security/users/user-edit.component";

const appRoutes: Routes = [
    { path: "", redirectTo: "home", pathMatch: "full" },
    { path: "home", component: HomeComponent },
    { path: "security/users",
        children: [
            { path: "", component: UserListComponent },
            { path: ":id", component: UserEditComponent },
            { path: "new", component: UserEditComponent }
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