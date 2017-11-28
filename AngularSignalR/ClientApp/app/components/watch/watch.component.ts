import { Component, Input } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { HubConnection } from '@aspnet/signalr-client';
import { YouTubeComponent } from '../youtube/youtube.component';
import { ChatComponent } from '../chat/chat.component';
import { UserListComponent } from '../user-list/user-list.component';

import { WatchService } from './watch.service';

@Component({
    selector: 'app-watch-component',
    templateUrl: './watch.component.html'
})

export class WatchComponent {
    private roomName: string;
    private userName: string;
    private isLoading = true;

    constructor(private route: ActivatedRoute, private watchService: WatchService) {

        this.route.params.subscribe(params => {
            this.roomName = params['roomname'];
            this.userName = params['username'];
        });

        this.watchService.roomName = this.roomName;
        this.watchService.userName = this.userName;

        this.watchService.loadingEmitter.subscribe((data: boolean) => {
            this.isLoading = data;
        });
    }
}