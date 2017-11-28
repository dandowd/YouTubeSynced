import { Component } from '@angular/core';
import { WatchService } from '../watch/watch.service';

@Component({
    selector: 'app-user-list-component',
    templateUrl: './user-list.component.html'
})

export class UserListComponent {

    users: string[] = [];

    constructor(private watchService: WatchService) {

    }
}