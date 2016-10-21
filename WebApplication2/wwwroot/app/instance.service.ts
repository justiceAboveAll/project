import {Injectable} from '@angular/core';
import {Instance} from './instance';

@Injectable()
export class InstanceService {
    private instances: Instance[] = [
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
    ]

    constructor() { }

    getInstances() {
        return this.instances;
    }
}