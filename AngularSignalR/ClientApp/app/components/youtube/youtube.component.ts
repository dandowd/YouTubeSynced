import { Component, AfterViewInit, OnDestroy, Injectable, EventEmitter } from '@angular/core'

import { WatchService } from '../watch/watch.service';
import { YouTubeService } from './youtube.service';

@Component({
    selector: 'app-youtube-player',
    templateUrl: './youtube.component.html'
})

export class YouTubeComponent implements AfterViewInit, OnDestroy {

    constructor(private youTubeService: YouTubeService, private watchService: WatchService) {

    }

    ChangeReady() {
        this.watchService.UserReadyToggle();
    }

    playVideo() {
        this.youTubeService.playVideo();
        this.watchService.playToggle(this.youTubeService.isPlaying());
    }

    pauseVideo() {
        this.youTubeService.pauseVideo();
        this.watchService.playToggle(this.youTubeService.isPlaying());
    }

    changeVideo(id: string) {
        this.youTubeService.changeVideo(id);
        this.watchService.changeVideo(id);
    }

    UserReady() {
        this.watchService.UserReadyToggle();
    }

    ngAfterViewInit() {
        const doc = (<any>window).document;
        let playerApiScript = doc.createElement('script');
        playerApiScript.id = 'youtubescript';
        playerApiScript.type = 'text/javascript';
        playerApiScript.src = 'https://www.youtube.com/iframe_api';
        doc.body.appendChild(playerApiScript);

        this.youTubeService.createPlayer();
    }

    ngOnDestroy() {
        var elm = (<any>window).document.getElementById('youtubescript');
        elm.parentNode.removeChild(elm);
    }
}