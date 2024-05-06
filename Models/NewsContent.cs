using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.Models
{
    public class NewsContent
    {
        [Key]
        public int NewsId { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string source { get; set; }
        public string urlToImage { get; set; }
        public string author { get; set; }
        public DateTime publishedAt { get; set; }
        public DateTime dateTime { get; set; }

        [ForeignKey("Register")]
        public int UserId { get; set; }
        public virtual Register Register { get; set; }
    }
}
