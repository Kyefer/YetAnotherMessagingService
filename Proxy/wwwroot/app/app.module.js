"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var core_1 = require("@angular/core");
var platform_browser_1 = require("@angular/platform-browser");
var forms_1 = require("@angular/forms");
var router_1 = require("@angular/router");
var app_component_1 = require("./app.component");
var message_component_1 = require("./message.component");
var login_component_1 = require("./login.component");
var register_component_1 = require("./register.component");
var socket_service_1 = require("./socket.service");
var app_routes_1 = require("./app.routes");
var AppModule = (function () {
    function AppModule() {
    }
    return AppModule;
}());
AppModule = __decorate([
    core_1.NgModule({
        imports: [
            router_1.RouterModule.forRoot(app_routes_1.routes),
            platform_browser_1.BrowserModule,
            forms_1.FormsModule,
        ],
        declarations: [
            app_component_1.AppComponent,
            message_component_1.MessageComponent,
            login_component_1.LoginComponent,
            register_component_1.RegisterComponent,
            message_component_1.ScrollGlue
        ],
        bootstrap: [app_component_1.AppComponent],
        providers: [socket_service_1.SocketService]
    })
], AppModule);
exports.AppModule = AppModule;
//# sourceMappingURL=app.module.js.map