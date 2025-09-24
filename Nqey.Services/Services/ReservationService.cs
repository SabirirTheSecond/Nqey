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

        public async Task<Reservation> UpdateReservationAsync(int id, Reservation updatedData)
        {
            var existing = await GetReservationByIdAsync(id);
            if (existing == null)
                return null;
           
            existing.Events.Add(new ReservationEvent
            { ReservationEventType = ReservationEventType.Changed,
                CreatedAt = DateTime.UtcNow,
            Notes = "Reservation updated"
            }
            );

            // modify this so it can only updates dates
             _dataContext.Update(updatedData);
            await _dataContext.SaveChangesAsync();
            return existing;

        } 
        public async Task<Reservation> AcceptReservationAsync(int id)
        {
            var toAccept = await _dataContext.Reservations
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (toAccept == null)
                return null;
            var provider = await _dataContext.Providers.FirstOrDefaultAsync(p => p.UserId == toAccept.ProviderUserId);
            provider.ProviderAnalytics.Accepts++;
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
        public async Task<Reservation> RefuseReservationAsync(int id)
        {
            var toRefuse = await _dataContext.Reservations
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (toRefuse == null)
                return null;
            var provider =await _dataContext.Providers.FirstOrDefaultAsync(p=>p.UserId== toRefuse.ProviderUserId);
            provider.ProviderAnalytics.Refuses++;
            toRefuse.Status = ReservationStatus.Cancelled;
            // Booking timeline tracker
            toRefuse.Events.Add(
                new ReservationEvent
                {
                    ReservationEventType = ReservationEventType.Rejected,
                    CreatedAt = DateTime.UtcNow,
                    Notes = "Reservation Rejected"
                }
                );
            await _dataContext.SaveChangesAsync();
            return toRefuse;
        }
        public async Task<Reservation> CompletedReservationAsync(int id)
        {
            var toComplete = await _dataContext.Reservations
                .Include(r => r.Client)
                .Include(r => r.Provider)
                .FirstOrDefaultAsync(r => r.ReservationId == id);

            if (toComplete == null)
                return null;
            var provider = await _dataContext.Providers.FirstOrDefaultAsync(p => p.UserId == toComplete.ProviderUserId);
            provider.ProviderAnalytics.Completions++;
            toComplete.Status = ReservationStatus.Completed;
            // Booking timeline tracker
            toComplete.Events.Add(
                new ReservationEvent
                {
                    ReservationEventType = ReservationEventType.Completed,
                    CreatedAt = DateTime.UtcNow,
                    Notes = "Reservation Completed"
                }
                );
            await _dataContext.SaveChangesAsync();
            return toComplete;

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
                .Include(r => r.Provider)
                    .ThenInclude(p => p.ProfileImage)
                 .Include(r=>r.Provider)
                    .ThenInclude(p=>p.Portfolio)
                .Include(r=> r.Client)
                .Include(r => r.Events)
                .Include(r => r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j=> j.Images)
                .Include(r=>r.Reviews)
                
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
                 .ThenInclude(p => p.ProfileImage)
                 .Include(r => r.Provider)
                    .ThenInclude(p => p.Portfolio)
                .Include(r => r.Events)
                .Include(r=> r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j=> j.Images)
                .Include(r => r.Reviews)
                .ToListAsync();
            if (reservations == null)
                //throw new NullReferenceException();
                return null;

            return reservations;
            
        }

       public async Task<Reservation> MakeReservationAsync(Reservation reservation)
        {
            
            var provider = await _dataContext.Providers.SingleOrDefaultAsync(p => p.UserId == reservation.ProviderUserId);
            var service = await _serviceRepository.GetServiceByIdAsync(provider.ServiceId);
            var client = await _clientRepository.GetClientByIdAsync(reservation.ClientUserId);
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
                .Where(r=> r.ClientUserId == clientId)
                .Include(r => r.Client)
                .Include(r => r.Provider)
                    .ThenInclude(p => p.ProfileImage)
                 .Include(r => r.Provider)
                    .ThenInclude(p => p.Portfolio)
                .Include(r => r.Events)
                .Include(r => r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j => j.Images)
                .Include(r => r.Reviews)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            if (reservations == null)
                
                return null;

            return reservations;
            
        }
    
        public async Task<List<Reservation>> GetReservationByProviderIdAsync(int providerId)
        {
            var reservations = await _dataContext.Reservations
                .OrderByDescending(r=>r.CreatedAt)
                .Where(r => r.ProviderUserId == providerId)
                .Include(r => r.Client)
                .Include(r => r.Provider)
                     .ThenInclude(p => p.ProfileImage)
                 .Include(r => r.Provider)
                    .ThenInclude(p => p.Portfolio)
                .Include(r => r.Events)
                .Include(r => r.Location)
                .Include(r => r.JobDescription)
                    .ThenInclude(j => j.Images)
                .Include(r => r.Reviews)
               
                .ToListAsync();
            if (reservations == null)
                
                return null;

            return reservations;

        }
    }
}
