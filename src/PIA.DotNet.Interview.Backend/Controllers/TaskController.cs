using Microsoft.AspNetCore.Mvc;using Microsoft.Extensions.Configuration;using Microsoft.Extensions.Logging;using Newtonsoft.Json;using PIA.DotNet.Interview.Backend.Service;using PIA.DotNet.Interview.Core.Models;using PIA.DotNet.Interview.Core.Repositories;using System;using System.Collections.Generic;using System.Threading.Tasks;namespace PIA.DotNet.Interview.Backend.Controllers{    [Route("api/[controller]")]    [ApiController]    public class TaskController : ControllerBase    {        private readonly ITaskLogicService _taskLogicService;        private readonly ILogger<TaskController> _logger;        public TaskController(ITaskLogicService taskLogic, ILogger<TaskController> logger)        {            _taskLogicService = taskLogic;            _logger = logger;        }        [HttpGet("[action]")]        public async Task<IEnumerable<TaskViewModel>> GetTasks()        {            try            {                return await _taskLogicService.Get();            }            catch(Exception ex)            {                _logger.LogError(ex, "##{p1}## TaskController-GetTasks() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));                return new List<TaskViewModel>();            }        }        [HttpPost("[action]")]        public async Task<bool> AddTask(TaskViewModel taskViewModel)        {            try            {                return await _taskLogicService.Add(taskViewModel);            }            catch (Exception ex)            {                _logger.LogError(ex, "##{p1}## TaskController-AddTask() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));                return false;            }        }        [HttpPost("[action]")]           public async Task<bool>  EditTask(string id, TaskViewModel taskViewModel)           {               try               {                   return await _taskLogicService.Edit(id, taskViewModel);               }               catch (Exception ex)               {                   _logger.LogError(ex, "##{p1}## TaskController-EditTask() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));                   return false;               }           }         [HttpPost("[action]")]       public  async Task<bool>  DeleteTask(string id)       {           try           {               return await _taskLogicService.Delete(id);           }           catch (Exception ex)           {               _logger.LogError(ex, "##{p1}## TaskController-Delete() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));               return false;           }       }       [HttpGet("[action]")]       public async Task<TaskViewModel>  GetTaskbyID(string id)       {           try           {               return await _taskLogicService.Get(id);           }           catch (Exception ex)           {               _logger.LogError(ex, "##{p1}## TaskController-GetTaskbyID() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));               return new TaskViewModel();           }       }       [HttpPost("[action]")]       public async Task<bool>  ChangeStatusTask(string id,bool Status)       {           try           {               var task =await _taskLogicService.Get(id);               task.IsFinished = Status;               return await _taskLogicService.Edit(id, task);           }           catch (Exception ex)           {               _logger.LogError(ex, "##{p1}## TaskController-ChangeStatusTask() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));               return false;           }       }       [HttpPost("[action]")]       public async Task<bool>  DeleteTaskbyid(string id)       {           try           {               return await _taskLogicService.Delete(id);           }           catch (Exception ex)           {               _logger.LogError(ex, "##{p1}## TaskController-DeleteTaskbyid() Exception", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));               return false;           }       }    }}