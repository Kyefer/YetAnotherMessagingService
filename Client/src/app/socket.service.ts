import { Injectable, EventEmitter } from '@angular/core'
import { Router } from '@angular/router'

import { Message } from "./Message"
import { Event, Authentication, RegisterResult } from "./Event"

@Injectable()
export class SocketService {
    private socket: WebSocket;
    private newMessageCallback: (message: Message) => void;
    private authCallback: (valid: boolean) => void;
    private registerCallback: (result: RegisterResult) => void;
    private allMessagesCallback: (messages: Message[]) => void;

    public constructor(private router: Router) {
        this.setUpSocket();
    }

    private setUpSocket() {
        this.socket = new WebSocket("ws://localhost:5000/ws")
        this.socket.onmessage = event => {
            let eventObj = JSON.parse(event.data);
            var type = eventObj.type.toLowerCase()


            if (type === "message_relay") {
                this.newMessageCallback(eventObj.data);
            } else if (type === "authentication_result") {
                this.authCallback(eventObj.data);
            } else if (type === "register_result") {
                this.registerCallback(eventObj.data);
            } else if (type === "message_return"){
                this.allMessagesCallback(eventObj.data);
            }

        }
        this.socket.onclose = event => {
            this.router.navigate(['login'])
        }
    }

    public sendMessage(message: Message) {
        var event = new Event("new_message", message);
        this.send(event);
    }

    public checkAuth(auth: Authentication) {
        var event = new Event("authentication", auth)
        this.send(event);
    }

    public register(auth: Authentication) {
        var event = new Event("register", auth)
        this.send(event)
    }

    public getAllMessages(){
        var event = new Event("message_fetch", null)
        this.send(event);
    }

    private send(object: any) {
        if (this.socket.readyState == WebSocket.OPEN) {
            this.socket.send(JSON.stringify(object));
        }
    }

    public close() {
        this.socket.close();
    }

    public setAuthListener(authCallback: (valid: boolean) => void) {
        this.authCallback = authCallback;
    }

    public setRegisterListener(registerCallback: (result: RegisterResult) => void) {
        this.registerCallback = registerCallback;
    }

    public setNewMessageListener(newMessageCallback: (message: Message) => void) {
        this.newMessageCallback = newMessageCallback;
    }

    public setAllMessagesCallback(allMessagesCallback: (messages: Message[]) => void){
        this.allMessagesCallback = allMessagesCallback;
    }
}