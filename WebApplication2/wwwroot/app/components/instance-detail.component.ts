import { Component, OnInit, Input } from '@angular/core';
import {IInstance} from '../instance';

@Component({
    moduleId: module.id,
    selector: 'instance-detail',
    templateUrl: 'instance-detail.component.html'
})
export class InstanceDetailComponent implements OnInit {
    @Input() selectedInstance: IInstance;

    constructor() { }

    ngOnInit(){
    }
}
