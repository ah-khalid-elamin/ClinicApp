using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Helpers.RequestResolver
{
    public interface IRequestResolver
    {
        Task<List<Attachment>> Resolve(string message);
    }
}
