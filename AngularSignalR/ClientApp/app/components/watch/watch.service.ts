import { Injectable, Inject, EventEmitter, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';

import { WATCH_CONFIG, IWatchConfig } from './watch.config';

@Injectable()
export class WatchService {
    private _hubConnection: HubConnection;
    public async: any;
    public roomName: string;
    public userName: string;
    public isLoading = true;
    loadingEmitter = new EventEmitter<boolean>();
    recieveEmitter = new EventEmitter<any>();
    userEmitter = new EventEmitter<any>();

    constructor( @Inject(WATCH_CONFIG) private config: IWatchConfig) {
        this.startSignalR();    
    }

    private startSignalR() {
        this._hubConnection = new HubConnection(this.config.apiEndpoint);

        this._hubConnection.on('Send', (data: any) => {
            const received = `Received: ${data}`;

            this.recieveEmitter.emit(received);
        });

        this._hubConnection.on('SetUsersOnline', (username: any) => {
            const user = `${username}`;
            console.log('SetUsersOnline' + username);
            this.userEmitter.emit(username);
        });

        this._hubConnection.on('UsersJoined', (usersJoined: any) => {
            const users = `${usersJoined}`;
            console.log(users);
        });

        this._hubConnection.on('UsersLeft', (usersLeft: any) => {
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
                console.log('signing');
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }

    public signIn(userName: string, roomName: string) {
        this._hubConnection.invoke('SignInAsync', userName, roomName);
    }

    public sendMessage(message: string): void {
        const data = `You: ${message}`;

        this._hubConnection.invoke('Send', data, this.roomName);
    }

    ngOnInit() {
    }
}