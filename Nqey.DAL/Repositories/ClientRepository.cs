﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;

namespace Nqey.DAL.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _dataContext;
        public ClientRepository(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

       public async Task<Client> AddClientAsync(Client client)
        {
            await _dataContext.Clients.AddAsync(client);
            await _dataContext.SaveChangesAsync();
            return client;
        }

       public async Task<Client> DeleteClientAsync(int id)
        {
            var toDelete = await _dataContext.Clients
                .FirstOrDefaultAsync(c => c.ClientId == id);
            if (toDelete == null)
                return null;
            _dataContext.Clients.Remove(toDelete);
            await _dataContext.SaveChangesAsync();
            return toDelete;
        }

       public async Task<Client> GetClientByIdAsync(int id)
        {
            var client = await _dataContext.Clients
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (client == null)
                return null;

            return client;
        }

       public async Task<List<Client>> GetClientsAsync()
        {
            var clients = await _dataContext.Clients.ToListAsync();
            if (clients == null)
                return null;
            return clients;
            
        }

       public async Task<Client> UpdateClientAsync(Client client)
        {
            _dataContext.Clients.Update(client);
            if (client == null)
                return null;
            await _dataContext.SaveChangesAsync();

            return client;
        }
    }
}
