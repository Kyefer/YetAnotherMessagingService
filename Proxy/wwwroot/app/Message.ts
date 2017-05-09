export class Message {
    sender: string;
    content: string;
    tagged: boolean;

    
    public constructor(content: string){
        this.content = content;
    }
}