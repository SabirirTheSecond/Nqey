﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.DAL;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;

namespace Nqey.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly DataContext _dataContext;


        public ReservationService(IServiceRepository serviceRepository,IClientRepository clientRepository , DataContext dataContext)
        {
            _serviceRepository = serviceRepository;
            _clientRepository = clientRepository;
            _dataContext = dataContext;

        }

        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            var existing = await GetReservationByIdAsync(id);
            if (existing == null)
                return null;
            _dataContext.Reservations.Update(reservation);
            await _dataContext.SaveChangesAsync();

            return reservation;

        }

        public async Task<Reservation> AcceptReservationAsync(int id)
        {
            var toAccept = await _dataContext.Reservations
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (toAccept == null)
                return null;

            toAccept.Status = ReservationStatus.Accepted;
            await _dataContext.SaveChangesAsync();
            return toAccept;

        }
        public async Task<Reservation> CancelReservationAsync(int id)
        {
            var toCancel = await _dataContext.Reservations
                .Include(r=>r.Client)
                .Include(r => r.Provider)
                .FirstOrDefaultAsync(r=> r.ReservationId == id);
            // makes sure that the reservation isn't confirmed by the provider
            // left for more analysis and discussion

            //if (toCancel.Status == ReservationStatus.Accepted)
            //    return null;


            toCancel.Status = ReservationStatus.Cancelled;
            await _dataContext.SaveChangesAsync();
            return toCancel;
            
        }

        public async Task<Reservation> DeleteReservationAsync(int id)
        {
            var toDelete = await _dataContext.Reservations
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .FirstOrDefaultAsync(r => r.ReservationId == id);
           
            if (toDelete == null)
                return null;

            _dataContext.Reservations.Remove(toDelete);
            await _dataContext.SaveChangesAsync();

            return toDelete;

        }

        public async Task<Reservation> GetReservationByIdAsync(int id)
        {
            var reservation = await _dataContext.Reservations
                .Include(r=> r.Provider)
                .Include(r=> r.Client)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (reservation == null)
                return null;
            return reservation;

            
        }

       public async Task<List<Reservation>> GetReservationsAsync()
        {
            var reservations = await _dataContext.Reservations
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .ToListAsync();
            if (reservations == null)
                return null;

            return reservations;
            
        }

       public async Task<Reservation> MakeReservationAsync(Reservation reservation)
        {
            
            var provider = await _dataContext.Providers.SingleOrDefaultAsync(p => p.ProviderId == reservation.ProviderId);
            var service = await _serviceRepository.GetServiceByIdAsync(provider.ServiceId);
            var client = await _clientRepository.GetClientByIdAsync(reservation.ClientId);
            if (service == null || provider == null || client == null)
                return null;

           await _dataContext.Reservations.AddAsync(reservation);
            await _dataContext.SaveChangesAsync();
            return reservation;
            
        }
    }
}
