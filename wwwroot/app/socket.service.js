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
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var Event_1 = require("./Event");
var SocketService = (function () {
    function SocketService(router) {
        this.router = router;
        this.setUpSocket();
    }
    SocketService.prototype.setUpSocket = function () {
        var _this = this;
        this.socket = new WebSocket("ws://localhost:5000/ws");
        this.socket.onmessage = function (event) {
            var eventObj = JSON.parse(event.data);
            var type = eventObj.type.toLowerCase();
            if (type === "message_relay") {
                _this.newMessageCallback(eventObj.data);
            }
            else if (type === "authentication_result") {
                _this.authCallback(eventObj.data);
            }
            else if (type === "register_result") {
                _this.registerCallback(eventObj.data);
            }
            else if (type === "message_return") {
                _this.allMessagesCallback(eventObj.data);
            }
        };
        this.socket.onclose = function (event) {
            _this.router.navigate(['login']);
        };
    };
    SocketService.prototype.sendMessage = function (message) {
        var event = new Event_1.Event("new_message", message);
        this.send(event);
    };
    SocketService.prototype.checkAuth = function (auth) {
        var event = new Event_1.Event("authentication", auth);
        this.send(event);
    };
    SocketService.prototype.register = function (auth) {
        var event = new Event_1.Event("register", auth);
        this.send(event);
    };
    SocketService.prototype.getAllMessages = function () {
        var event = new Event_1.Event("message_fetch", null);
        this.send(event);
    };
    SocketService.prototype.send = function (object) {
        if (this.socket.readyState == WebSocket.OPEN) {
            this.socket.send(JSON.stringify(object));
        }
    };
    SocketService.prototype.close = function () {
        this.socket.close();
    };
    SocketService.prototype.setAuthListener = function (authCallback) {
        this.authCallback = authCallback;
    };
    SocketService.prototype.setRegisterListener = function (registerCallback) {
        this.registerCallback = registerCallback;
    };
    SocketService.prototype.setNewMessageListener = function (newMessageCallback) {
        this.newMessageCallback = newMessageCallback;
    };
    SocketService.prototype.setAllMessagesCallback = function (allMessagesCallback) {
        this.allMessagesCallback = allMessagesCallback;
    };
    return SocketService;
}());
SocketService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [router_1.Router])
], SocketService);
exports.SocketService = SocketService;
//# sourceMappingURL=socket.service.js.map