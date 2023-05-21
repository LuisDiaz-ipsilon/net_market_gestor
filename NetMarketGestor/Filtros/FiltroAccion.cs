using Microsoft.AspNetCore.Mvc.Filters;

namespace NetMarketGestor.Filtros
{
    public class FiltroAccion : IActionFilter
    {
        private readonly ILogger<FiltroAccion> _logger;

        public FiltroAccion(ILogger<FiltroAccion> logger)
        {
            this._logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Antes de ejecutar la accion.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Despes de ejecutar la accion");
        }
    }
}
