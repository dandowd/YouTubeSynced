import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserMenuComponent } from './user-menu.component';

@NgModule({
    imports: [
        CommonModule
    ],
    declarations: [
        UserMenuComponent,
    ],
    exports: [
        UserMenuComponent
    ]
})

export class MenuModule {}