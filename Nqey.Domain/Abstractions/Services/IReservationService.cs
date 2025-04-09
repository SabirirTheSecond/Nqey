using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IReservationService
    {
        Task<Reservation> MakeReservationAsync(Reservation reservation);
        Task<List<Reservation>> GetReservationsAsync();
        Task<Reservation> GetReservationByIdAsync(int id);
        Task<Reservation> CancelReservationAsync(int id);
        Task<Reservation> AcceptReservationAsync(int id);
        Task<Reservation> DeleteReservationAsync(int id);
        Task<Reservation> UpdateReservationAsync(int id, Reservation reservation);

    }
}
