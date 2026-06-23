using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace GymManagementSystem.DAL.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = default!;

    }
}
