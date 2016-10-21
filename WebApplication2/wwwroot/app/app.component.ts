import { Component } from '@angular/core';
import { InstanceService } from './instance.service';

@Component({
    moduleId: module.id,
    selector: 'my-app',
    templateUrl: 'app.component.html',
    providers: [InstanceService]
})
export class AppComponent { }
