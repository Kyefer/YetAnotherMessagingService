import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms'

import { MessageComponent, ScrollGlue } from './message.component';
import { HttpModule } from '@angular/http';

import { SocketService } from "./socket.service"


@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
  ],
  declarations: [
    MessageComponent,
    ScrollGlue
  ],

  bootstrap: [MessageComponent],
  providers: [SocketService]
})
export class AppModule { }

