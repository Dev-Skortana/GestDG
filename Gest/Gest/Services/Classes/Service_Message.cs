using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Database_Initialize;
using Gest.Models;
using Gest.Services.Interfaces;
using SQLite;

namespace Gest.Services.Classes
{
        
    class Service_Message : IService_Message
    {
                                                                         /* CRUD  basique ,ne pas faire d'ajout */
        public async Task<bool> delete(Message message)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.ExecuteAsync($"delete from messages where messages.nb_message={message.nb_message}");
            return (nombres_records >= 1);
        }

        public async Task<Message> Get(int nb_message)
        {
            var connection = await Database_configuration.Database_Initialize();
            var messages = await connection.QueryAsync<Message>($"Select * from messages where messages.nb_message={nb_message}");
            return messages.Count!=0 ? messages[0] : null;
        }

        public async Task<IEnumerable<Message>> GetList()
        {
            var connection = await Database_configuration.Database_Initialize();
            var messages = await connection.QueryAsync<Message>($"Select * from messages");
            return messages;
        }

        public async Task<bool> insert(Message message)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = 0;
            try
            {
                nombres_records = await connection.InsertAsync(message);
            }
            catch (SQLiteException exception)
            {
                if (exception.Result != SQLite3.Result.Constraint)
                {
                    throw SQLiteException.New(exception.Result, exception.InnerException.ToString());
                }
            }
            return (nombres_records >= 1);
        }

        public async Task<bool> update(Message message)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.ExecuteAsync($"update messages set messages.nb_message={message.nb_message} where messages.nb_message={message.nb_message}");
            return (nombres_records >= 1);
        }
    }
}
