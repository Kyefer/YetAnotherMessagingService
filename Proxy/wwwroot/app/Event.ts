
export class Event {
    type: string;
    data: any;

    public constructor(type: string, data: any) {
        this.type = type;
        this.data = data;
    }
}

export class Authentication {
    username: string;
    password: string;

    public constructor(username: string, password: string) {
        this.username = username;
        this.password = password;
    }
}

export class RegisterResult {
    success: boolean;
    reason: string;

    public constructor(success: boolean, reason: string) {
        this.success = success;
        this.reason = reason;
    }
}