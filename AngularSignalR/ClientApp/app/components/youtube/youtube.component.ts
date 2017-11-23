import { Component, AfterViewInit, OnDestroy, Injectable } from '@angular/core'

import { YouTubeService } from './youtube.service';

@Component({
    selector: 'app-youtube-player',
    templateUrl: './youtube.component.html'
})

export class YouTubeComponent implements AfterViewInit, OnDestroy {

    constructor(private youTubeService: YouTubeService) {
    }

    playVideo() {
        this.youTubeService.playVideo();
    }

    changeVideo(id: string) {
        this.youTubeService.changeVideo(id);
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