using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ProductMange.Model
{
    public class ProductManageDbContext: DbContext
    {
        //public DbSet<Delivery> Deliveries { get; set; }

        public ProductManageDbContext(DbContextOptions options) : base(options)
        {



        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool))
                    {
                        property.SetValueConverter(new BoolToIntConverter());
                    }
                }
            }
        }


        public class BoolToIntConverter : ValueConverter<bool, int>
        {
            public BoolToIntConverter(ConverterMappingHints mappingHints = null)
                : base(
                      v => Convert.ToInt32(v),
                      v => Convert.ToBoolean(v),
                      mappingHints)
            {
            }

            public static ValueConverterInfo DefaultInfo { get; }
                = new ValueConverterInfo(typeof(bool), typeof(int), i => new BoolToIntConverter(i.MappingHints));
        }


        public DbSet<Prc_VersionInfo> Prc_VersionInfos { get; set; }
        public DbSet<Prc_UserInfo> Prc_UserInfo { get; set; }
        public DbSet<Prc_ConfigSet> Prc_ConfigSets { get; set; }
        public DbSet<Prc_Holiday> Prc_Holidays { get; set; }
        public DbSet<Prc_UpgradeInfo> Prc_UpgradeInfos { get; set; }
        public DbSet<Prc_UpgradeInfoItem> Prc_UpgradeInfoItems { get; set; }
        public DbSet<Prc_UpgradeMessage> Prc_UpgradeMessages { get; set; }

    }
}
