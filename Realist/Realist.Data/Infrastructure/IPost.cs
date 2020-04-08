using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Realist.Data.Infrastructure
{
    public interface IPost
    {
        Task<bool> Post();
    }
}
