![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/871/badge)
# Digital Aprenticeship Service
## Public sector reporting

### Developer Setup

#### Requirements

1. Install [Visual Studio] with these workloads:
    - ASP.NET and web development
    - Azure development
    - .NET desktop development
2. Install [SQL Server Management Studio]
3. Install [Azure Storage Explorer]
4. Administator Access
5. Optional - Install Redis (please configure session storage as applicable)

#### Setup

##### Open the solution

- Open Visual Studio as an administrator
- Open the solution
- Set SFA.DAS.PSRService.Web as the startup project
- Running the solution will launch the site in your browser

##### Publish the databases

Steps:

* Right click on the db project in the solution explorer (SFA.DAS.PSRService.Database)
* Click on publish menu item
* Click the edit button
* Select Local > ProjectsV13
* Add the project name in again as the Database name (i.e. SFA.DAS.PSRService.Database)
* Click publish

##### Add configuration to Azure Storage Emulator

The configuration is loaded from azure table storage.

* Run the Azure Storage Emulator
* Clone the [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config) repository
* Clone the [das-employer-config-updater](https://github.com/SkillsFundingAgency/das-employer-config-updater) repository
* Run the das-employer-config-updater console application and follow the instructions to import the config from the das-employer-config directory

> The two repos above are private. If the links appear to be dead make sure you are logged into github with an account that has access to these (i.e. that you are part of the Skills Funding Agency Team organization).

##### How to configure session storage

There are two different storage for session information which can be configured by editing the config stored in Azure Storage. The two options are:

* 'Default' - This stores all session data in cookies. This can cause issues when using internet explorer due to the cookie length exceeding the maximum length
* 'Redis' - This stores the session in the Redis cache and stores a pointer to the Redis data in the cookie, reducing the cookie size significantly.

