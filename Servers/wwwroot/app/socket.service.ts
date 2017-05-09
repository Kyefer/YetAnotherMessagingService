import { Injectable, EventEmitter } from '@angular/core'
import { Http } from '@angular/http';


import { Message } from "./Message"

@Injectable()
export class SocketService {
    private socket: WebSocket;
    private newMessageCallback: (message: Message) => void;
    private authCallback: (valid: boolean) => void;
    private allMessagesCallback: (messages: Message[]) => void;

    public constructor(private http: Http) {
        this.http.get("slave").subscribe((r) => {
            this.setUpSocket(+r.text());
        });
    }

    private setUpSocket(port: Number) {
        this.socket = new WebSocket("ws://localhost:" + port + "/ws")
        this.socket.onmessage = event => {
            let message = JSON.parse(event.data);
                this.newMessageCallback(message);

        }
    }

  
    public send(object: any) {
        if (this.socket.readyState == WebSocket.OPEN) {
            this.socket.send(JSON.stringify(object));
        }
    }

    public close() {
        this.socket.close();
    }

    public setNewMessageListener(newMessageCallback: (message: Message) => void) {
        this.newMessageCallback = newMessageCallback;
    }
}