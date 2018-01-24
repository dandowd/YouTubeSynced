import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class YouTubeService {
    public player: any;
    private height = '390';
    private width = '640';
    private videoId = '';
    private divId = 'player';
    private state: number;

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
                            console.log('onReady');
                            this.pauseVideo();
                        },
                        'onStateChange': (ev: any) => {
                            this.onStateChange(ev);
                        }
                    }
                });
                clearInterval(interval);
                console.log('YT ready status ' + (<any>window).YT.Player.ready);
            }
        }, 100);
    }

    onStateChange(ev: any) {
        switch (ev.data) {
            case this.UNSTARTED: {
                console.log('unstarted');
                this.pauseVideo();
                this.state = this.UNSTARTED;
                break;
            }
            case this.ENDED: {
                console.log('ended');
                this.state = this.ENDED;
                break;
            }
            case this.PLAYING: {
                console.log('playing');
                this.state = this.PLAYING;
                break;
            }
            case this.PAUSED: {
                console.log('paused');
                this.state = this.PAUSED;
                break;
            }
            case this.BUFFERING: {
                this.state = this.BUFFERING;
                break;
            }
            case this.VIDEO_CUED: {
                console.log('video cued');
                this.state = this.VIDEO_CUED;
                break;
            }
                
        }
    }

    isPlaying(): boolean {
        if (this.state == this.PLAYING) {
            return true;
        }
        else {
            return false;
        }
    }

    changePlaying(isPlaying: boolean) {
        console.log("playing status" + isPlaying);
        if (isPlaying) {
            this.pauseVideo();
        }
        else {
            this.playVideo();
        }
    }

    changeVideo(id: string) {
        if (id !== this.videoId) {
            this.videoId = id;
            this.player.loadVideoById(id);
        }
    }

    playVideo() {
        this.player.playVideo();
    }

    pauseVideo() {
        this.player.pauseVideo();
    }
}