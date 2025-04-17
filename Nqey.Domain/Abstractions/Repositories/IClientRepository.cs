using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IClientRepository
    {
        Task<List<Client>> GetClientsAsync();
        Task<Client> GetClientByIdAsync(int id);
        Task<int?> GetClientIdByUserNameAsync(string userName);
        Task<Client> AddClientAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task<Client> DeleteClientAsync(int id);

    }
}
