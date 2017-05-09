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
var http_1 = require("@angular/http");
var SocketService = (function () {
    function SocketService(http) {
        var _this = this;
        this.http = http;
        this.http.get("slave").subscribe(function (r) {
            _this.setUpSocket(+r.text());
        });
    }
    SocketService.prototype.setUpSocket = function (port) {
        var _this = this;
        this.socket = new WebSocket("ws://localhost:" + port + "/ws");
        this.socket.onmessage = function (event) {
            var message = JSON.parse(event.data);
            _this.newMessageCallback(message);
        };
    };
    SocketService.prototype.send = function (object) {
        if (this.socket.readyState == WebSocket.OPEN) {
            this.socket.send(JSON.stringify(object));
        }
    };
    SocketService.prototype.close = function () {
        this.socket.close();
    };
    SocketService.prototype.setNewMessageListener = function (newMessageCallback) {
        this.newMessageCallback = newMessageCallback;
    };
    return SocketService;
}());
SocketService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.Http])
], SocketService);
exports.SocketService = SocketService;
//# sourceMappingURL=socket.service.js.map