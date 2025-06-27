using System;
using System.Collections.Generic;
using ST10442835_PROG6221_POE.Models;

namespace ST10442835_PROG6221_POE.Managers
{
    public class TaskManager
    {
        public List<CyberTask> Tasks { get; private set; } = new List<CyberTask>();

        public void AddTask(string title, string description, DateTime? reminderDate = null)
        {
            Tasks.Add(new CyberTask
            {
                Title = title,
                Description = description,
                ReminderDate = reminderDate
            });
        }

        
    }
}
