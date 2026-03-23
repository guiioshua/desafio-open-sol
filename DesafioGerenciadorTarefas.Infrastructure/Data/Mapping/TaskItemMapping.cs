using DesafioGerenciadorTarefas.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioGerenciadorTarefas.Infrastructure.Data.Mappings
{
    public class TaskItemMapping : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("Tasks");
            
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(t => t.Description)
                .HasMaxLength(500);

            builder.Property(t => t.Status)
                .IsRequired();
            
            builder.Property(t => t.CreatedTime)
                .IsRequired();

             builder.Property(t => t.LastUpdate)
                .IsRequired();
        }

    }
}
