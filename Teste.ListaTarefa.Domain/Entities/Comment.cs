using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teste.ListaTarefa.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Text { get; set; }
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }
        public int TaskId { get; set; }
        public virtual Task Task { get; set; }
    }
}
