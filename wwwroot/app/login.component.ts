import { Router } from '@angular/router'
import { Component } from '@angular/core';
import { SocketService } from './socket.service'

import { Authentication } from './Event'

@Component({
    selector: 'login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    username: string;
    password: string;
    valid: boolean;

    constructor(private router: Router, private socket: SocketService) {
        this.socket.setAuthListener((valid) => this.handleAuthResult(valid));
        this.valid = true;
    }

    login() {
        var auth = new Authentication(this.username, this.password);
        this.socket.checkAuth(auth);
    }

    handleAuthResult(valid: boolean) {
        this.valid = valid;
        if (valid) {
            this.router.navigate(["chat"]);
        }
    }

    register() {
        this.router.navigate(["register"])
    }
}