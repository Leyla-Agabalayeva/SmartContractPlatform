using EsignPlatform.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsignPlatform.DAL.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Template> Templates => Set<Template>();
        public DbSet<Contract> Contracts => Set<Contract>();
        public DbSet<ContractParty> ContractParties => Set<ContractParty>();
        public DbSet<Signature> Signatures => Set<Signature>();
        public DbSet<Document> Documents => Set<Document>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(e =>
            {
                e.Property(x => x.FinOrVoen).HasMaxLength(20).IsRequired();
                e.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
            });

            builder.Entity<Template>(e =>
            {
                e.Property(x => x.Name).HasMaxLength(200).IsRequired();
                e.HasQueryFilter(x => !x.IsDeleted);
            });

            builder.Entity<Contract>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(300).IsRequired();
                e.HasQueryFilter(x => !x.IsDeleted);

                e.HasOne(x => x.Template)
                    .WithMany(t => t.Contracts)
                    .HasForeignKey(x => x.TemplateId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.CreatedByUser)
                    .WithMany(u => u.CreatedContracts)
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Document)
                    .WithOne(d => d.Contract)
                    .HasForeignKey<Document>(d => d.ContractId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ContractParty>(e =>
            {
                e.Property(x => x.FinOrVoen).HasMaxLength(20).IsRequired();
                e.Property(x => x.DisplayName).HasMaxLength(200).IsRequired();
                e.HasQueryFilter(x => !x.IsDeleted);

                e.HasOne(x => x.Contract)
                    .WithMany(c => c.Parties)
                    .HasForeignKey(x => x.ContractId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Signature>(e =>
            {
                e.HasQueryFilter(x => !x.IsDeleted);

                e.HasOne(x => x.Contract)
                    .WithMany(c => c.Signatures)
                    .HasForeignKey(x => x.ContractId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.ContractParty)
                    .WithMany()
                    .HasForeignKey(x => x.ContractPartyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Document>(e =>
            {
                e.Property(x => x.Hash).HasMaxLength(128).IsRequired();
                e.HasQueryFilter(x => !x.IsDeleted);
            });
        }
    }

}
