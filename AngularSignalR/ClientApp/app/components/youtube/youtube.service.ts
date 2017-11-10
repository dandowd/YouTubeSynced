import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class YouTubeService {
    public player: any;
    private height = '390';
    private width = '640';
    private videoId = '';
    private divId = 'player';
    private prevState: number;

    private readonly UNSTARTED = -1;
    private readonly ENDED = 0;
    private readonly PLAYING = 1;
    private readonly PAUSED = 2;
    private readonly BUFFERING = 4;
    private readonly VIDEO_CUED = 5;

    constructor() {
       
    }

    createPlayer() {
        let interval = setInterval(() => {
            if (typeof ((<any>window).YT !== 'undefined') && (<any>window).YT && (<any>window).YT.Player) {
                this.player = new (<any>window).YT.Player(this.divId, {
                    height: this.height,
                    width: this.width,
                    videoId: 'cyDOUPz0hpA',
                    playerVars: { 'autoplay': 0, 'rel': 0, 'controls': 0, 'start': 0 },
                    events: {
                        'onReady': () => {
                            this.pauseVideo();
                        },
                        'onStateChange': (ev: any) => {
                            this.onStateChange(ev);
                        }
                    }
                });
                clearInterval(interval);
                console.log('test ' + (<any>window).YT.Player.ready);
            }
        }, 100);
    }

    onStateChange(ev: any) {
        switch (ev.data) {
            case this.UNSTARTED: {
                console.log('unstarted');

                this.prevState = this.UNSTARTED;
                break;
            }
            case this.ENDED: {
                console.log('ended');

                this.prevState = this.ENDED;
                break;
            }
            case this.PLAYING: {
                console.log('playing');
                if (this.prevState == this.UNSTARTED || this.BUFFERING) {
                    this.pauseVideo();
                }
                this.prevState = this.PLAYING;
                break;
            }
            case this.PAUSED: {
                console.log('paused');
                break;
            }
            case this.BUFFERING: {
                this.prevState = this.BUFFERING;
                console.log('prevState set to' + this.prevState);
                break;
            }
            case this.VIDEO_CUED: {
                console.log('video cued');
                break;
            }
                
        }
    }

    changeVideo(id: string) {
        this.player.loadVideoById(id);
    }

    playVideo() {
        this.player.playVideo();
    }

    pauseVideo() {
        this.player.pauseVideo();
    }
}