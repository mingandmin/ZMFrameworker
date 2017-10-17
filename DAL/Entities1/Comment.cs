using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAppDAL.Entities1
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreationTime { get; set; }

        public int? BlogId { get; set; }
        public int? PosterId { get; set; }
        public virtual Blog Blog { get; set; }
        public virtual User User { get; set; }

        public string  User11 { get; set; }
    }
}
