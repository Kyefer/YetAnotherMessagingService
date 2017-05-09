import { Router } from '@angular/router'
import { Component } from '@angular/core';
import { SocketService } from './socket.service'

import { RegisterResult, Authentication } from './Event'

@Component({
    selector: 'register',
    templateUrl: './register.component.html'
})
export class RegisterComponent {
    username: string;
    password: string;
    result: RegisterResult;

    constructor(private router: Router, private socket: SocketService) {
        this.socket.setRegisterListener((result)=>this.handleRegisterResult(result));
    }

    
    handleRegisterResult (result: RegisterResult){
        this.result = result;
        if (result.success){
            this.router.navigate(["chat"])
        }
    }

    register() {
        var auth = new Authentication(this.username, this.password);
        this.socket.register(auth);
    }
}