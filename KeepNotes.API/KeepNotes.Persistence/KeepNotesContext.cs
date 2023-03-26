using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeepNotes.Persistence
{
    public class KeepNotesContext : DbContext
    {
        public KeepNotesContext(DbContextOptions<KeepNotesContext> options) : base(options) { }

        protected KeepNotesContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
