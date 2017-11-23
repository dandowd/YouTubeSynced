import { Injectable, Inject, EventEmitter, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';

import { WATCH_CONFIG, IWatchConfig } from './watch.config';

@Injectable()
export class WatchService {
    private _hubConnection: HubConnection;
    public async: any;
    public roomName: string;
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


        this._hubConnection.on('SetUsersOnline', (data: any) => {
            const user = `${data} logged on`;
            console.log('set users');
            this.userEmitter.emit(data);
        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started')
                this.isLoading = false;
                this.loadingEmitter.emit(this.isLoading);
                this.joinRoom(this.roomName);
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }

    public joinRoom(roomName: string): void {
        this._hubConnection.invoke('AddGroupAsync', roomName);
    }

    public sendMessage(message: string): void {
        const data = `You: ${message}`;

        this._hubConnection.invoke('Send', data, this.roomName);
    }

    ngOnInit() {
    }
}