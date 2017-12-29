import { Component, DoCheck, EventEmitter } from '@angular/core';
import { WatchService, UserDetails, UserEvent, ReadyEvent } from '../watch/watch.service';
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
                console.log(this.users)
            }
        });

        this.watchService.setEmitter.subscribe((data: UserDetails[]) => {
            data.forEach(value => {
                console.log(value);
                this.users.push(value);
            });
        })

        this.watchService.readyStatusEmitter.subscribe((data: ReadyEvent) => {
            var t = this.users.find(x => x.name == data.user);
            if (t) {
                t.readyStatus = data.readyChange;
                this.users.splice(this.users.indexOf(t), 1, t);
            }
        });

    }

    public ngDoCheck() {
        //console.log('docheck');
    }
}