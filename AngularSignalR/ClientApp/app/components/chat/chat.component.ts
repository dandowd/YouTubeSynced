import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HubConnection } from '@aspnet/signalr-client';

import { WatchService } from '../watch/watch.service';

@Component({
    selector: 'app-chat-component',
    templateUrl: './chat.component.html'
})

export class ChatComponent implements OnInit {
    @Input() roomName: string;
    public async: any;
    message = '';
    messages: string[] = [];

    constructor(private watchService: WatchService) {

    }

    public sendMessage() {
        this.watchService.sendMessage(this.message);
    }

    ngOnInit() {
        this.watchService.recieveEmitter.subscribe((data: any) => {
            console.log(data);
            this.messages.push(data);
        });
    }
}