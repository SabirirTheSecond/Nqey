using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.DAL;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;

namespace Nqey.Services.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly DataContext _dataContext;


        public ReservationService(IServiceRepository serviceRepository,
            IClientRepository clientRepository ,IUserRepository userRepository ,DataContext dataContext)
        {
            _serviceRepository = serviceRepository;
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _dataContext = dataContext;

        }

        public async Task<Reservation> UpdateReservationAsync(int id, Reservation reservation)
        {
            var existing = await GetReservationByIdAsync(id);
            if (existing == null)
                return null;

            reservation.Events.Add(new ReservationEvent
            { ReservationEventType = ReservationEventType.Changed,
                CreatedAt = DateTime.UtcNow,
            Notes = "Reservation updated"
            }
            );
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
            // Booking timeline tracker
            toAccept.Events.Add(
                new ReservationEvent
                {
                    ReservationEventType = ReservationEventType.Accepted,
                    CreatedAt = DateTime.UtcNow,
                    Notes = "Reservation Accepted"
                }
                );
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
            toCancel.Events.Add(
               new ReservationEvent
               {
                   ReservationEventType = ReservationEventType.Cancelled,
                   CreatedAt = DateTime.UtcNow,
                   Notes = "Reservation Cancelled"
               }
               );
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
                .Include(r => r.Events)
                .Include(r => r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j=> j.Images)
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
                .Include(r => r.Events)
                .Include(r=> r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j=> j.Images)
                .ToListAsync();
            if (reservations == null)
                //throw new NullReferenceException();
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

            reservation.JobDescription.Reservation = reservation;
            await _dataContext.JobDescriptions.AddAsync(reservation.JobDescription);
           await _dataContext.Reservations.AddAsync(reservation);
            await _dataContext.SaveChangesAsync();
            return reservation;
            
        }

        public  Location GetReservationLocation(Reservation reservation)
        {
            if(reservation.Location == null)
              return null;

            return reservation.Location;
        }

        public async Task<List<Reservation>> GetReservationByClientIdAsync(int clientId)
        {
            var reservations = await _dataContext.Reservations
                .Where(r=> r.ClientId == clientId)
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .Include(r => r.Events)
                .Include(r => r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j => j.Images)
                .ToListAsync();
            if (reservations == null)
                //throw new NullReferenceException();
                return null;

            return reservations;
            
        }
       public async Task<List<Domain.Reservation>> GetMyReservationsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var clientId = await _clientRepository.GetClientIdByUserNameAsync(user.UserName);
         
            var reservations = await _dataContext.Reservations
               .Where(r => r.Client == userId || r.Provider.UserId == userId)
               .ToListAsync();


        }
        public async Task<List<Reservation>> GetReservationByProviderIdAsync(int providerId)
        {
            var reservations = await _dataContext.Reservations
                .Where(r => r.ProviderId == providerId)
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .Include(r => r.Events)
                .Include(r => r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j => j.Images)
                .ToListAsync();
            if (reservations == null)
                //throw new NullReferenceException();
                return null;

            return reservations;

        }
    }
}
