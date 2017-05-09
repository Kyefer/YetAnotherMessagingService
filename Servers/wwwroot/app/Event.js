"use strict";
var Event = (function () {
    function Event(type, data) {
        this.type = type;
        this.data = data;
    }
    return Event;
}());
exports.Event = Event;
var Authentication = (function () {
    function Authentication(username, password) {
        this.username = username;
        this.password = password;
    }
    return Authentication;
}());
exports.Authentication = Authentication;
var RegisterResult = (function () {
    function RegisterResult(success, reason) {
        this.success = success;
        this.reason = reason;
    }
    return RegisterResult;
}());
exports.RegisterResult = RegisterResult;
//# sourceMappingURL=Event.js.map