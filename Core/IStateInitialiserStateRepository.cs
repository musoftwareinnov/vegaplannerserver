using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core.Models.States;

namespace vega.Core
{
    public interface IStateInitialiserStateRepository
    {
          void AddBeginning(StateInitialiserState stateInitialiserState);
          void AddAfter(StateInitialiserState stateInitialiserState, int InsertAfterStateOrderId);
          Task<StateInitialiserState> GetStateInitialiserState(int id);
          void Update(StateInitialiserState stateInitialiserState);
    }
}