"use strict";
var login_component_1 = require("./login.component");
var message_component_1 = require("./message.component");
var register_component_1 = require("./register.component");
exports.routes = [
    { path: '', component: login_component_1.LoginComponent },
    { path: 'chat', component: message_component_1.MessageComponent },
    { path: 'login', component: login_component_1.LoginComponent },
    { path: 'register', component: register_component_1.RegisterComponent }
];
//# sourceMappingURL=app.routes.js.map