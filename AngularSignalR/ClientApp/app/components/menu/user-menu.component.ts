import { Component, Injectable, Input } from '@angular/core';
import { Menu, MenuItem } from './menu.component';

@Component({
    selector: 'user-menu',
    templateUrl: './user-menu.component.html',
    styleUrls: ['./usermenu.css'],
    animations: [

    ]
})

export class UserMenuComponent implements Menu {
    username: string;

    menuItems: MenuItem[];
    isVisible: boolean;

    constructor() {
    }

    public toggleMenu(username: string, x: number, y: number) {
        this.username = username;

        if (this.isVisible) {
            this.isVisible = false;
        }
        else {
            this.isVisible = true;
            var usermenu = document.getElementById('usermenu');
            var underMouse = document.elementFromPoint(x, y).getBoundingClientRect();
            if (usermenu) {
                usermenu.style.position = "absolute";
                usermenu.style.left = x - underMouse.left + 20 + 'px';
                usermenu.style.top = y - 55 + 'px';
            }
        }
    }
}