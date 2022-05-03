using CronParser.Lib.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CronParser.Lib.Extensions
{
    public static class ServiceCollectionExt
    {
        public static IServiceCollection ConfigureParserServices(this IServiceCollection services)
        {
            services.AddSingleton<ICronExpressionParser, CronExpressionParser>();
            services.AddSingleton<ICronExpressionWriter, CronExpressionWriter>();
            return services;
        }
    }
}
