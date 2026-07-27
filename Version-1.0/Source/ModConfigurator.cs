using Bindito.Core;
using Timberborn.Stockpiles;
using Timberborn.TemplateInstantiation;
using Timberborn.TemplateSystem;

namespace Calloatti.PausableStorage
{
  [Context("Game")]
  public class PausableStorageConfigurator : Configurator
  {
    protected override void Configure()
    {
      Bind<PausableStorageComponent>().AsTransient();
      MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
    }

    private static TemplateModule ProvideTemplateModule()
    {
      TemplateModule.Builder builder = new TemplateModule.Builder();

      // Inject the pause capability into the vanilla stockpiles
      builder.AddDecorator<StockpileSpec, PausableStorageComponent>();

      return builder.Build();
    }
  }
}