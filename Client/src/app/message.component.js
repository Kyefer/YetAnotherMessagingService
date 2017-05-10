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
var socket_service_1 = require("./socket.service");
var Message_1 = require("./Message");
var MessageComponent = (function () {
    function MessageComponent(socket) {
        this.socket = socket;
        this.messages = [];
    }
    MessageComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.socket.setNewMessageListener(function (message) {
            var tagged = new RegExp(".*@" + _this.username + "(\\s.*|$)");
            if (tagged.test(message.content)) {
                console.log("Tagged");
                message.tagged = true;
                var audio = new Audio("notification.wav");
                audio.load();
                audio.play();
            }
            _this.messages.push(message);
        });
    };
    MessageComponent.prototype.ngOnDestroy = function () {
        this.socket.close();
    };
    MessageComponent.prototype.send = function () {
        if (this.msgInput && this.username) {
            var msg = new Message_1.Message(this.username, this.msgInput);
            this.socket.send(msg);
            this.msgInput = "";
        }
    };
    return MessageComponent;
}());
MessageComponent = __decorate([
    core_1.Component({
        selector: 'root',
        templateUrl: './message.component.html',
    }),
    __metadata("design:paramtypes", [socket_service_1.SocketService])
], MessageComponent);
exports.MessageComponent = MessageComponent;
var ScrollGlue = (function () {
    function ScrollGlue(_el) {
        this._el = _el;
        this.isLocked = false;
        this._oldScrollHeight = 0;
        this.el = _el.nativeElement;
    }
    ScrollGlue.prototype.onScroll = function () {
        // let percent = (this.el.scrollHeight / 100);
        // if (this.el.scrollHeight - this.el.scrollTop > (10 * percent)) {
        //   this.isLocked = true;
        // } else {
        //   this.isLocked = false;
        // }
        // console.log(this.el.scrollHeight)
        // console.log(this.el.scrollTop)
        // console.log(this.el.scroll
    };
    ScrollGlue.prototype.ngAfterContentInit = function () {
        var _this = this;
        this.el.scrollTop = this.el.scrollHeight;
        // create an observer instance
        this._observer = new MutationObserver(function (mutations) {
            if (!_this.isLocked) {
                _this._oldScrollHeight = _this.el.scrollHeight;
                _this.el.scrollTop = _this.el.scrollHeight;
            }
        });
        // configuration of the observer:
        var config = { childList: true, subtree: true };
        var target = this.el;
        // pass in the target node, as well as the observer options
        this._observer.observe(target, config);
    };
    ScrollGlue.prototype.ngOnDestroy = function () {
        this._observer.disconnect();
    };
    return ScrollGlue;
}());
ScrollGlue = __decorate([
    core_1.Directive({
        selector: '[scroll-glue]',
        host: {
            '(scroll)': 'onScroll()'
        }
    }),
    __metadata("design:paramtypes", [core_1.ElementRef])
], ScrollGlue);
exports.ScrollGlue = ScrollGlue;
//# sourceMappingURL=message.component.js.map