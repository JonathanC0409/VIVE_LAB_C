using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vivelab.API.Consume;
using Vivelab.Modelos;
using Vivelab.Servicios.Interfaces;

namespace Vivelab.Servicios
{
    public class PlanService : IPlanService
    {
        public Task<bool> ElejirPlan(int planCodigo)
        {
            var plan = CRUD<Plan>.GetById(planCodigo);
            return Task.FromResult(plan != null);
        }
    }
}
