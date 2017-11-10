import { Component, Input, ViewChild, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';

import { WatchComponent } from '../watch/watch.component';

@Component({
    templateUrl: './viewer.component.html'
})

export class Viewer {
    @Input() comps: Component[];
    currentComponent: number = 0;
    @ViewChild(Viewer) viewer: Viewer;

    constructor(private componentFactory: ComponentFactoryResolver, private viewContainerRef: ViewContainerRef) {

    }

    private sayHello() {
        const factory = this.componentFactory.resolveComponentFactory(WatchComponent)
        console.log(factory);
        const ref = this.viewContainerRef.createComponent(factory);
        ref.changeDetectorRef.detectChanges();
    }
}