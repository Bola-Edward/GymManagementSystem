using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        // use interceptor to set the CreatedAt, UpdatedAt, DeletedAt, IsDeleted properties automatically
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
