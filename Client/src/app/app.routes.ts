import { Routes, CanActivate, Router } from '@angular/router';
import { Injectable } from '@angular/core'

import { LoginComponent } from "./login.component"
import { MessageComponent } from './message.component'
import { RegisterComponent } from './register.component';
import { SocketService } from './socket.service'


export const routes: Routes = [
    { path: '', component: LoginComponent },
    { path: 'chat', component: MessageComponent},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent }
]
