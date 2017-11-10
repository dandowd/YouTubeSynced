import { InjectionToken } from '@angular/core';

export let WATCH_CONFIG = new InjectionToken('watch.config');

export interface IWatchConfig {
    apiEndpoint: string;
}

export const WatchConfig: IWatchConfig = {
    apiEndpoint: '/chat'
};