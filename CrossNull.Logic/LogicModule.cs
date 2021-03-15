using CrossNull.Data;
using CrossNull.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic
{
    public class LogicModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<GameContext>().ToSelf();
            Bind<IStatisticService>().To<StatisticService>();
        }
    }
}
