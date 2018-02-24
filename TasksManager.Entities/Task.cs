using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TasksManager.Entities
{
    public class Task : DomainObject
    {
        private TaskStatus _status;
        private DateTime _createDate;

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(4096)]
        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreateDate
        {
            get
            {
                if (_createDate == default(DateTime))
                    _createDate = DateTime.Now;
                return _createDate;
            }
            private set => _createDate = value;
        }

        public DateTime? CompleteDate { get; private set; }

        [Required]
        public TaskStatus Status
        {
            get => _status;
            set
            {
                if (value == TaskStatus.Completed)
                    CompleteDate = DateTime.Now;
                _status = value;
                
            }
        }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<TagsInTask> Tags { get; set; }
    }
}
