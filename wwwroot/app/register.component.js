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
var RegisterComponent = (function () {
    function RegisterComponent(router, socket) {
        var _this = this;
        this.router = router;
        this.socket = socket;
        this.socket.setRegisterListener(function (result) { return _this.handleRegisterResult(result); });
    }
    RegisterComponent.prototype.handleRegisterResult = function (result) {
        this.result = result;
        if (result.success) {
            this.router.navigate(["chat"]);
        }
    };
    RegisterComponent.prototype.register = function () {
        var auth = new Event_1.Authentication(this.username, this.password);
        this.socket.register(auth);
    };
    return RegisterComponent;
}());
RegisterComponent = __decorate([
    core_1.Component({
        selector: 'register',
        templateUrl: './register.component.html'
    }),
    __metadata("design:paramtypes", [router_1.Router, socket_service_1.SocketService])
], RegisterComponent);
exports.RegisterComponent = RegisterComponent;
//# sourceMappingURL=register.component.js.map