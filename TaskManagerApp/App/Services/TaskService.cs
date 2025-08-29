using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepo _taskRepo;
        public TaskService(ITaskRepo taskRepo) => _taskRepo = taskRepo; 

        public async Task<List<TaskItem>> GetOngoingTasks(string userId) =>
            await _taskRepo.GetTasksForUserAsync(userId, false);

        public async Task<List<TaskItem>> GetCompletedTasks(string userId) =>
            await _taskRepo.GetTasksForUserAsync(userId, true);

        public async Task AddTask(TaskItem task) =>
            await _taskRepo.AddAsync(task);

        public async Task CompleteTask(TaskItem task)
        {
            task.IsCompleted = true;
            await _taskRepo.UpdateAsync(task);
        }
    }
}
