using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wacomi.API.Data;
using Wacomi.API.Helper;

namespace Wacomi.Xunit
{
    public class ContextMapperFixture : IDisposable
    {
        public ApplicationDbContext Context {get;}
        public IMapper Mapper {get;}

        public ContextMapperFixture(){
             var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseInMemoryDatabase();
            Context = new ApplicationDbContext(optionsBuilder.Options);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfiles());
            });
            Mapper = mockMapper.CreateMapper();
        }

        public void Dispose()
        {
        }
    }
}