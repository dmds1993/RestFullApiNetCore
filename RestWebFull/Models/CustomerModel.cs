using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestWebFull.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [Range(0, 100)]
        public int Age { get; set; }
        public void SetId(Guid id)
        {
            Id = id;
        }
    }
}
