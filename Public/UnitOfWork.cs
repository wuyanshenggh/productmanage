using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductMange.Model;

namespace ProductMange
{
    public class UnitOfWork:IDisposable
    {
        private DbContext _context;
        Repository<Prc_VersionInfo> _versionInfoRepository;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _versionInfoRepository = new Repository<Prc_VersionInfo>(context);

        }

        public Repository<Prc_VersionInfo> VersionInfoRepository
        {
            get
            {
                return _versionInfoRepository ?? new Repository<Prc_VersionInfo>(_context);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        void IDisposable.Dispose()
        {
            _context.Dispose();
        }
    }
}
