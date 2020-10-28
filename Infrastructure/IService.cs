using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Infrastructure
{
    interface IService
    {
        void Start();
        void Stop();
    }
}
