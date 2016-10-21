"use strict";
var router_1 = require('@angular/router');
var home_component_1 = require('./components/home.component');
var instance_list_component_1 = require('./components/instance-list.component');
var appRoutes = [
    {
        path: '',
        component: home_component_1.HomeComponent
    },
    {
        path: 'instance-list',
        component: instance_list_component_1.InstanceListComponent
    }
];
exports.routing = router_1.RouterModule.forRoot(appRoutes);
//# sourceMappingURL=app.routing.js.map