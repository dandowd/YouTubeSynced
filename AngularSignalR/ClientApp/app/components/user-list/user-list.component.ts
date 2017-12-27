import { Component, DoCheck } from '@angular/core';
import { WatchService, UserDetails, UserEvent } from '../watch/watch.service';
import { List } from '../../../common/List';

@Component({
    selector: 'app-user-list-component',
    templateUrl: './user-list.component.html',
    styleUrls: ['./user-list.css']
})

export class UserListComponent implements DoCheck {

    private users: UserDetails[] = [];

    constructor(private watchService: WatchService) {
        this.watchService.userEmitter.subscribe((data: UserEvent) => {
            if (data.isJoining) {
                this.users.push(data.user);
                console.log(this.users);
            }
            else {
                this.users.splice(this.users.indexOf(data.user), 1);
            }
        });

        this.watchService.setEmitter.subscribe((data: UserDetails[]) => {
            data.forEach(value => {
                console.log(value);
                this.users.push(value);
            });
        })
    }

    public ngDoCheck() {
        console.log('docheck');
    }
}