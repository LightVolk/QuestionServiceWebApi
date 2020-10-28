using QuestionServiceWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Db.Repository
{
    public class EfCoreTagRepository: EfCoreRepository<Tag,ApplicationContext>
    {
        public EfCoreTagRepository(ApplicationContext applicationContext):base(applicationContext)
        {

        }
    }
}
