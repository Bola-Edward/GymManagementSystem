using GymManagementSystem.DAL.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagementSystem.BusinessLogic.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1, 300, ErrorMessage = "Height must be greater than 0")]
        public decimal Height { get; set; }

        [Range(0.1, 500, ErrorMessage = "Weight must be greater than 0")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "Blood Type Is Required")]
        [EnumDataType(typeof(BloodType), ErrorMessage = "Invalid Blood Type")]
        public BloodType BloodType { get; set; } = default!;
        public string? Note { get; set; } = default!;
    }
}
