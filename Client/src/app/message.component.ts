import { Component, ElementRef, OnInit, OnDestroy, Directive } from '@angular/core';
import { SocketService } from './socket.service'
import { Message } from "./Message"

@Component({
  selector: 'root',
  templateUrl: './message.component.html',
})
export class MessageComponent implements OnInit, OnDestroy {

  public messages: Array<Message>
  public msgInput: string;
  public username: string;

  public constructor(private socket: SocketService) {
    this.messages = []
  }

  public ngOnInit() {
    
    this.socket.setNewMessageListener((message: Message) => {
      if (new RegExp(".*@" + this.username + + "(\\s|$)").test(this.username)){
        message.tagged = true;
        var audio = new Audio("notification.wav")
        audio.load();
        audio.play();
      }
      this.messages.push(message);
    });
  }

  public ngOnDestroy() {
    this.socket.close();
  }

  public send() {
    if (this.msgInput && this.username) {
      var msg = new Message(this.username, this.msgInput);
      this.socket.send(msg);
      this.msgInput = "";
    }
  }
}


@Directive({
  selector: '[scroll-glue]',
  host: {
    '(scroll)': 'onScroll()'
  }
})
export class ScrollGlue {
  public el: any;
  public isLocked: boolean = false;
  private _observer: any;
  private _oldScrollHeight: number = 0;

  constructor(private _el: ElementRef) {
    this.el = _el.nativeElement;
  }

  onScroll() {
    // let percent = (this.el.scrollHeight / 100);
    // if (this.el.scrollHeight - this.el.scrollTop > (10 * percent)) {
    //   this.isLocked = true;
    // } else {
    //   this.isLocked = false;
    // }
    // console.log(this.el.scrollHeight)
    // console.log(this.el.scrollTop)
    // console.log(this.el.scroll
  }

  ngAfterContentInit() {
    this.el.scrollTop = this.el.scrollHeight;

    // create an observer instance
    this._observer = new MutationObserver((mutations) => {
      if (!this.isLocked) {
        this._oldScrollHeight = this.el.scrollHeight;
        this.el.scrollTop = this.el.scrollHeight;
      }
    });

    // configuration of the observer:
    var config = { childList: true, subtree: true };
    var target = this.el;

    // pass in the target node, as well as the observer options
    this._observer.observe(target, config);
  }

  ngOnDestroy() {
    this._observer.disconnect();
  }
}
