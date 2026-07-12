using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.SessionViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string Speciality { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TrainerName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int BookedCount { get; set; }
        public int Capacity { get; set; }
        public SessionStatus Status
        {
            get
            {
                var now = DateTime.Now;
                if (now < StartDate) return SessionStatus.Upcoming;
                if (now >= StartDate && now <= EndDate) return SessionStatus.Ongoing;
                return SessionStatus.Completed;
            }
        }

        public string DateDisplay => $"{StartDate:MMM dd , yyyy}";
        public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";
        public string DurationDisplay
        {
            get
            {
                var duration = EndDate - StartDate;
                if (duration.TotalDays >= 1)
                {
                    return $"{(int)duration.TotalHours} Hours {duration.Minutes} Minutes";
                }
                return $"{duration.Hours} Hours {duration.Minutes} Minutes";
            }
        }

        public string HeaderClass => Status switch
        {
            SessionStatus.Upcoming => "bg-primary",
            SessionStatus.Ongoing => "bg-success",
            SessionStatus.Completed => "bg-secondary",
            _ => "bg-secondary"
        };
    }
}
