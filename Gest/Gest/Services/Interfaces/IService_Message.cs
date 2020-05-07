using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;

namespace Gest.Services.Interfaces
{
    interface IService_Message
    {
        Task<IEnumerable<Message>> GetList();
        Task<Message> Get(int nb_message);
        Task<Boolean> insert(Message message);
        Task<Boolean> update(Message message);
        Task<Boolean> delete(Message message);
    }
}
