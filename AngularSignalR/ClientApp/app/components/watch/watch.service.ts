﻿import { Injectable, Inject, Output, EventEmitter, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';
import { YouTubeService } from '../youtube/youtube.service';

import { WATCH_CONFIG, IWatchConfig } from './watch.config';

export interface UserDetails {
    connectionId: string;
    name: string;
    readyStatus: boolean;
}

export interface UserEvent {
    user: UserDetails;
    isJoining: boolean;
}

export interface ReadyEvent {
    user: string;
    readyChange: boolean;
}

export interface Message {
    user: string;
    message: string;
}

@Injectable()
export class WatchService {
    private _hubConnection: HubConnection;
    public async: any;
    public roomName: string;
    public userName: string;
    public guid: string;
    private isUserReady = false;
    loadingEmitter = new EventEmitter<boolean>();
    recieveEmitter = new EventEmitter<any>();
    userEmitter: EventEmitter<UserEvent> = new EventEmitter();
    setEmitter: EventEmitter<UserDetails[]> = new EventEmitter();
    readyStatusEmitter: EventEmitter<ReadyEvent> = new EventEmitter();

    constructor( @Inject(WATCH_CONFIG) private config: IWatchConfig, private youtubeService: YouTubeService) {
        this.startSignalR();    
    }

    private startSignalR() {
        this._hubConnection = new HubConnection(this.config.apiEndpoint);

        this._hubConnection.on('Send', (data: Message) => {
            const received = `${data}`;
            console.log(data);
            this.recieveEmitter.emit(received);
        });

        this._hubConnection.on('SetUsersOnline', (usersSet: UserDetails[]) => {
            this.setEmitter.emit(usersSet);
            this.loadingEmitter.emit(false);
        });

        this._hubConnection.on('UsersJoined', (usersJoined: UserDetails) => {
            const joiningEvent: UserEvent = { user: usersJoined, isJoining: true };
            this.userEmitter.emit(joiningEvent);
        });

        this._hubConnection.on('UsersLeft', (usersLeft: UserDetails) => {
            const joiningEvent: UserEvent = { user: usersLeft, isJoining: false };
            this.userEmitter.emit(joiningEvent);
        });

        this._hubConnection.on('UserReady', (userName: string, readyStatus: boolean) => {
            const userStatus: ReadyEvent = { user: userName, readyChange: readyStatus }
            console.log(userStatus);
            this.readyStatusEmitter.emit(userStatus);
        });

        this._hubConnection.on('ChangeVideo', (videoURL: string) => {
            this.youtubeService.changeVideo(videoURL);
        });

        this._hubConnection.on('PlayToggle', (isPlaying: boolean) => {
            this.youtubeService.changePlaying(isPlaying);
        });

        this._hubConnection.on('SignInVerification', (signinSuccess: boolean) => {

        });

        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
                this.signIn(this.userName, this.roomName);
            })
            .catch(err => {
                console.log('Error while establishing connection')
            });
    }

    public signIn(userName: string, groupName: string) {
        this._hubConnection.invoke('SignInAsync', this.guid);
    }

    public joinRoom(groupName: string) {
        this._hubConnection.invoke('JoinRoom', groupName);
    }

    public leaveRoom(userName: string) {
        this._hubConnection.invoke('LeaveRoom', userName);
    }

    public sendMessage(message: string): void {
        const data = `${message}`;

        this._hubConnection.invoke('Send', data, this.roomName);
    }

    public changeVideo(videoUrl: string) {
        this._hubConnection.invoke('ChangeVideo', this.roomName, videoUrl);
    }

    private userReady() {
        this._hubConnection.invoke('UserReady', this.userName, this.roomName, this.isUserReady);
    }

    public playToggle(isPlaying: boolean) {
        this._hubConnection.invoke('PlayToggle', this.roomName, isPlaying);
    }

    public UserReadyToggle() {
        if (this.isUserReady)
            this.isUserReady = false;
        else
            this.isUserReady = true;

        this.userReady();
    }

    ngOnInit() {
    }
}