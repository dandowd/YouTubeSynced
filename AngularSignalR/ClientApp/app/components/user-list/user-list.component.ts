import { Component } from '@angular/core';
import { WatchService, UserDetails } from '../watch/watch.service';

@Component({
    selector: 'app-user-list-component',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.css']
})

export class UserListComponent {

    users: UserDetails[] = [];

    constructor(private watchService: WatchService) {
        this.watchService.userEmitter.subscribe((data: UserDetails) => {
            var t = data;
            console.log(data);
            this.users.push(data);
        });

        this.watchService.setEmitter.subscribe((data: UserDetails[]) => {
            data.forEach(value => {
                this.users.push(value);
            });
        })
    }
}