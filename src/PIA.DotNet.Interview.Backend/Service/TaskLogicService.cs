using PIA.DotNet.Interview.Core;
using PIA.DotNet.Interview.Core.Models;
using PIA.DotNet.Interview.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PIA.DotNet.Interview.Backend.Service
{
    public class TaskLogicService : ITaskLogicService
    {
        private readonly TaskSQLiteRepository _TaskSQLiteRepository;
        string connectionString;
        public TaskLogicService(ITaskRepository tasksRepository)
        {

            string inipath = Environment.CurrentDirectory + "/central.ini";
            var ini = new IniHelper(inipath);
            connectionString = "Data Source="+ini.ReadValue("Setting", "database2");
            _TaskSQLiteRepository = new TaskSQLiteRepository(connectionString);
        }

        public async Task<bool>  Add(TaskViewModel task)
        {
            var guid = new Guid();
            if (!Guid.TryParse(task.Id, out guid))
                return false;

            /*        return  await _tasksRepository.Create(new Core.Models.Task
                    {
                        Id = guid,
                        Title = task.Title,
                        Description = task.Description,
                        IsFinished = task.IsFinished,
                        Example = task.Example,
                    });*/
            int i= await _TaskSQLiteRepository.InsertIntoTaskTable(new Core.Models.Task
            {
                Id = guid,
                Title = task.Title,
                Description = task.Description,
                IsFinished = task.IsFinished,
                Example = task.Example,
            });
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            var guid = new Guid();
            if (!Guid.TryParse(id, out guid))
                return false;
            int i= await _TaskSQLiteRepository.DeleteFromTaskTable(new Guid(id));
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            /*
                        return await _tasksRepository.Delete(guid, new Core.Models.Task
                        {
                            Id = guid
                        });*/
        }

        public async Task<bool> Edit(string id, TaskViewModel task)
        {

            int i= await _TaskSQLiteRepository.UpdateTaskTable(new Core.Models.Task
            {
                Id = new Guid(task.Id),
                Title = task.Title,
                Description = task.Description,
                IsFinished = task.IsFinished,
                Example = task.Example,
            });
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

/*            return await _tasksRepository.Update(new Guid(id), new Core.Models.Task
            {
                Id = new Guid(task.Id),
                Title = task.Title,
                Description = task.Description,
                IsFinished = task.IsFinished,
                Example = task.Example,
            });*/
        }

        public async Task<TaskViewModel> Get(string id)
        {

            var taskModel = await _TaskSQLiteRepository.GetTaskById(new Guid(id));
/*            var taskModel =await _tasksRepository.Get(new Guid(id));*/

            return  new TaskViewModel
            {
                Id = taskModel.Id.ToString(),
                Title = taskModel.Title,
                Description = taskModel.Description,
                IsFinished = taskModel.IsFinished,
                Example = taskModel.Example,
            };
        }

        public async Task<IEnumerable<TaskViewModel>> Get()
        {
            var tasks = await _TaskSQLiteRepository.GetTask();

            /*            var tasks = _tasksRepository.Get();*/
            return tasks.Select(t => new TaskViewModel
            {
                Id = t.Id.ToString(),
                Title = t.Title,
                Description = t.Description,
                IsFinished = t.IsFinished,
                Example = t.Example
            });
        }


    }
}
