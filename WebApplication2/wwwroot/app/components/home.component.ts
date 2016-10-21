import { Component } from '@angular/core';
@Component({
    moduleId: module.id,
    selector: 'home',
    templateUrl: 'home.component.html'
})
export class HomeComponent {
    private textAbovePic: string;
    private textUnderPic: string;
    private textDeveloped: string;

    constructor() {
        this.textAbovePic = "Save 2 hours each day monitoring your SQL Servers";
        this.textUnderPic = `SQL Monitor is a SQL server monitoring tool that transforms
                           the way you look at your database. It cuts your daily check
                            to minutes, with a web-based overview of all your SQL Servers.`;
        this.textDeveloped = "Developed by: If-068.net";
    }
}

