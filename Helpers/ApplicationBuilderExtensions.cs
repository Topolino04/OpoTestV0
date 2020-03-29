using DevExpress.Xpo;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace OpoTest {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseXpoDemoData(this IApplicationBuilder app) {
            using(var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
                UnitOfWork uow = new UnitOfWork( scope.ServiceProvider.GetService<IDataLayer>());
                DemoDataHelper.Seed(uow);
            }
            return app;
        }
    }
}
