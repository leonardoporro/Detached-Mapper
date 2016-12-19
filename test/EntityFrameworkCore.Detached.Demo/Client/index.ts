import 'reflect-metadata';
import "zone.js";

import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { enableProdMode } from '@angular/core';
import { AppModule } from './app/app.module';


const platform = platformBrowserDynamic();

if (module['hot']) {
    module['hot'].accept();
    module['hot'].dispose(() => { platform.destroy(); });
} else {
    enableProdMode();
}

platform.bootstrapModule(AppModule);

require("./index.scss");

