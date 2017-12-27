import { Injectable, Inject, Output, EventEmitter, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';

import { WATCH_CONFIG, IWatchConfig } from './watch.config';

export interface UserDetails {
    connectionId: string;
    name: string;
}

export interface UserEvent {
    user: UserDetails;
    isJoining: boolean;
}

@Injectable()
export class WatchService {
    private _hubConnection: HubConnection;
    public async: any;
    public roomName: string;
    public userName: string;
    public isLoading = true;
    loadingEmitter = new EventEmitter<boolean>();
    recieveEmitter = new EventEmitter<any>();
    userEmitter: EventEmitter<UserEvent> = new EventEmitter();
    setEmitter: EventEmitter<UserDetails[]> = new EventEmitter();

    constructor( @Inject(WATCH_CONFIG) private config: IWatchConfig) {
        this.startSignalR();    
    }

    private startSignalR() {
        this._hubConnection = new HubConnection(this.config.apiEndpoint);

        this._hubConnection.on('Send', (data: any) => {
            const received = `Received: ${data}`;

            this.recieveEmitter.emit(received);
        });

        this._hubConnection.on('SetUsersOnline', (usersSet: UserDetails[]) => {
            this.setEmitter.emit(usersSet);
        });

        this._hubConnection.on('UsersJoined', (usersJoined: UserDetails) => {
            const joiningEvent: UserEvent = { user: usersJoined, isJoining: true };
            this.userEmitter.emit(joiningEvent);
        });

        this._hubConnection.on('UsersLeft', (usersLeft: UserDetails) => {
            const users = `${usersLeft}`;
            console.log('Users Left: ' + users);
        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started')
                this.isLoading = false;
                this.loadingEmitter.emit(this.isLoading);
                console.log('username: ' + this.userName);
                this.signIn(this.userName, this.roomName);
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }

    public signIn(userName: string, groupName: string) {
        this._hubConnection.invoke('SignInAsync', userName, groupName);
    }

    public joinRoom(groupName: string) {
        this._hubConnection.invoke('JoinRoom', groupName);
    }

    public leaveRoom(userName: string) {
        this._hubConnection.invoke('LeaveRoom', userName);
    }

    public sendMessage(message: string): void {
        const data = `You: ${message}`;

        this._hubConnection.invoke('Send', data, this.roomName);
    }

    ngOnInit() {
    }
}