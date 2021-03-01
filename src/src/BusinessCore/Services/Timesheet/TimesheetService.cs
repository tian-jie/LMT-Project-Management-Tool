using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ServiceInterfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class TimesheetService : ITimesheetService
    {
        IClientService _clientService;
        IEmployeeService _employeeService;
        IGroupService _groupService;
        IProjectService _projectService;
        ITimeEntryService _timeEntryService;
        IClockifyService _clockifyService;
        IUserGroupService _userGroupService;
        IProjectTaskService _projectTaskService;
        private readonly ILogger<TimesheetService> _logger;
        private readonly IEmployeeTitleService _employeeTitleService;

        public TimesheetService(IClientService clientService,
            IEmployeeService employeeService,
            IGroupService groupService,
            IProjectService projectService,
            ITimeEntryService timeEntryService,
            IClockifyService clockifyService,
            IUserGroupService userGroupService,
            IProjectTaskService projectTaskService,
            IEmployeeTitleService employeeTitleService,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<TimesheetService>();

            _clientService = clientService;
            _employeeService = employeeService;
            _groupService = groupService;
            _projectService = projectService;
            _timeEntryService = timeEntryService;
            _clockifyService = clockifyService;
            _userGroupService = userGroupService;
            _projectTaskService = projectTaskService;
            _employeeTitleService = employeeTitleService;

        }
        public async Task<int> SyncUsers(string userid, string token)
        {
            // 先到clockify获取数据
            var ls = await _clockifyService.GetUsers(userid, token);
            var employees = new List<Employee>();
            var employeeTitles = new List<EmployeeTitle>();
            foreach (var l in ls)
            {
                employees.Add(new Employee()
                {
                    Gid = l.id,
                    FullName = l.name,
                    Email = l.email,
                    ProfilePicture = l.profilePicture,
                    Status = l.status,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = userid,
                    CreatedUserName = userid,
                    IsDeleted = false,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserID = userid,
                    UpdatedUserName = userid
                });

                employeeTitles.Add(new EmployeeTitle()
                {
                    EmployeeGid = l.id,
                    IsDeleted = false,
                    IsLatest = true,
                    Month = DateTime.Now.ToString("yyyyMM"),
                    RoleId = 0
                });
            }

            // 然后在本地插入或者同步数据
            await _employeeService.Clear();
            await _employeeTitleService.SqlExecuteNonQuery($"delete from EmployeeTitle where Month>='{DateTime.Now.ToString("yyyyMM")}' and Month<'{DateTime.Now.AddMonths(1).ToString("yyyyMM")}'");

            var affectedRecordCount = await _employeeService.AddManyAsync(employees);
            affectedRecordCount = await _employeeTitleService.AddManyAsync(employeeTitles);

            return affectedRecordCount;
        }

        public async Task<int> SyncGroups(string userid, string token)
        {
            // 先到clockify获取数据
            var ls = await _clockifyService.GetUserGroups(userid, token);
            var list = new List<Group>();

            var userGroups = new List<UserGroup>();
            foreach (var l in ls)
            {
                list.Add(new Group()
                {
                    Gid = l.id,
                    Name = l.name,
                    CompanyId = "",
                    WorkspaceId = l.workspaceId,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = userid,
                    CreatedUserName = userid,
                    IsDeleted = false,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserID = userid,
                    UpdatedUserName = userid
                });

                foreach (var uid in l.userIds)
                {
                    userGroups.Add(new UserGroup()
                    {
                        GroupId = l.id,
                        UserId = uid,
                        CreatedDate = DateTime.Now,
                        CreatedUserID = userid,
                        CreatedUserName = userid,
                        IsDeleted = false,
                        UpdatedDate = DateTime.Now,
                        UpdatedUserID = userid,
                        UpdatedUserName = userid
                    });
                }
            }

            // 然后在本地插入或者同步数据
            await _groupService.Clear();
            var affectedRecordCount = await _groupService.AddManyAsync(list);

            await _userGroupService.Clear();
            var affectedRecordCount1 = await _userGroupService.AddManyAsync(userGroups);

            return affectedRecordCount1;
        }

        public async Task<int> SyncProjects(string userid, string token)
        {
            // 先到clockify获取数据
            var ls = await _clockifyService.GetProjects(userid, token);
            var projects = new List<Project>();
            var projectTasks = new List<ProjectTask>();
            foreach (var l in ls)
            {
                projects.Add(new Project()
                {
                    Gid = l.id,
                    Name = l.name,
                    Archived = l.archived,
                    Billable = l.billable,
                    ClientId = l.clientId,
                    Color = l.color,
                    Duration = l.duration,
                    IsPublic = l.Public,
                    Note = l.note,
                    WorkspaceId = l.workspaceId,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = userid,
                    CreatedUserName = userid,
                    IsDeleted = false,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserID = userid,
                    UpdatedUserName = userid
                });

                projectTasks.Add(new ProjectTask()
                {
                    Gid = null,
                    ProjectGid = l.id
                });
            }

            // 然后在本地插入或者同步数据
            await _projectService.Clear();
            await _projectTaskService.SqlExecuteNonQuery("delete from [PROJECTTASK] where Gid is null");

            var affectedRecordCount = await _projectService.AddManyAsync(projects);

            // TODO: 同时在Task表里增加对应的空task的项目记录。
            var n = await _projectTaskService.AddManyAsync(projectTasks);

            return affectedRecordCount;
        }

        public async Task<int> SyncClients(string userid, string token)
        {
            // 先到clockify获取数据
            var ls = await _clockifyService.GetClients(userid, token);
            var list = new List<Client>();
            foreach (var l in ls)
            {
                list.Add(new Client()
                {
                    Gid = l.id,
                    Name = l.name,
                    Note = l.note,
                    CreatedDate = DateTime.Now,
                    CreatedUserID = userid,
                    CreatedUserName = userid,
                    IsDeleted = false,
                    UpdatedDate = DateTime.Now,
                    UpdatedUserID = userid,
                    UpdatedUserName = userid
                });
            }

            // 然后在本地插入或者同步数据
            await _clientService.Clear();

            var affectedRecordCount = await _clientService.AddManyAsync(list);

            return affectedRecordCount;
        }

        public async Task<int> SyncTimeRecordsV2(string userid, string token, DateTime startDate, DateTime endDate)
        {
            // 先到clockify获取数据
            var ls = await _clockifyService.GetTimeEntriesV2(userid, token, startDate, endDate);
            _logger.LogDebug(JsonConvert.SerializeObject(ls));
            var list = new List<TimeEntry>();

            //_projectService.ListAsync(a=>a.)
            var projectIdList = (await _projectService.ListAllAsync()).Select(a => a.Gid).ToList();
            var taskIdList = (await _projectTaskService.ListAllAsync()).Select(a => a.Gid).ToList();
            var tasksToAdd = new List<ProjectTask>();

            foreach (var l in ls)
            {
                try
                {
                    list.Add(new TimeEntry()
                    {
                        Gid = l._id,
                        UserId = l.userId,
                        //WorkspaceId = l.workspaceId,
                        Description = l.description,
                        //IsBillable = 
                        IsLocked = l.isLocked,
                        ProjectId = l.projectId,
                        TaskId = l.taskId,
                        TotalHours = (decimal)(l.timeInterval.duration / 3600.0f),
                        Date = l.timeInterval.start.Date.AddHours(8),
                        CreatedDate = DateTime.Now,
                        CreatedUserID = userid,
                        CreatedUserName = userid,
                        IsDeleted = false,
                        UpdatedDate = DateTime.Now,
                        UpdatedUserID = userid,
                        UpdatedUserName = userid
                    });

                    // 检查TaskGID是不是已经存在，如果不存在，同时在Task表里增加对应的空task的项目记录。
                    if (!taskIdList.Contains(l.taskId))
                    {
                        taskIdList.Add(l.taskId);
                        tasksToAdd.Add(new ProjectTask()
                        {
                            Gid = l.taskId,
                            ProjectGid = l.projectId
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    _logger.LogError(JsonConvert.SerializeObject(l));
                }
            }

            // 然后在本地插入或者同步数据
            await _timeEntryService.DeleteAsync(a => a.Date >= startDate && a.Date < endDate.AddDays(1));
            //var affectedRecordCount = _timeEntryService.SqlExecute($"delete from timeentry where [date]>='{startDate.ToString("yyyy-MM-dd")}' and [date]<'{endDate.AddDays(1).ToString("yyyy-MM-dd")}'");

            var insertedRecordCount = await _timeEntryService.AddManyAsync(list);

            var cnt = _projectTaskService.AddManyAsync(tasksToAdd);

            return insertedRecordCount;
        }
        public async Task<int> SyncTimeRecordsV3(string userid, string token, DateTime startDate, DateTime endDate)
        {
            // 先到clockify获取数据
            var byUser = await _clockifyService.GetTimeEntriesV3(userid, token, startDate, endDate);
            _logger.LogDebug(JsonConvert.SerializeObject(byUser));

            // 然后在本地插入或者同步数据
            await _timeEntryService.DeleteAsync(a => a.Date >= startDate && a.Date < endDate.AddDays(1));

            var insertedRecordCount = await _timeEntryService.AddManyAsync(byUser);

            return insertedRecordCount;
        }

    }
}
