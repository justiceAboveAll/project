import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import {NavbarComponent} from './components/navbar.component';
import {HomeComponent} from './components/home.component';
import {InstanceListComponent} from './components/instance-list.component';
import {routing} from './app.routing';
import {DropdownDirective} from "./dropdown.directive";


@NgModule({
    imports: [BrowserModule, routing],
    declarations: [AppComponent,
                   NavbarComponent,
                   HomeComponent,
                   InstanceListComponent,
                   DropdownDirective],
    bootstrap:[AppComponent]
})
export class AppModule { }
