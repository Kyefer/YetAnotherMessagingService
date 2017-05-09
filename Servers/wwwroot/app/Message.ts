export class Message {
    sender: string;
    content: string;
    tagged: boolean;

    
    public constructor(sender: string, content: string){
        this.sender = sender;
        this.content = content;
    }
}