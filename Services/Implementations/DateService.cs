using System;
using System.Threading.Tasks;
using vega.Core.Utils;
using vega.Services.Interfaces;
using vegaplannerserver.Core;

namespace vega.Services.Implementations
{
    public class DateService : IDateService
    {
        public DateService(IBusinessDateRepository IBusinessDateRepository)
        {
            this.IBusinessDateRepository = IBusinessDateRepository;
        }

        public IBusinessDateRepository IBusinessDateRepository { get; }

        public DateTime GetCurrentDate() {
            if(SystemDate.Instance.date==null)
                SystemDate.Instance.date = IBusinessDateRepository.GetBusinessDate().Result.CurrBusDate;

            return SystemDate.Instance.date;
        }

        public void SetCurrentDate(DateTime businessDate) {
            IBusinessDateRepository.SetBusinessDate(businessDate);
            SystemDate.Instance.date = IBusinessDateRepository.GetBusinessDate().Result.CurrBusDate;  //Cache the date and use
            Console.WriteLine("Current Business Date (From Cache) : " + SystemDate.Instance.date);
        }
    }
}