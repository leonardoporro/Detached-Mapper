"use strict";
require("zone.js");
require("reflect-metadata");
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
var core_1 = require('@angular/core');
var app_module_1 = require('./app/app.module');
var platform = platform_browser_dynamic_1.platformBrowserDynamic();
if (module['hot']) {
    module['hot'].accept();
    module['hot'].dispose(function () { platform.destroy(); });
}
else {
    core_1.enableProdMode();
}
platform.bootstrapModule(app_module_1.AppModule);
//# sourceMappingURL=main.js.map