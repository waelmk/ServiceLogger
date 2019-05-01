using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWinService
{
    public class Worker
    {
        private static ILog log =
      LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void doSomeThing()
        {
            var a = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
            try
            {
                log.Info("----WorkerStart---");
                throw new Exception("new exp logging");
            }
            catch (Exception ex)
            {
                log.Error("----WorkerERROR---" + ex.Message);
                log.Fatal("----WorkerFATAL--");
            }

        }
    }
}
