import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home.component';
import { InstanceListComponent } from './components/instance-list.component';

const appRoutes: Routes = [
    {
        path: '',
        component: HomeComponent
    },
    {
        path: 'instance-list',
        component: InstanceListComponent
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);