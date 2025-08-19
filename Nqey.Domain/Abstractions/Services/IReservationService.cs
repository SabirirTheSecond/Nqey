using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IReservationService
    {
        Task<Reservation> MakeReservationAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<List<Reservation>> GetReservationByClientIdAsync(int clientId);
        Task<List<Reservation>> GetReservationByProviderIdAsync(int providerId);
        Task<List<Domain.Reservation>> GetMyReservationsAsync(int userId);
        Task<Reservation> CancelReservationAsync(int id);
        Task<Reservation> AcceptReservationAsync(int id);
        Task<Reservation> DeleteReservationAsync(int id);
        Task<Reservation> UpdateReservationAsync(int id, Reservation reservation);
        Location GetReservationLocation(Reservation reservation);

    }
}
