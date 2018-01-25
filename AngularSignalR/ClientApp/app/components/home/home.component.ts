// stack overflow about user movement https://stackoverflow.com/questions/41162620/angular-2-lifecycle-event-after-constructor-called
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Http } from '@angular/http';

@Component({
    templateUrl: './home.component.html'
})

export class HomeComponent{
    userName = '';
    roomName = '';

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string, private router: Router) {

    }

    joinRoom() {       
        this.http.post(this.baseUrl + 'Home/TrySignin?username=' + this.userName + '&roomname=' + this.roomName, '').subscribe(result => {
            var body = JSON.parse(result.text());
            console.log(body.guid);
            if (body) {
                this.router.navigate(['watch', this.roomName], { queryParams: { username: this.userName, guid: body.guid }});
            }
        }, error => console.error(error));
    }
}