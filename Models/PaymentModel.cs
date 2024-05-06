using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.Models
{
    public class PaymentModel
    {
        [Key]
        public int PaymentId { get; set; }
        public string PaymentMode { get; set; }
        public string ReferenceNo { get; set; }
        public int Amount { get; set; }
        public DateTime Validity { get; set; }
        public DateTime CreateTime { get; set; }
        public string Status { get; set; }

        [ForeignKey("Register")]
        public int UserId { get; set; }
        public virtual Register Register { get; set; }
    }
}
