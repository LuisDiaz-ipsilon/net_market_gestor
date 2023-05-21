using Microsoft.AspNetCore.Mvc.Filters;

namespace NetMarketGestor.Filtros
{
    public class FiltroExcepcion : ExceptionFilterAttribute
    {
        private readonly ILogger<FiltroExcepcion> _logger;


        public FiltroExcepcion(ILogger<FiltroExcepcion> logger)
        {
            this._logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}
