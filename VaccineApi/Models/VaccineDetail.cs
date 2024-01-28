using System;
using System.Collections.Generic;

namespace VaccineApi.Models
{
    public partial class VaccineDetail
    {
        public int VaccineId { get; set; }
        public string? VaccineName { get; set; }
        public string? Manufacturer { get; set; }
        public int? Stock { get; set; }
    }
}
