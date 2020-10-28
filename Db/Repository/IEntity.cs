using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Db.Repository
{
    public interface IEntity
    {
        int Id { get; set; }
    }
}
