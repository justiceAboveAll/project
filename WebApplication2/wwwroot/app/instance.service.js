"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var InstanceService = (function () {
    function InstanceService() {
        this.instances = [
            {
                "instanceId": 1,
                "instanceName": "SQL Server (SQLEXPRESS)",
                "serverName": "CD1002-DO1/SQLEXPRESS",
                "version": "10.50.1600.1",
                "dbs": 2,
                "users": 12,
                "status": "online",
                "hostOs": "Debian 6.0.10",
                "hostCpu": "4",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            },
            {
                "instanceId": 2,
                "instanceName": "SQL Server (MSSQLSERVER)",
                "serverName": "CD1002-DO1/SQLEXPRESS",
                "version": "10.0.2531",
                "dbs": 3,
                "users": 16,
                "status": "online",
                "hostOs": "CentOS 6.6",
                "hostCpu": "8",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            },
            {
                "instanceId": 3,
                "instanceName": "SQL Server Agent (SQLEXPRESS)",
                "serverName": "CD1002-DO1/SQLDEVO1",
                "version": "10.50.1600.1",
                "dbs": 1,
                "users": 8,
                "status": "offline",
                "hostOs": "Debian 6.0.10",
                "hostCpu": "4",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            },
            {
                "instanceId": 4,
                "instanceName": "SQL Server Agent (MSSQLEXPRESS)",
                "serverName": "CD1002-DO1/SQLPRODO1",
                "version": "10.0.2531",
                "dbs": 3,
                "users": 27,
                "status": "online",
                "hostOs": "Debian 6.0.10",
                "hostCpu": "4",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            },
            {
                "instanceId": 5,
                "instanceName": "SQL Server Reporting Services",
                "serverName": "CD1002-DO1/SQLEXPRESS",
                "version": "10.50.1600.1",
                "dbs": 2,
                "users": 10,
                "status": "unknown",
                "hostOs": "Debian 6.0.10",
                "hostCpu": "8",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            },
            {
                "instanceId": 6,
                "instanceName": "SQL Server Analysis Services",
                "serverName": "CD1002-DO1/SQLPRODO1",
                "version": "10.50.1600.1",
                "dbs": 2,
                "users": 3,
                "status": "unknown",
                "hostOs": "Ubuntu 14.04",
                "hostCpu": "8",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            },
            {
                "instanceId": 7,
                "instanceName": "SQL Server Browser",
                "serverName": "CD1002-DO1/SQLPRODO1",
                "version": "10.0.2531",
                "dbs": 2,
                "users": 5,
                "status": "offline",
                "hostOs": "CentOS 6.6",
                "hostCpu": "8",
                "listOfDbs": "",
                "listOfUsers": "",
                "listOfRoles": ""
            }
        ];
    }
    InstanceService.prototype.getInstances = function () {
        return this.instances;
    };
    InstanceService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], InstanceService);
    return InstanceService;
}());
exports.InstanceService = InstanceService;
//# sourceMappingURL=instance.service.js.map