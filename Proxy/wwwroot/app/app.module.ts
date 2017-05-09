import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'
import { RouterModule, Routes } from '@angular/router';

import { AppComponent } from './app.component'
import { MessageComponent, ScrollGlue } from './message.component';
import { LoginComponent } from './login.component';
import { RegisterComponent } from './register.component';

import { SocketService } from "./socket.service"

import { routes } from "./app.routes"

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    BrowserModule,
    FormsModule,
  ],
  declarations: [
    AppComponent,
    MessageComponent,
    LoginComponent,
    RegisterComponent,
    ScrollGlue
  ],

  bootstrap: [AppComponent],
  providers: [SocketService]
})
export class AppModule { }

