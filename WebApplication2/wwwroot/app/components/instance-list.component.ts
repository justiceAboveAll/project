import { Component, OnInit } from '@angular/core';
import { Instance } from '../instance';
import { InstanceDetailComponent } from '../components/instance-detail.component';
import { InstanceService } from '../instance.service';

@Component({
    moduleId: module.id,
    selector: 'instance-list',
    templateUrl: 'instance-list.component.html',
    styleUrls: ['instance-list.component.css'],
})
export class InstanceListComponent implements OnInit {
    instances: Instance[];

    constructor(private instanceService: InstanceService) { }

    ngOnInit() {
        this.instances = this.instanceService.getInstances();
    }

    onSelecte(instance: Instance) { }
}