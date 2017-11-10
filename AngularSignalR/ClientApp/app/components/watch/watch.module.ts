import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { WatchComponent } from './watch.component';
import { ChatComponent } from '../chat/chat.component';
import { YouTubeComponent } from '../youtube/youtube.component';
import { UserListComponent } from '../user-list/user-list.component';

import { WatchService } from './watch.service';
import { YouTubeService } from '../youtube/youtube.service'

import { WATCH_CONFIG, WatchConfig } from './watch.config';

@NgModule({
    imports: [
        FormsModule,
        CommonModule
    ],
    declarations: [
        WatchComponent,
        YouTubeComponent,
        ChatComponent,
        UserListComponent
    ],
    providers: [
        WatchService,
        YouTubeService,
        { provide: WATCH_CONFIG, useValue: WatchConfig }
    ]

})

export class WatchModule{ }