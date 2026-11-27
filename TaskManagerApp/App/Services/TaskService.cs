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
        private readonly ITaskTagRepo _tagRepo;

        public TaskService(ITaskRepo taskRepo, ITaskTagRepo tagRepo)
        {
            _taskRepo = taskRepo;
            _tagRepo = tagRepo;
        }

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

        public async Task UpdateTaskAsync(int taskId, string userId, string title, string? description, int? taskTagId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Task title is required.", nameof(title));
            }

            var task = (await _taskRepo.GetTasksForUserAsync(userId))
                .FirstOrDefault(item => item.Id == taskId);

            if (task is null)
            {
                throw new InvalidOperationException("Task not found.");
            }

            if (taskTagId is int tagId &&
                !(await _tagRepo.GetForUserAsync(userId)).Any(tag => tag.Id == tagId))
            {
                throw new InvalidOperationException("Choose one of your tags for this task.");
            }

            task.Title = title.Trim();
            task.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            task.TaskTagId = taskTagId;

            await _taskRepo.UpdateAsync(task);
        }

        public Task<List<TaskItem>> GetAllTasks(string userId) =>
            _taskRepo.GetTasksForUserAsync(userId);

        public Task DeleteTaskAsync(int taskId, string userId) =>
            _taskRepo.DeleteAsync(taskId, userId);
    }
}
