"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var router_1 = require("@angular/router");
var core_1 = require("@angular/core");
var socket_service_1 = require("./socket.service");
var Event_1 = require("./Event");
var LoginComponent = (function () {
    function LoginComponent(router, socket) {
        var _this = this;
        this.router = router;
        this.socket = socket;
        this.socket.setAuthListener(function (valid) { return _this.handleAuthResult(valid); });
        this.valid = true;
    }
    LoginComponent.prototype.login = function () {
        var auth = new Event_1.Authentication(this.username, this.password);
        this.socket.checkAuth(auth);
    };
    LoginComponent.prototype.handleAuthResult = function (valid) {
        this.valid = valid;
        if (valid) {
            this.router.navigate(["chat"]);
        }
    };
    LoginComponent.prototype.register = function () {
        this.router.navigate(["register"]);
    };
    return LoginComponent;
}());
LoginComponent = __decorate([
    core_1.Component({
        selector: 'login',
        templateUrl: './login.component.html'
    }),
    __metadata("design:paramtypes", [router_1.Router, socket_service_1.SocketService])
], LoginComponent);
exports.LoginComponent = LoginComponent;
//# sourceMappingURL=login.component.js.map