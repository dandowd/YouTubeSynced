// stack overflow about user movement https://stackoverflow.com/questions/41162620/angular-2-lifecycle-event-after-constructor-called
import { Component, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    templateUrl: './home.component.html'
})

export class HomeComponent{
    userName = '';
    roomName = '';

    constructor(private router: Router) {

    }

    joinRoom() {
        this.router.navigate(['watch', this.roomName], { queryParams: { username: this.userName } });
    }
}